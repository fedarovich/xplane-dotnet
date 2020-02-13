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
    public class EnumBuilder
    {
        private readonly AdhocWorkspace _workspace;
        private readonly ProjectId _projectId;
        private readonly string _directory;
        private readonly TypeMap _typeMap;

        public EnumBuilder(AdhocWorkspace workspace, ProjectId projectId, string directory, TypeMap typeMap)
        {
            _workspace = workspace;
            _projectId = projectId;
            _directory = directory;
            _typeMap = typeMap;
        }

        public void Build(CppContainerList<CppEnum> enums, string @namespace)
        {
            foreach (var cppEnum in enums)
            {
                var nativeName = GetNativeName(cppEnum);
                if (_typeMap.TryGetType(nativeName, out var typeInfo))
                {
                    if (typeInfo.IsSame(cppEnum))
                        continue;

                    throw new ArgumentException("Duplicate type " + nativeName);
                }

                var managedName = GetManagedName(nativeName);

                var prefixLength = GetItemsPrefixLength(cppEnum);

                var @enum = EnumDeclaration(managedName)
                    .AddModifiers(Token(SyntaxKind.PublicKeyword))
                    .AddMembers(cppEnum.Items.Select(cppEnumItem => BuildEnumMember(cppEnumItem, prefixLength)).ToArray());

                ApplyFlagsHeuristic(cppEnum, ref @enum);
                AddComments(cppEnum, ref @enum);

                BuildDocument(@namespace, @enum, managedName);

                _typeMap.RegisterEnumType(nativeName, cppEnum, managedName);
            }
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

        private static string GetNativeName(CppEnum cppEnum)
        {
            if (!string.IsNullOrEmpty(cppEnum.Name))
                return cppEnum.Name;

            var comment = cppEnum.Comment.ToString();
            if (string.IsNullOrEmpty(comment))
                return $"Enum_{Guid.NewGuid():N}";

            var match = Regex.Match(comment, @"\w+");
            return match.Value;
        }

        private static string GetManagedName(string nativeName)
        {
            if (nativeName.StartsWith("XPLM"))
                return nativeName[4..];
            
            if (nativeName.StartsWith("XP"))
                return nativeName[2..];

            return nativeName;
        }

        private static void ApplyFlagsHeuristic(CppEnum cppEnum, ref EnumDeclarationSyntax @enum)
        {
            if (cppEnum.Items.All(i => IsPowerOf2(i.Value)))
            {
                @enum = @enum.AddAttributeLists(
                    AttributeList(SingletonSeparatedList(
                        Attribute(IdentifierName("System.Flags")))));
            }

            static bool IsPowerOf2(long value) => ((value - 1) & value) == 0;
        }

        private void AddComments(CppEnum cppEnum, ref EnumDeclarationSyntax @enum)
        {
            // TODO:
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

        private void BuildDocument(string @namespace, EnumDeclarationSyntax @enum, string managedName)
        {
            var ns = NamespaceDeclaration(IdentifierName(@namespace))
                .WithMembers(SingletonList<MemberDeclarationSyntax>(@enum))
                .NormalizeWhitespace();

            var filename = $"{managedName}.cs";
            var path = Path.Combine(_directory, filename);

            var documentInfo = DocumentInfo.Create(
                DocumentId.CreateNewId(_projectId, filename),
                filename,
                loader: TextLoader.From(TextAndVersion.Create(
                    SourceText.From(ns.ToFullString()), VersionStamp.Create(), path)),
                filePath: path);
            var doc = _workspace.AddDocument(documentInfo);
        }
    }
}
