using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Rename;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XP.SDK.Analyzers
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(Utf8StringLiteralCodeFixProvider)), Shared]
    public class Utf8StringLiteralCodeFixProvider : CodeFixProvider
    {
        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            // TODO: Replace the following code with your own analysis, generating a CodeAction for each fix to suggest
            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            if (diagnostic.Id == Utf8StringLiteralAnalyzer.Rule8001Id)
            {
                var methodDecl = (MethodDeclarationSyntax) root.FindToken(diagnosticSpan.Start).Parent;

                // Register a code action that will invoke the fix.
                context.RegisterCodeFix(
                    CodeAction.Create(
                        title:"Convert to UTF-8 literal method.",
                        createChangedDocument: c => ConvertToUtf8LiteralAsync(context.Document, methodDecl, diagnostic.Properties, c),
                        equivalenceKey: "ConvertToUtf8Literal"),
                    diagnostic);
            }
        }

        private async Task<Document> ConvertToUtf8LiteralAsync(Document document, MethodDeclarationSyntax methodDecl, ImmutableDictionary<string, string> properties, CancellationToken cancellationToken)
        {
            var literal = properties[Utf8StringLiteralAnalyzer.LiteralProperty];
            var attribute = properties[Utf8StringLiteralAnalyzer.Utf8AttributeProperty];

            var newMethodDecl = methodDecl
                .WithBody(null)
                .WithExpressionBody(null)
                .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                .AddModifiers(Token(SyntaxKind.PartialKeyword))
                .WithAdditionalAnnotations(Formatter.Annotation)
                .AddAttributeLists(AttributeList().AddAttributes(
                    Attribute(
                        ParseName(attribute),
                        AttributeArgumentList().AddArguments(
                            AttributeArgument(literal != null
                                ? LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(literal))
                                : LiteralExpression(SyntaxKind.NullLiteralExpression))))));
            
            var oldRoot = await document.GetSyntaxRootAsync(cancellationToken);
            var newRoot = oldRoot.ReplaceNode(methodDecl, newMethodDecl);

            return document.WithSyntaxRoot(newRoot);
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return WellKnownFixAllProviders.BatchFixer;
        }

        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(Utf8StringLiteralAnalyzer.Rule8001Id);
    }
}
