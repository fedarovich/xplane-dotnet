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
    public class Utf8StringOverloadAnalyzer : DiagnosticAnalyzer
    {
        public const string Rule8003Id = "XPSDK8003";
        public const string Rule8004Id = "XPSDK8004";

        private static readonly DiagnosticDescriptor Rule8003 = new(
            Rule8003Id,
            "UTF-8 strings should be used for better performance",
            "Use overload taking Utf8String instead",
            "Usage",
            DiagnosticSeverity.Warning,
            true);

        private static readonly DiagnosticDescriptor Rule8004 = new(
            Rule8004Id,
            "UTF-8 strings should be used for better performance",
            "Consider using overload taking Utf8String instead",
            "Usage",
            DiagnosticSeverity.Info,
            true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule8003, Rule8004);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxNodeAction(AnalyzeMethodInvocation, SyntaxKind.InvocationExpression);
        }

        private void AnalyzeMethodInvocation(SyntaxNodeAnalysisContext context)
        {
            var invocationExpr = (InvocationExpressionSyntax)context.Node;
            if (invocationExpr.Expression is not MemberAccessExpressionSyntax memberAccessExpr ||
                context.SemanticModel.GetSymbolInfo(memberAccessExpr).Symbol is not IMethodSymbol methodSymbol ||
                methodSymbol.Parameters.Length == 0)
                return;
            
            if (context.Compilation.GetTypeByMetadataName("XP.SDK.Utf8String") is not { } utf8StringSymbol)
                return;

            if (context.Compilation.GetTypeByMetadataName("System.ReadOnlySpan`1") is not { } readOnlySpanSymbol)
                return;

            INamedTypeSymbol stringType = context.Compilation.GetSpecialType(SpecialType.System_String);
            INamedTypeSymbol charType = context.Compilation.GetSpecialType(SpecialType.System_Char);

            var readOnlySpanOfCharSymbol = readOnlySpanSymbol.Construct(charType);

            var indices = methodSymbol.Parameters
                .Select((param, index) => (param, index))
                .Where(t =>
                    SymbolEqualityComparer.Default.Equals(t.param.Type, stringType) ||
                    SymbolEqualityComparer.Default.Equals(t.param.Type, readOnlySpanOfCharSymbol))
                .Select(t => t.index)
                .ToImmutableHashSet();
            
            if (indices.Count == 0)
                return;

            var overloads = methodSymbol.ContainingType
                .GetMembers(methodSymbol.Name)
                .OfType<IMethodSymbol>()
                .Where(m => !SymbolEqualityComparer.Default.Equals(m, methodSymbol));

            // ReSharper disable once ConvertClosureToMethodGroup
            var matchingOverload = overloads.FirstOrDefault(o => IsMatchingUtf8Overload(o));
            if (matchingOverload == null)
                return;

            var constants = GetArgumentConstantValues();

            if (constants != null)
            {
                var properties = constants
                    .ToImmutableDictionary(
                        kv => "arg_" + kv.Key,
                        kv => kv.Value);
                
                var diagnostic = Diagnostic.Create(Rule8003, invocationExpr.GetLocation(), properties);
                context.ReportDiagnostic(diagnostic);
            }
            else
            {
                var diagnostic = Diagnostic.Create(Rule8004, invocationExpr.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }

            bool IsMatchingUtf8Overload(IMethodSymbol overload)
            {
                if (overload.Parameters.Length != methodSymbol.Parameters.Length)
                    return false;

                for (int i = 0; i < overload.Parameters.Length; i++)
                {
                    if (indices.Contains(i))
                    {
                        if (!SymbolEqualityComparer.Default.Equals(overload.Parameters[i].Type, utf8StringSymbol))
                            return false;
                    }
                    else if (!SymbolEqualityComparer.Default.Equals(overload.Parameters[i].Type, methodSymbol.Parameters[i].Type))
                    {
                        return false;
                    }
                }

                return true;
            }

            ImmutableDictionary<string, string> GetArgumentConstantValues()
            {
                var builder = ImmutableDictionary.CreateBuilder<string, string>();
                for (int i = 0; i < invocationExpr.ArgumentList.Arguments.Count; i++)
                {
                    var arg = invocationExpr.ArgumentList.Arguments[i];
                    if (arg.NameColon == null)
                    {
                        if (!indices.Contains(i))
                            continue;

                        var literal = context.SemanticModel.GetConstantValue(arg.Expression);
                        if (!literal.HasValue)
                            return null;
                        
                        builder.Add(methodSymbol.Parameters[i].Name, literal.Value as string);
                    }
                    else
                    {
                        var (parameter, index) = methodSymbol.Parameters
                            .Select((param, ind) => (param, ind))
                            .FirstOrDefault(t => t.param.Name == arg.NameColon.Name.ToString());

                        if (parameter == null)
                            return null;

                        var literal = context.SemanticModel.GetConstantValue(arg.Expression);
                        if (!literal.HasValue)
                            return null;
                        
                        builder.Add(parameter.Name, literal.Value as string);
                    }
                }

                return builder.ToImmutable();
            }
        }

    }
}
