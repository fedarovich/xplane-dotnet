using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace XP.SDK.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class Utf8StringLiteralAnalyzer : DiagnosticAnalyzer
    {
        public const string Rule8001Id = "XPSDK8001";
        public const string Rule8002Id = "XPSDK8002";

        public const string LiteralProperty = "Literal";
        public const string Utf8AttributeProperty = "Utf8Attribute";

        private static readonly DiagnosticDescriptor Rule8001 = new DiagnosticDescriptor(
            Rule8001Id,
            "The method can be converted to UTF-8 string literal method",
            "Convert method to UTF-8 string literal method",
            "Design",
            DiagnosticSeverity.Warning,
            true,
            "UTF-8 string literal partial methods allows you to create UTF-8 strings without additional encoding and memory allocations.");
        
        private static readonly DiagnosticDescriptor Rule8002 = new DiagnosticDescriptor(
            Rule8002Id,
            "UTF-8 string literal method should be used for better performance",
            "Use UTF-8 string literal method instead",
            "Usage",
            DiagnosticSeverity.Warning,
            true,
            "UTF-8 string literal partial methods allows you to create UTF-8 strings without additional encoding and memory allocations.");


        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule8001, Rule8002);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxNodeAction(AnalyzeMethodReturningUtf8String, SyntaxKind.MethodDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeUtf8StringCreation, SyntaxKind.InvocationExpression);
        }

        private void AnalyzeMethodReturningUtf8String(SyntaxNodeAnalysisContext context)
        {
            var methodDecl = (MethodDeclarationSyntax) context.Node;
            
            if (context.SemanticModel.GetSymbolInfo(methodDecl.ReturnType).Symbol is not { } returnTypeSymbol)
                return;

            if (context.Compilation.GetTypeByMetadataName("XP.SDK.Utf8String") is not { } utf8StringSymbol)
                return;

            if (!SymbolEqualityComparer.Default.Equals(returnTypeSymbol, utf8StringSymbol))
                return;

            if (context.Compilation.GetTypeByMetadataName("XP.SDK.Utf8StringLiteralAttribute") is not { } utf8StringLiteralAttributeSymbol)
                return;

            Optional<string> literal = default;
            
            if (methodDecl.Body != null &&
                methodDecl.Body.Statements.Count == 1 &&
                methodDecl.Body.Statements[0] is ReturnStatementSyntax {Expression: InvocationExpressionSyntax bodyInvocationExpr})
            {
                literal = GetUtf8StringLiteral(context, bodyInvocationExpr);
            }
            else if (methodDecl.ExpressionBody?.Expression is InvocationExpressionSyntax invocationExpr)
            {
                literal = GetUtf8StringLiteral(context, invocationExpr);
            }

            if (literal.HasValue)
            {
                var properties = ImmutableDictionary<string, string>.Empty
                    .Add(LiteralProperty, literal.Value)
                    .Add(Utf8AttributeProperty, utf8StringLiteralAttributeSymbol.ToMinimalDisplayString(context.SemanticModel, methodDecl.SpanStart));

                var diagnostic = Diagnostic.Create(Rule8001, methodDecl.GetLocation(), properties);
                context.ReportDiagnostic(diagnostic);
            }
        }

        private void AnalyzeUtf8StringCreation(SyntaxNodeAnalysisContext context)
        {
            var invocationExpr = (InvocationExpressionSyntax)context.Node;

            var literal = GetUtf8StringLiteral(context, invocationExpr);
            if (literal.HasValue)
            {
                var properties = ImmutableDictionary<string, string>.Empty
                    .Add(LiteralProperty, literal.Value);
                
                var diagnostic = Diagnostic.Create(Rule8002, invocationExpr.GetLocation(), properties);
                context.ReportDiagnostic(diagnostic);
            }
        }

        private static Optional<string> GetUtf8StringLiteral(SyntaxNodeAnalysisContext context, InvocationExpressionSyntax invocationExpr)
        {
            if (invocationExpr.Expression is not MemberAccessExpressionSyntax memberAccessExpr ||
                invocationExpr.ArgumentList.Arguments.Count != 1 ||
                memberAccessExpr.Name.ToString() != "FromString")
                return default;

            if (context.SemanticModel.GetSymbolInfo(memberAccessExpr).Symbol is not IMethodSymbol memberSymbol)
                return default;

            if (memberSymbol.Parameters.Length != 1)
                return default;

            if (context.Compilation.GetTypeByMetadataName("XP.SDK.Utf8String") is not { } utf8StringSymbol)
                return default;

            if (!SymbolEqualityComparer.Default.Equals(memberSymbol.ContainingType, utf8StringSymbol))
                return default;

            var argument = invocationExpr.ArgumentList.Arguments[0];
            var constantValue = context.SemanticModel.GetConstantValue(argument.Expression);
            if (!constantValue.HasValue)
                return default;

            return constantValue.Value as string;
        }
    }
}
