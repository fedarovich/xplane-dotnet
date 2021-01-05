using System;
using System.Buffers;
using System.Buffers.Text;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace XP.SDK.Analyzers
{
    [Generator]
    public class Utf8StringFormatGenerator : ISourceGenerator
    {
        private static readonly SymbolDisplayFormat MethodSignatureFormat = new (
            globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.Included,
            typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
            genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters | SymbolDisplayGenericsOptions.IncludeTypeConstraints | SymbolDisplayGenericsOptions.IncludeVariance,
            memberOptions: SymbolDisplayMemberOptions.IncludeType | SymbolDisplayMemberOptions.IncludeParameters | SymbolDisplayMemberOptions.IncludeRef | SymbolDisplayMemberOptions.IncludeConstantValue,
            parameterOptions: SymbolDisplayParameterOptions.IncludeExtensionThis | SymbolDisplayParameterOptions.IncludeParamsRefOut | SymbolDisplayParameterOptions.IncludeType | SymbolDisplayParameterOptions.IncludeName ,
            miscellaneousOptions: SymbolDisplayMiscellaneousOptions.UseSpecialTypes | SymbolDisplayMiscellaneousOptions.EscapeKeywordIdentifiers);

        private static readonly SymbolDisplayFormat MethodModifiersFormat = new (
            memberOptions: SymbolDisplayMemberOptions.IncludeAccessibility | SymbolDisplayMemberOptions.IncludeModifiers);

        private static readonly IReadOnlyDictionary<SpecialType, string> Formats = new Dictionary<SpecialType, string>
        {
            [SpecialType.System_Boolean] = "lG",
            [SpecialType.System_DateTime] = "lGOR",
            // Integer types
            [SpecialType.System_Byte] = "dgnxDGNX",
            [SpecialType.System_Int16] = "dgnxDGNX",
            [SpecialType.System_Int32] = "dgnxDGNX",
            [SpecialType.System_Int64] = "dgnxDGNX",
            [SpecialType.System_IntPtr] = "dgnxDGNX",
            [SpecialType.System_SByte] = "dgnxDGNX",
            [SpecialType.System_UInt16] = "dgnxDGNX",
            [SpecialType.System_UInt32] = "dgnxDGNX",
            [SpecialType.System_UInt64] = "dgnxDGNX",
            [SpecialType.System_UIntPtr] = "dgnxDGNX",
            // Floating point types
            [SpecialType.System_Single] = "efgEFG",
            [SpecialType.System_Double] = "efgEFG",
            [SpecialType.System_Decimal] = "efgEFG",
        };

        private static readonly DiagnosticDescriptor Rule8200 = new (
            "XPSDK8200",
            "Format string is invalid",
            "The format string has invalid format",
            "Usage",
            DiagnosticSeverity.Error,
            true);

        private static readonly DiagnosticDescriptor Rule8201 = new (
            "XPSDK8201",
            "Format string contains invalid parameter index",
            "Invalid parameter index {0}",
            "Usage",
            DiagnosticSeverity.Error,
            true);

        private static readonly DiagnosticDescriptor Rule8202 = new(
            "XPSDK8202",
            "Format string contains invalid parameter name",
            "Invalid parameter name '{0}'",
            "Usage",
            DiagnosticSeverity.Error,
            true);

        private static readonly DiagnosticDescriptor Rule8203 = new(
            "XPSDK8203",
            "Unsupported parameter type",
            "The type of parameter '{0}' must either be of one of the predefined types or implement XP.SDK.Text.IUtf8Formattable",
            "Usage",
            DiagnosticSeverity.Error,
            true);

        private static readonly DiagnosticDescriptor Rule8204 = new(
            "XPSDK8204",
            "Invalid initial buffer capacity",
            "The initial buffer capacity must be greater than or equal to 0",
            "Usage",
            DiagnosticSeverity.Error,
            true);

        private static readonly DiagnosticDescriptor Rule8205 = new(
            "XPSDK8205",
            "Unsupported format symbol",
            "The type '{0}' does not support format '{1}'. The following formats are allowed: {2}.",
            "Usage",
            DiagnosticSeverity.Error,
            true);

        private static readonly Regex ParameterNameRegex = new (@"^[\w-[\d]][\w]{0,511}$", RegexOptions.Compiled);
        
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is not SyntaxReceiver syntaxReceiver)
                return;

            var compilation = context.Compilation;

            if (compilation.GetTypeByMetadataName("XP.SDK.Utf8StringFormatAttribute") is not { } utf8FormatAttributeSymbol)
                return;

            if (compilation.GetTypeByMetadataName("XP.SDK.Utf8String") is not { } utf8StringSymbol)
                return;

            if (compilation.GetTypeByMetadataName("XP.SDK.Utf8StringScope") is not { } utf8StringScopeSymbol)
                return;

            if (compilation.GetTypeByMetadataName("XP.SDK.Text.Utf8StringBuilderFactory") is not { } utf8StringBuilderFactorySymbol)
                return;

            if (compilation.GetTypeByMetadataName("XP.SDK.Text.Utf8StringBuilder") is not { } utf8StringBuilderSymbol)
                return;

            if (compilation.GetTypeByMetadataName("XP.SDK.Text.IUtf8Formattable") is not { } utf8FormattableSymbol)
                return;

            if (compilation.GetTypeByMetadataName("XP.SDK.Text.SupportedFormatAttribute") is not { } supportedFormatAttributeSymbol)
                return;

            if (compilation.GetTypeByMetadataName("System.ReadOnlySpan`1") is not { } readOnlySpanSymbol)
                return;

            if (compilation.GetTypeByMetadataName("System.TimeSpan") is not { } timeSpanSymbol)
                return;

            if (compilation.GetTypeByMetadataName("System.DateTimeOffset") is not { } dateTimeOffsetSymbol)
                return;

            if (compilation.GetTypeByMetadataName("System.Guid") is not { } guidSymbol)
                return;
            
            if (compilation.GetTypeByMetadataName("System.Runtime.InteropServices.InAttribute") is not { } inAttributeSymbol)
                return;

            var byteSymbol = compilation.GetSpecialType(SpecialType.System_Byte);

            var readOnlySpanOfByteSymbol = readOnlySpanSymbol.Construct(byteSymbol);

            var builder = new StringBuilder();
            var toolName = GetType().FullName;
            var toolVersion = Assembly.GetExecutingAssembly()
                .GetCustomAttribute<AssemblyFileVersionAttribute>()
                .Version;
            var utf8 = new UTF8Encoding(false);

            var groups = GetMethods().GroupBy(
                m => m.method.ContainingType,
                SymbolEqualityComparer.Default);

            foreach (var group in groups)
            {
                var source = Generate(group);
                var fileName = GetFileName(group.Key);
                context.AddSource(fileName, source);
            }

            IEnumerable<(IMethodSymbol method, FormatInfo formatInfo)> GetMethods()
            {
                foreach (var candidate in syntaxReceiver.Candidates)
                {
                    if (!candidate.Modifiers.Any(SyntaxKind.PartialKeyword))
                        continue;

                    var model = compilation.GetSemanticModel(candidate.SyntaxTree);
                    if (!ReturnsUtf8String(model, candidate.ReturnType, out bool scoped))
                        continue;

                    if (model.GetDeclaredSymbol(candidate) is not { } methodSymbol)
                        continue;

                    if (!TryGetFormat(methodSymbol, out var format, out var formatAttribute))
                        continue;

                    yield return (methodSymbol, new FormatInfo(format, formatAttribute, scoped));
                }
            }

            bool ReturnsUtf8String(SemanticModel model, TypeSyntax returnType, out bool scoped)
            {
                if (model.GetSymbolInfo(returnType).Symbol is INamedTypeSymbol s)
                {
                    if (SymbolEqualityComparer.Default.Equals(s, utf8StringSymbol))
                    {
                        scoped = false;
                        return true;
                    }
                    
                    if (SymbolEqualityComparer.Default.Equals(s, utf8StringScopeSymbol))
                    {
                        scoped = true;
                        return true;
                    }
                }

                scoped = false;
                return false;
            }

            bool TryGetFormat(IMethodSymbol symbol, out string format, out AttributeData formatAttribute)
            {
                formatAttribute = null;
                
                foreach (var attribute in symbol.GetAttributes())
                {
                    if (!SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, utf8FormatAttributeSymbol))
                        continue;

                    formatAttribute = attribute;
                    var args = attribute.ConstructorArguments;
                    if (args.Length != 1)
                        continue;

                    if (args[0].Value is string || args[0].Value == null)
                    {
                        format = args[0].Value as string;
                        return true;
                    }
                }

                format = default;
                return false;
            }

            string GetFileName(ISymbol type)
            {
                builder.Clear();

                foreach (var part in type.ContainingNamespace.ToDisplayParts())
                {
                    if (part.Symbol is { Name: var name } && !string.IsNullOrEmpty(name))
                    {
                        builder.Append(name);
                        builder.Append('_');
                    }
                }
                builder.Append(type.Name);
                builder.Append("_Utf8Format.cs");

                return builder.ToString();
            }

            string Generate(IGrouping<ISymbol, (IMethodSymbol method, FormatInfo format)> group)
            {
                var containingType = (INamedTypeSymbol)group.Key;

                builder.Clear();
                using var writer = new IndentedTextWriter(new StringWriter(builder, CultureInfo.InvariantCulture));

                var hasNamespace = !string.IsNullOrEmpty(containingType.ContainingNamespace?.Name);
                if (hasNamespace)
                {
                    writer.WriteLine("namespace {0}", containingType.ContainingNamespace.ToDisplayString());
                    writer.WriteLine("{");
                    writer.Indent++;
                }

                writer.WriteLine("using XP.SDK.Text;");
                writer.WriteLine();
                
                writer.WriteLine("// Generated by \"{0}\", version \"{1}\"", toolName, toolVersion);
                writer.WriteLine("partial {0} {1}",
                    containingType.IsValueType ? "struct" : "class",
                    containingType.Name);
                writer.WriteLine("{");

                writer.Indent++;

                foreach (var (method, formatInfo) in group)
                {
                    var modifiers = method.ToDisplayParts(MethodModifiersFormat);
                    foreach (var modifier in modifiers.Where(modifier => modifier.Kind != SymbolDisplayPartKind.MethodName))
                    {
                        writer.Write(modifier.ToString());
                    }
                    
                    writer.Write("partial ");
                    writer.Write(method.ToDisplayString(MethodSignatureFormat));

                    var attributeLocation = formatInfo.FormatAttribute.ApplicationSyntaxReference?.GetSyntax().GetLocation();
                    var (formatParts, literal) = ParseFormatString(formatInfo.Format, attributeLocation);
                    if (formatParts.Count == 1)
                    {
                        switch (formatParts[0])
                        {
                            case LiteralFormatPart:
                                var bytes = utf8.GetBytes(literal);
                                writer.Write(formatInfo.Scoped ? " => new (" : " => ");
                                writer.Write("new {0}(new byte[] {{ ", utf8StringSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
                                foreach (var b in bytes)
                                {
                                    writer.Write("0x{0:X2}, ", b);
                                }
                                writer.Write("0x00 }}, {0})", bytes.Length);
                                writer.WriteLine(formatInfo.Scoped ? ", null);" : ";");
                                writer.WriteLine();
                                continue;
                            case NullFormatPart:
                                writer.WriteLine(" => default;");
                                writer.WriteLine();
                                continue;
                            case EmptyFormatPart:
                                writer.WriteLine(formatInfo.Scoped ? " => new ({0}.Empty, null);" : " => {0}.Empty;", 
                                    utf8StringSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
                                writer.WriteLine();
                                continue;
                            case ThrowFormatPart:
                                writer.WriteLine(" => throw new global::System.FormatException(\"The format string is invalid.\");");
                                writer.WriteLine();
                                continue;
                        }
                    }

                    writer.WriteLine();
                    writer.WriteLine("{");
                    writer.Indent++;

                    var formatProperties = formatInfo.FormatAttribute.NamedArguments
                        .ToImmutableDictionary(kv => kv.Key, kv => kv.Value);
                    
                    // Declare literal.
                    writer.Write(readOnlySpanOfByteSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
                    writer.Write(" __literal__ = ");
                    if (string.IsNullOrEmpty(literal))
                    {
                        writer.WriteLine("default;");
                    }
                    else
                    {
                        var literalBytes = utf8.GetBytes(literal);
                        writer.Write("new byte[] { ");
                        writer.WriteBytes(literalBytes);
                        writer.WriteLine("};");
                    }

                    // Declare nullable substitute.
                    bool hasNullSubstitute = false;
                    if (formatProperties.TryGetValue("NullDisplayText", out var nullLiteralConstant) 
                        && nullLiteralConstant.Value is string nullLiteral
                        && !string.IsNullOrEmpty(nullLiteral))
                    {
                        hasNullSubstitute = true;
                        writer.Write(readOnlySpanOfByteSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
                        writer.Write(" __null__ = new byte[] { ");
                        writer.WriteBytes(utf8.GetBytes(nullLiteral));
                        writer.WriteLine("};");
                    }

                    // Declare builder.
                    writer.Write("var __builder__ = ");
                    writer.Write(utf8StringBuilderFactorySymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
                    writer.Write(formatInfo.Scoped ? ".SharedPooled.CreateBuilder(" : ".Shared.CreateBuilder(");
                    if (formatProperties.TryGetValue("InitialBufferCapacity", out var initialBufferCapacityConstant))
                    {
                        var initialBufferCapacity = initialBufferCapacityConstant.Value as int?;
                        if (initialBufferCapacity >= 0)
                        {
                            writer.Write(initialBufferCapacity.Value);
                        }
                        else if (initialBufferCapacity != null)
                        {
                            context.ReportDiagnostic(Diagnostic.Create(Rule8204, attributeLocation));
                        }
                    }
                    writer.WriteLine(");");
                    
                    // Try block
                    writer.WriteLine("try");
                    writer.WriteLine("{");
                    writer.Indent++;
                    
                    foreach (var part in formatParts)
                    {
                        switch (part)
                        {
                            case LiteralFormatPart literalPart:
                                writer.Write("__builder__.Append(__literal__.Slice(");
                                writer.Write(literalPart.Offset);
                                if (literalPart.Length != null)
                                {
                                    builder.Append(", ");
                                    builder.Append(literalPart.Length.Value);
                                }
                                writer.WriteLine("));");
                                break;
                            case NewLineFormatPart:
                                writer.WriteLine("__builder__.AppendLine();");
                                break;
                            case StandardFormatPart { Index: { } index, Format: var format }:
                                if (index < 0 || index >= method.Parameters.Length)
                                {
                                    context.ReportDiagnostic(Diagnostic.Create(Rule8201, attributeLocation, index));
                                    writer.WriteLine("throw new global::System.FormatException(\"Invalid parameter index {0}.\");", index);
                                    goto LOOP_END;
                                }

                                var indexedParam = method.Parameters[index];
                                WriteFormatPart(indexedParam, format, hasNullSubstitute);
                                break;
                            case StandardFormatPart { Name: { } name, Format: var format }:
                                var namedParam = method.Parameters.FirstOrDefault(p => p.Name == name);
                                if (namedParam == null)
                                {
                                    context.ReportDiagnostic(Diagnostic.Create(Rule8202, attributeLocation, name));
                                    writer.WriteLine("throw new global::System.FormatException(\"Invalid parameter name '{0}'.\");", name);
                                    goto LOOP_END;
                                }
                                WriteFormatPart(namedParam, format, hasNullSubstitute);
                                break;
                            case StandardFormatPart:
                                context.ReportDiagnostic(Diagnostic.Create(Rule8200, attributeLocation));
                                writer.WriteLine("throw new global::System.FormatException(\"The format string is invalid.\");");
                                break;
                        }
                    }
                    LOOP_END:
                    
                    // Return result.
                    writer.WriteLine(formatInfo.Scoped ? "return __builder__.BuildScoped();" : "return __builder__.Build();");

                    // Catch block
                    writer.Indent--;
                    writer.WriteLine("}");
                    writer.WriteLine("catch");
                    writer.WriteLine("{");
                    writer.Indent++;
                    writer.WriteLine("__builder__.Dispose();");
                    writer.WriteLine("throw;");
                    writer.Indent--;
                    writer.WriteLine("}");
                    
                    writer.Indent--;
                    writer.WriteLine("}");
                    writer.WriteLine();
                }

                writer.Indent--;

                writer.WriteLine("}");

                if (writer.Indent > 0)
                {
                    writer.Indent = 0;
                    writer.WriteLine("}");
                }

                writer.Flush();
                return builder.ToString();

                void WriteFormatPart(IParameterSymbol parameter, StandardFormat format, bool hasNullSubstitute)
                {
                    bool isNullable = parameter.Type.IsValueType && parameter.Type.NullableAnnotation == NullableAnnotation.Annotated;
                    if (isNullable || parameter.Type.IsReferenceType)
                    {
                        writer.WriteLine("if ({0} != null)", parameter.Name);
                        writer.WriteLine("{");
                        writer.Indent++;
                        WriteNotNull(isNullable 
                            ? ((INamedTypeSymbol) parameter.Type).TypeArguments[0] 
                            : parameter.Type);
                        writer.Indent--;
                        writer.WriteLine("}");
                        if (hasNullSubstitute)
                        {
                            writer.WriteLine("else");
                            writer.WriteLine("{");
                            writer.Indent++;
                            writer.WriteLine("__builder__.Append(__null__);");
                            writer.Indent--;
                            writer.WriteLine("}");
                        }
                    }
                    else
                    {
                        WriteNotNull(parameter.Type);
                    }

                    void WriteNotNull(ITypeSymbol type)
                    {
                        switch (type.SpecialType)
                        {
                            case SpecialType.System_Boolean:
                            case SpecialType.System_SByte:
                            case SpecialType.System_Byte:
                            case SpecialType.System_Int16:
                            case SpecialType.System_UInt16:
                            case SpecialType.System_Int32:
                            case SpecialType.System_UInt32:
                            case SpecialType.System_Int64:
                            case SpecialType.System_UInt64:
                            case SpecialType.System_Decimal:
                            case SpecialType.System_Single:
                            case SpecialType.System_Double:
                            case SpecialType.System_IntPtr:
                            case SpecialType.System_UIntPtr:
                            case SpecialType.System_DateTime:
                                VerifyFormatOfSpecialType(type, format);
                                WriteCore();
                                return;
                            case SpecialType.System_Char:
                            case SpecialType.System_String:
                                WriteCore();
                                return;
                        }

                        if (SymbolEqualityComparer.Default.Equals(type, utf8StringSymbol))
                        {
                            WriteCore();
                        }
                        else if (SymbolEqualityComparer.Default.Equals(type, timeSpanSymbol))
                        {
                            VerifyFormat(type, format, "cgtGT");
                            WriteCore();
                        }
                        else if (SymbolEqualityComparer.Default.Equals(type, dateTimeOffsetSymbol))
                        {
                            VerifyFormat(type, format, "lGOR");
                            WriteCore();
                        }
                        else if (SymbolEqualityComparer.Default.Equals(type, guidSymbol))
                        {
                            VerifyFormat(type, format, "BDNP");
                            WriteCore();
                        }
                        else if (type is INamedTypeSymbol { EnumUnderlyingType: not null })
                        {
                            VerifyFormat(type, format, "dfgxDFGX");
                            WriteCore();
                        }
                        else if (type is INamedTypeSymbol namedType && namedType.IsSubtypeOf(utf8FormattableSymbol))
                        {
                            VerifyFormatOfFormattable(type, format);
                            WriteCore(true);
                        }
                        else
                        {
                            // TODO: Consider user extension method lookup.
                            context.ReportDiagnostic(Diagnostic.Create(Rule8203, parameter.DeclaringSyntaxReferences[0].GetSyntax().GetLocation(), parameter.Name));
                            writer.WriteLine("throw new global::System.ArgumentException(\"The argument must be of one of the types supported by XP.SDK.Text.Utf8StringBuilder.\", nameof({0}));", parameter.Name);
                        }
                        
                        void WriteCore(bool byRefIfNeeded = false)
                        {
                            writer.Write("__builder__.Append(");
                            if (byRefIfNeeded && parameter.RefCustomModifiers.Any(m => SymbolEqualityComparer.Default.Equals(m.Modifier, inAttributeSymbol)))
                            {
                                writer.Write("in ");
                            }
                            writer.Write(parameter.Name);
                            if (isNullable)
                            {
                                writer.Write(".Value");
                            }
                            if (!format.IsDefault)
                            {
                                writer.Write(", new ('{0}'", format.Symbol);
                                if (format.HasPrecision)
                                {
                                    writer.Write(", {0})", format.Precision);
                                }
                                else
                                {
                                    writer.Write(")");
                                }
                            }

                            writer.WriteLine(");");
                        }
                    }

                    void VerifyFormatOfSpecialType(ITypeSymbol type, StandardFormat standardFormat)
                    {
                        if (standardFormat.IsDefault || !Formats.TryGetValue(type.SpecialType, out var allowedFormats))
                            return;

                        VerifyFormat(type, standardFormat, allowedFormats);
                    }

                    void VerifyFormatOfFormattable(ITypeSymbol type, StandardFormat standardFormat)
                    {
                        if (standardFormat.IsDefault)
                            return;
                        
                        var allowedFormats = type.GetAttributes()
                            .Where(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, supportedFormatAttributeSymbol))
                            .Select(a => a.ConstructorArguments.FirstOrDefault().Value as char?)
                            .Where(c => c != null)
                            .Select(c => c.Value)
                            .Distinct()
                            .OrderBy(c => c)
                            .ToList();
                        
                        if (allowedFormats.Count == 0)
                            return;

                        VerifyFormat(type, standardFormat, allowedFormats);
                    }

                    void VerifyFormat(ITypeSymbol type, StandardFormat standardFormat, IEnumerable<char> allowedFormats)
                    {
                        // ReSharper disable PossibleMultipleEnumeration
                        if (!standardFormat.IsDefault && !allowedFormats.Contains(standardFormat.Symbol))
                        {
                            context.ReportDiagnostic(Diagnostic.Create(
                                Rule8205,
                                parameter.DeclaringSyntaxReferences[0].GetSyntax().GetLocation(),
                                type.ToString(),
                                standardFormat.Symbol,
                                string.Join(", ", allowedFormats.Select(c => $"'{c}'"))));
                        }
                        // ReSharper restore PossibleMultipleEnumeration
                    }
                }
            }

            (IReadOnlyList<FormatPart> parts, string literal) ParseFormatString(string format, Location location)
            {
                if (string.IsNullOrEmpty(format))
                    return (new FormatPart[] { format == null ? new NullFormatPart() : new EmptyFormatPart()}, null);
                
                var literalBuilder = new StringBuilder(format.Length);
                var parts = new List<FormatPart>();
                int scanIndex = 0;
                int endIndex = format.Length;

                // TODO: Ensure all invalid formats are handled.
                while (scanIndex < endIndex)
                {
                    int openBraceIndex = FindBraceIndex(format, '{', scanIndex, endIndex);
                    if (scanIndex == 0 && openBraceIndex == endIndex)
                    {
                        // No holes found.
                        if (FindBraceIndex(format, '}', scanIndex, endIndex) == endIndex)
                        {
                            literalBuilder.Append(format)
                                .Replace("{{", "{")
                                .Replace("}}", "}");
                            parts.Add(new LiteralFormatPart());
                        }
                        else
                        {
                            context.ReportDiagnostic(Diagnostic.Create(Rule8200, location));
                            parts.Add(new ThrowFormatPart());
                        }

                        break;
                    }

                    int closeBraceIndex = FindBraceIndex(format, '}', openBraceIndex, endIndex);

                    if (closeBraceIndex == endIndex)
                    {
                        var literalPos = literalBuilder.Length;
                        literalBuilder.Append(format.AsSpan(scanIndex))
                            .Replace("{{", "{", literalPos, literalBuilder.Length - literalPos)
                            .Replace("}}", "}", literalPos, literalBuilder.Length - literalPos);
                        parts.Add(new LiteralFormatPart(literalPos));
                        scanIndex = endIndex;
                    }
                    else
                    {
                        var literalPos = literalBuilder.Length;
                        if (scanIndex != openBraceIndex)
                        {
                            literalBuilder.Append(format.AsSpan(scanIndex, openBraceIndex - scanIndex))
                                .Replace("{{", "{", literalPos, literalBuilder.Length - literalPos)
                                .Replace("}}", "}", literalPos, literalBuilder.Length - literalPos);
                            parts.Add(new LiteralFormatPart(literalPos, literalBuilder.Length - literalPos));
                        }

                        // Format item syntax : { index[ :formatString] }.
                        var formatSpan = format.AsSpan(openBraceIndex + 1, closeBraceIndex - openBraceIndex - 1);
                        int formatDelimiterIndex = formatSpan.IndexOf(':');

                        var formatIndexSpan = formatDelimiterIndex >= 0
                            ? formatSpan.Slice(0, formatDelimiterIndex)
                            : formatSpan;

                        if (formatIndexSpan.Length == 0)
                        {
                            parts.Add(new ThrowFormatPart());
                            context.ReportDiagnostic(Diagnostic.Create(Rule8200, location));
                            goto CONT;
                        }

                        if (formatIndexSpan.Length == 1 && formatIndexSpan[0] == '\n')
                        {
                            parts.Add(new NewLineFormatPart());
                            goto CONT;
                        }

                        var formatStringSpan = formatDelimiterIndex >= 0
                            ? formatSpan.Slice(formatDelimiterIndex + 1)
                            : default;

                        var standardFormat = default(StandardFormat);
                        if (!formatStringSpan.IsEmpty)
                        {
                            try
                            {
                                standardFormat = StandardFormat.Parse(formatStringSpan);
                            }
                            catch
                            {
                                parts.Add(new ThrowFormatPart());
                                context.ReportDiagnostic(Diagnostic.Create(Rule8200, location));
                                goto CONT;
                            }
                        }

                        var formatIndexString = formatIndexSpan.AsString();
                        if (int.TryParse(formatIndexString, out var parameterIndex))
                        {
                            parts.Add(new StandardFormatPart(parameterIndex, standardFormat));
                        }
                        else if (ParameterNameRegex.IsMatch(formatIndexString))
                        {
                            parts.Add(new StandardFormatPart(formatIndexString, standardFormat));
                        }
                        else
                        {
                            parts.Add(new ThrowFormatPart());
                            context.ReportDiagnostic(Diagnostic.Create(Rule8200, location));
                        }

                        CONT:
                        scanIndex = closeBraceIndex + 1;
                    }
                }

                if (parts.OfType<ThrowFormatPart>().Any())
                    return (new FormatPart[] { new ThrowFormatPart() }, null);

                return (parts, literalBuilder.ToString());

                static int FindBraceIndex(string format, char brace, int startIndex, int endIndex)
                {
                    // Example: {{prefix{{{Argument}}}suffix}}.
                    int braceIndex = endIndex;
                    int scanIndex = startIndex;
                    int braceOccurrenceCount = 0;

                    while (scanIndex < endIndex)
                    {
                        if (braceOccurrenceCount > 0 && format[scanIndex] != brace)
                        {
                            if (braceOccurrenceCount % 2 == 0)
                            {
                                // Even number of '{' or '}' found. Proceed search with next occurrence of '{' or '}'.
                                braceOccurrenceCount = 0;
                                braceIndex = endIndex;
                            }
                            else
                            {
                                // An unescaped '{' or '}' found.
                                break;
                            }
                        }
                        else if (format[scanIndex] == brace)
                        {
                            if (brace == '}')
                            {
                                if (braceOccurrenceCount == 0)
                                {
                                    // For '}' pick the first occurrence.
                                    braceIndex = scanIndex;
                                }
                            }
                            else
                            {
                                // For '{' pick the last occurrence.
                                braceIndex = scanIndex;
                            }

                            braceOccurrenceCount++;
                        }

                        scanIndex++;
                    }

                    return braceIndex;
                }
            }
        }

        private class SyntaxReceiver : ISyntaxReceiver
        {
            private readonly List<MethodDeclarationSyntax> _candidates = new();

            public IReadOnlyList<MethodDeclarationSyntax> Candidates => _candidates;

            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                if (syntaxNode is MethodDeclarationSyntax method &&
                    method.AttributeLists.Count > 0)
                {
                    _candidates.Add(method);
                }
            }
        }

        private readonly struct FormatInfo
        {
            public FormatInfo(string format, AttributeData formatAttribute, bool scoped)
            {
                Format = format;
                Scoped = scoped;
                FormatAttribute = formatAttribute;
            }

            public readonly string Format;

            public readonly AttributeData FormatAttribute;
            
            public readonly bool Scoped;
            
        }

        [Flags]
        private enum Modifiers
        {
            None = 0,
            Static = 1 << 0,
            Virtual = 1 << 1,
            Override = 1 << 2,
            Sealed = 1 << 3,
        }

        private abstract record FormatPart;

        private sealed record NullFormatPart : FormatPart;

        private sealed record EmptyFormatPart : FormatPart;

        private sealed record LiteralFormatPart(int Offset = 0, int? Length = null) : FormatPart;

        private sealed record NewLineFormatPart : FormatPart;

        private sealed record ThrowFormatPart : FormatPart;

        private sealed record StandardFormatPart : FormatPart
        {
            public StandardFormatPart(int index, StandardFormat format) => (Index, Format) = (index, format);

            public StandardFormatPart(string name, StandardFormat format) => (Name, Format) = (name, format);

            public int? Index { get; }

            public string Name { get; }

            public StandardFormat Format { get; }
        }
    }
}
