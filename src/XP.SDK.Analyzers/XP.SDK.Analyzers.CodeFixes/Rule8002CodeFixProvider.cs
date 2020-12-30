using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Humanizer;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XP.SDK.Analyzers
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(Rule8002CodeFixProvider)), Shared]
    public class Rule8002CodeFixProvider : CodeFixProvider
    {
        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            var node = root.FindNode(diagnosticSpan);
            var invocationExpr = (InvocationExpressionSyntax) node;

            var literal = diagnostic.Properties[Utf8StringLiteralAnalyzer.LiteralProperty];

            switch (literal)
            {
                case null:
                    context.RegisterCodeFix(
                        CodeAction.Create(
                            "Replace with default.",
                            c => ReplaceWithDefaultAsync(context.Document, invocationExpr, c),
                            "XPSDK8002_ReplaceWithDefault"),
                        diagnostic);
                    context.RegisterCodeFix(
                        CodeAction.Create(
                            "Replace with default(Utf8String).",
                            c => ReplaceWithDefaultUtf8StringAsync(context.Document, invocationExpr, diagnostic.Properties, c),
                            "XPSDK8002_ReplaceWithDefaultUtf8String"),
                        diagnostic);
                    break;
                case "":
                    context.RegisterCodeFix(
                        CodeAction.Create(
                            "Replace with Utf8String.Empty.",
                            c => ReplaceWithEmptyUtf8StringAsync(context.Document, invocationExpr, diagnostic.Properties, c),
                            "XPSDK8002_ReplaceWithEmptyUtf8String"),
                        diagnostic);
                    break;
                default:
                    context.RegisterCodeFix(
                        CodeAction.Create(
                            "Replace with UTF-8 literal method.",
                            c => ReplaceWithUtf8LiteralAsync(context.Document, invocationExpr, diagnostic.Properties, c),
                            "ReplaceWithUtf8Literal"),
                        diagnostic);
                    break;
            }
        }

        private async Task<Document> ReplaceWithDefaultAsync(Document document,
            InvocationExpressionSyntax invocationExpr,
            CancellationToken cancellationToken)
        {
            var newExpression = LiteralExpression(SyntaxKind.DefaultLiteralExpression, Token(SyntaxKind.DefaultKeyword));

            var oldRoot = await document.GetSyntaxRootAsync(cancellationToken);
            var newRoot = oldRoot.ReplaceNode(invocationExpr, newExpression);
            return document.WithSyntaxRoot(newRoot);
        }

        private async Task<Document> ReplaceWithDefaultUtf8StringAsync(Document document,
            InvocationExpressionSyntax invocationExpr,
            ImmutableDictionary<string, string> properties,
            CancellationToken cancellationToken)
        {
            var utf8String = properties[Utf8StringLiteralAnalyzer.Utf8StringTypeProperty];
            var newExpression = DefaultExpression(ParseName(utf8String));

            var oldRoot = await document.GetSyntaxRootAsync(cancellationToken);
            var newRoot = oldRoot.ReplaceNode(invocationExpr, newExpression);
            return document.WithSyntaxRoot(newRoot);
        }

        private async Task<Document> ReplaceWithEmptyUtf8StringAsync(Document document,
            InvocationExpressionSyntax invocationExpr,
            ImmutableDictionary<string, string> properties,
            CancellationToken cancellationToken)
        {
            var utf8String = properties[Utf8StringLiteralAnalyzer.Utf8StringTypeProperty];
            var newExpression = MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                ParseName(utf8String),
                IdentifierName("Empty"));

            var oldRoot = await document.GetSyntaxRootAsync(cancellationToken);
            var newRoot = oldRoot.ReplaceNode(invocationExpr, newExpression);
            return document.WithSyntaxRoot(newRoot);
        }


        private async Task<Document> ReplaceWithUtf8LiteralAsync(Document document, InvocationExpressionSyntax invocationExpr, ImmutableDictionary<string, string> properties, CancellationToken cancellationToken)
        {
            var utf8String = properties[Utf8StringLiteralAnalyzer.Utf8StringTypeProperty];
            var literal = properties[Utf8StringLiteralAnalyzer.LiteralProperty];
            var attribute = properties[Utf8StringLiteralAnalyzer.Utf8AttributeProperty];

            var methodName = new string(literal.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)).ToArray()).Underscore().Pascalize() + "Literal";
            var newInvocationExpr = InvocationExpression(IdentifierName(methodName));
            
            var newMethodDecl = MethodDeclaration(ParseName(utf8String), methodName)
                .WithBody(null)
                .WithExpressionBody(null)
                .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                .AddModifiers(Token(SyntaxKind.PrivateKeyword), Token(SyntaxKind.PartialKeyword))
                .WithAdditionalAnnotations(Formatter.Annotation)
                .AddAttributeLists(AttributeList().AddAttributes(
                    Attribute(
                        ParseName(attribute),
                        AttributeArgumentList().AddArguments(
                            AttributeArgument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(literal)))))));

            var oldTypeDeclaration = invocationExpr.FirstAncestorOrSelf<TypeDeclarationSyntax>();
            var newTypeDeclaration = oldTypeDeclaration
                .ReplaceNode(invocationExpr, newInvocationExpr)
                .AddMembers(newMethodDecl);
            
            var oldRoot = await document.GetSyntaxRootAsync(cancellationToken);
            var newRoot = oldRoot.ReplaceNode(oldTypeDeclaration, newTypeDeclaration);
            
            return document.WithSyntaxRoot(newRoot);
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return WellKnownFixAllProviders.BatchFixer;
        }

        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(Utf8StringLiteralAnalyzer.Rule8002Id);
    }
}
