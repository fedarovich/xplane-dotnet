using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace BindingsGenerator
{
    internal static class SyntaxExtensions
    {
        internal static readonly NameSyntax System = IdentifierName("System");
        internal static readonly NameSyntax CompilerServices =
            QualifiedName(
                QualifiedName(
                    System,
                    IdentifierName(nameof(global::System.Runtime))),
                IdentifierName(nameof(global::System.Runtime.CompilerServices)));

        public static T AddAggressiveInlining<T>(this T member, bool fullyQualified = false) where T : MemberDeclarationSyntax
        {
            NameSyntax attributeType = IdentifierName(nameof(global::System.Runtime.CompilerServices.MethodImplAttribute));
            NameSyntax argumentType = IdentifierName(nameof(global::System.Runtime.CompilerServices.MethodImplOptions));
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
                                IdentifierName(nameof(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining))))))));
        }
    }
}
