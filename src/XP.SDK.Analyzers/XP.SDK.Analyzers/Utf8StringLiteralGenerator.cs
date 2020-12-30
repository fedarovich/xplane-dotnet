using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace XP.SDK.Analyzers
{
    [Generator]
    public class Utf8StringLiteralGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is not SyntaxReceiver syntaxReceiver)
                return;

            var compilation = context.Compilation;

            if (compilation.GetTypeByMetadataName("XP.SDK.Utf8StringLiteralAttribute") is not { } utf8AttributeSymbol)
                return;
            
            if (compilation.GetTypeByMetadataName("XP.SDK.Utf8String") is not { } utf8StringSymbol)
                return;

            var utf8StringSymbolDisplayString = utf8StringSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            
            var builder = new StringBuilder();
            var toolName = GetType().FullName;
            var toolVersion = Assembly.GetExecutingAssembly()
                .GetCustomAttribute<AssemblyFileVersionAttribute>()
                .Version;
            var utf8 = new UTF8Encoding(false);

            var groups = GetMethods().GroupBy(
                m => m.containingType, 
                m => m.literalInfo,
                SymbolEqualityComparer.Default);
            
            
            foreach (var group in groups)
            {
                var source = Generate(group);
                var fileName = GetFileName(group.Key);
                context.AddSource(fileName, source);
            }
            
            IEnumerable<(INamedTypeSymbol containingType, LiteralInfo literalInfo)> GetMethods()
            {
                foreach (var candidate in syntaxReceiver.Candidates)
                {
                    if (!candidate.Modifiers.Any(SyntaxKind.PartialKeyword))
                        continue;
                    
                    if (candidate.ParameterList.Parameters.Any())
                        continue;

                    var model = compilation.GetSemanticModel(candidate.SyntaxTree);
                    if (!ReturnsUtf8String(model, candidate.ReturnType))
                        continue;

                    if (model.GetDeclaredSymbol(candidate) is not { } methodSymbol)
                        continue;
                        
                    if (!TryGetLiteral(methodSymbol, out var literal))
                        continue;

                    Modifiers modifiers = default;
                    if (candidate.Modifiers.Any(SyntaxKind.StaticKeyword)) modifiers |= Modifiers.Static;
                    if (candidate.Modifiers.Any(SyntaxKind.VirtualKeyword)) modifiers |= Modifiers.Virtual;
                    if (candidate.Modifiers.Any(SyntaxKind.OverrideKeyword)) modifiers |= Modifiers.Override;
                    if (candidate.Modifiers.Any(SyntaxKind.SealedKeyword)) modifiers |= Modifiers.Sealed;
                    
                    yield return (methodSymbol.ContainingType, new LiteralInfo(methodSymbol.DeclaredAccessibility, modifiers, methodSymbol.Name, literal));
                }
            }

            bool ReturnsUtf8String(SemanticModel model, TypeSyntax returnType) =>
                model.GetSymbolInfo(returnType).Symbol is INamedTypeSymbol s
                && SymbolEqualityComparer.Default.Equals(s, utf8StringSymbol);

            bool TryGetLiteral(IMethodSymbol symbol, out string literal)
            {
                foreach (var attribute in symbol.GetAttributes())
                {
                    if (!SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, utf8AttributeSymbol))
                        continue;
                    
                    var args = attribute.ConstructorArguments;
                    if (args.Length != 1)
                        continue;

                    if (args[0].Value is string || args[0].Value == null)
                    {
                        literal = args[0].Value as string;
                        return true;
                    }
                }

                literal = default;
                return false;
            }

            string Generate(IGrouping<ISymbol, LiteralInfo> group)
            {
                var containingType = (INamedTypeSymbol) group.Key;
                
                builder.Clear();
                using var writer = new IndentedTextWriter(new StringWriter(builder, CultureInfo.InvariantCulture));
                
                var hasNamespace = !string.IsNullOrEmpty(containingType.ContainingNamespace?.Name);
                if (hasNamespace)
                {
                    writer.WriteLine("namespace {0}", containingType.ContainingNamespace.ToDisplayString());
                    writer.WriteLine("{");
                    writer.Indent++;
                }
                
                writer.WriteLine("[global::System.CodeDom.Compiler.GeneratedCode(\"{0}\", \"{1}\")]", toolName, toolVersion);
                writer.WriteLine("partial {0} {1}",
                    containingType.IsValueType ? "struct" : "class",
                    containingType.Name);
                writer.WriteLine("{");

                writer.Indent++;

                foreach (var literalInfo in group)
                {
                    writer.WriteLine("/// <returns>Null-terminated UTF-8 representation of {0}.</returns>", 
                        literalInfo.Literal != null
                            ? "&quot;" + new XText(literalInfo.Literal) + "&quot;"
                            : "<see langword=\"null\" />");
                    writer.WriteLine("[global::System.Runtime.CompilerServices.MethodImplAttribute(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]");
                    writer.Write("{0} {1}{2}{3}{4}partial {5} {6}() => ",
                        AccessibilityToString(literalInfo.Accessibility),
                        literalInfo.Modifiers.HasFlag(Modifiers.Static) ? "static " : string.Empty,
                        literalInfo.Modifiers.HasFlag(Modifiers.Sealed) ? "sealed " : string.Empty,
                        literalInfo.Modifiers.HasFlag(Modifiers.Virtual) ? "virtual " : string.Empty,
                        literalInfo.Modifiers.HasFlag(Modifiers.Override) ? "override " : string.Empty,
                        utf8StringSymbolDisplayString,
                        literalInfo.Name);
                    
                    if (!string.IsNullOrEmpty(literalInfo.Literal))
                    {
                        writer.Write("new {0}(new byte[] {{ ", utf8StringSymbolDisplayString);

                        var bytes = utf8.GetBytes(literalInfo.Literal);
                        foreach (var b in bytes)
                        {
                            writer.Write("0x{0:X2}, ", b);
                        }

                        writer.WriteLine("0x00 }}, {0});", bytes.Length);
                    }
                    else if (literalInfo.Literal != null)
                    {
                        writer.WriteLine("global::XP.SDK.Utf8String.Empty;");
                    }
                    else
                    {
                        writer.WriteLine("default;");
                    }
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
                builder.Append("_Utf8Literal.cs");

                return builder.ToString();
            }

            static string AccessibilityToString(Accessibility access) =>
                access switch
                {
                    Accessibility.Private => "private",
                    Accessibility.ProtectedAndInternal => "private protected",
                    Accessibility.Protected => "protected",
                    Accessibility.Internal => "internal",
                    Accessibility.ProtectedOrInternal => "protected internal",
                    Accessibility.Public => "public",
                    _ => throw new ArgumentOutOfRangeException(nameof(access), access, null)
                };
        }

        private class SyntaxReceiver : ISyntaxReceiver
        {
            private readonly List<MethodDeclarationSyntax> _candidates = new ();

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

        private readonly struct LiteralInfo
        {
            public LiteralInfo(Accessibility accessibility, Modifiers modifiers, string name, string literal)
            {
                Accessibility = accessibility;
                Modifiers = modifiers;
                Name = name;
                Literal = literal;
            }
            
            public readonly Accessibility Accessibility;
            
            public readonly Modifiers Modifiers;

            public readonly string Name;

            public readonly string Literal;
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
    }
}
