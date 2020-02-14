using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CppAst;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace BindingsGenerator
{
    public class EnumBuilder : BuilderBase<CppEnum>
    {
        public EnumBuilder(AdhocWorkspace workspace, ProjectId projectId, string directory, TypeMap typeMap) : base(workspace, projectId, directory, typeMap)
        {
        }

        protected override MemberDeclarationSyntax BuildType(CppEnum cppType, string nativeName, string managedName)
        {
            var prefixLength = GetItemsPrefixLength(cppType);

            var @enum = EnumDeclaration(managedName)
                .AddModifiers(Token(SyntaxKind.PublicKeyword))
                .AddMembers(cppType.Items.Select(cppEnumItem => BuildEnumMember(cppEnumItem, prefixLength)).ToArray());

            ApplyFlagsHeuristic(cppType, ref @enum);
            TypeMap.RegisterEnumType(nativeName, cppType, managedName);
            return @enum;
        }

        private static EnumMemberDeclarationSyntax BuildEnumMember(CppEnumItem cppEnumItem, int prefixLength)
        {
            return EnumMemberDeclaration(PrettyItemName(cppEnumItem.Name))
                .WithEqualsValue(EqualsValueClause(LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal((int)cppEnumItem.Value))))
                .WithAdditionalAnnotations(new SyntaxAnnotation(Annotations.NativeName, cppEnumItem.Name));
            
            string PrettyItemName(string itemName)
            {
                var parts = itemName[prefixLength..].Split("_");
                return string.Concat(parts.Select(p => p[..1].ToUpperInvariant() + p[1..]));
            }
        }

        protected override string GetNativeName(CppEnum cppEnum)
        {
            if (!string.IsNullOrEmpty(cppEnum.Name))
                return cppEnum.Name;

            var comment = cppEnum.Comment.ToString();
            if (string.IsNullOrEmpty(comment))
                return $"Enum_{Guid.NewGuid():N}";

            var match = Regex.Match(comment, @"\w+");
            return match.Value;
        }

        private static void ApplyFlagsHeuristic(CppEnum cppEnum, ref EnumDeclarationSyntax @enum)
        {
            if (cppEnum.Items.All(i => IsPowerOf2(i.Value)))
            {
                @enum = @enum.AddAttributeLists(
                    AttributeList(SingletonSeparatedList(
                        Attribute(QualifiedName(IdentifierName("System"), IdentifierName("FlagsAttribute"))))));
            }

            static bool IsPowerOf2(long value) => ((value - 1) & value) == 0;
        }

        private int GetItemsPrefixLength(CppEnum cppEnum)
        {
            if (cppEnum.Items.Count < 2) return 0;

            var item1 = cppEnum.Items[0].Name;
            var item2 = cppEnum.Items[^1].Name;
            var len = Math.Min(item1.Length, item2.Length);
            int prefixLength;
            for (prefixLength = 0; prefixLength < len; prefixLength++)
            {
                if (item1[prefixLength] != item2[prefixLength]) break;
            }

            while (prefixLength > 0)
            {
                var prefix = item1[..prefixLength];
                if (cppEnum.Items.All(i => i.Name.StartsWith(prefix)))
                {
                    return prefixLength;
                }

                prefixLength -= 1;
            }

            return 0;
        }
    }
}
