using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

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
                context.AddSource(fileName, SourceText.From(source, Encoding.UTF8));
            }

            IEnumerable<(INamedTypeSymbol containingType, LiteralInfo literalInfo)> GetMethods()
            {
                foreach (var candidate in syntaxReceiver.Candidates)
                {
                    if (!candidate.Modifiers.Any(SyntaxKind.PartialKeyword))
                        continue;
                        
                    if (!candidate.Modifiers.Any(SyntaxKind.StaticKeyword))
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
                    
                    yield return (methodSymbol.ContainingType, new LiteralInfo(methodSymbol.DeclaredAccessibility, methodSymbol.Name, literal));
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

                    if (args[0].Value is not string s)
                        continue;
                    
                    literal = s;
                    return true;
                }

                literal = default;
                return false;
            }

            string Generate(IGrouping<ISymbol, LiteralInfo> group)
            {
                var containingType = (INamedTypeSymbol) group.Key;
                
                builder.Clear();
                builder
                    .AppendLine("using global::XP.SDK;")
                    .AppendLine();

                int ident = 0;
                var hasNamespace = !string.IsNullOrEmpty(containingType.ContainingNamespace.Name);
                if (hasNamespace)
                {
                    builder
                        .AppendFormat("namespace {0}", containingType.ContainingNamespace.ToDisplayString())
                        .AppendLine()
                        .AppendLine("{");
                    ident++;
                }
                
                builder
                    .AppendIdent(ident)
                    .AppendFormat("[global::System.CodeDom.Compiler.GeneratedCode(\"{0}\", \"{1}\")]", 
                        toolName, 
                        toolVersion)
                    .AppendLine()
                    .AppendIdent(ident)
                    .AppendFormat("partial {0} {1}", 
                        containingType.IsValueType ? "struct" : "class",
                        containingType.Name)
                    .AppendLine()
                    .AppendIdent(ident)
                    .AppendLine("{");

                ident++;

                foreach (var literalInfo in group)
                {
                    builder
                        .AppendIdent(ident)
                        .Append("//")
                        .AppendLine(literalInfo.Literal)
                        .AppendIdent(ident)
                        .AppendFormat("{0} static partial global::XP.SDK.Utf8String {1}() => ",
                            AccessibilityToString(literalInfo.Accessibility),
                            literalInfo.Name);

                    if (literalInfo.Literal != null)
                    {
                        builder.Append("new global::XP.SDK.Utf8String(new byte[] { ");

                        var bytes = utf8.GetBytes(literalInfo.Literal);
                        foreach (var b in bytes)
                        {
                            builder.AppendFormat("0x{0:X2}, ", b);
                        }

                        builder
                            .AppendFormat(CultureInfo.InvariantCulture, "0x00 }}, {0});", bytes.Length)
                            .AppendLine()
                            .AppendLine();
                    }
                    else
                    {
                        builder.AppendLine("default;").AppendLine();
                    }
                }

                ident--;

                builder.AppendIdent(ident).AppendLine("}");

                if (ident > 0)
                {
                    builder.AppendLine("}");
                }

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
            public LiteralInfo(Accessibility accessibility, string name, string literal)
            {
                Accessibility = accessibility;
                Name = name;
                Literal = literal;
            }

            public readonly Accessibility Accessibility;

            public readonly string Name;

            public readonly string Literal;
        }
    }
}
