using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace BindingsGenerator
{
    internal static class SyntaxExtensions
    {
        internal static readonly NameSyntax SystemNamespace = IdentifierName(nameof(System));
        internal static readonly NameSyntax CompilerServices =
            QualifiedName(
                QualifiedName(
                    SystemNamespace,
                    IdentifierName(nameof(System.Runtime))),
                IdentifierName(nameof(System.Runtime.CompilerServices)));
        internal static readonly NameSyntax InteropServices =
            QualifiedName(
                QualifiedName(
                    SystemNamespace,
                    IdentifierName(nameof(System.Runtime))),
                IdentifierName(nameof(System.Runtime.InteropServices)));

        public static T AddAggressiveInlining<T>(this T member, bool fullyQualified = false) where T : MemberDeclarationSyntax
        {
            NameSyntax attributeType = IdentifierName(nameof(MethodImplAttribute));
            NameSyntax argumentType = IdentifierName(nameof(MethodImplOptions));
            if (fullyQualified)
            {
                attributeType = QualifiedName(CompilerServices, (IdentifierNameSyntax) argumentType);
                argumentType = QualifiedName(CompilerServices, (IdentifierNameSyntax) argumentType);
            }

            return (T) member.AddAttributeLists(
                    AttributeList(SingletonSeparatedList(
                        Attribute(attributeType)
                            .AddArgumentListArguments(AttributeArgument(MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                argumentType,
                                IdentifierName(nameof(MethodImplOptions.AggressiveInlining))))))));
        }
    }
}
