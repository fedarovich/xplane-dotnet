using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using CppAst;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace BindingsGenerator
{
    public class StructBuilder : TypeBuilderBase<CppClass>
    {
        public StructBuilder(AdhocWorkspace workspace, ProjectId projectId, string directory, TypeMap typeMap) 
            : base(workspace, projectId, directory, typeMap)
        {
        }

        protected override MemberDeclarationSyntax BuildType(CppClass cppType, string nativeName, string managedName)
        {
            var @struct = StructDeclaration(managedName)
                .AddModifiers(Token(SyntaxKind.PublicKeyword))
                .AddMembers(cppType.Fields.Select(BuildField).ToArray());

            if (@struct.DescendantNodes().OfType<PointerTypeSyntax>().Any())
            {
                @struct = @struct.AddModifiers(Token(SyntaxKind.UnsafeKeyword));
            }

            @struct = @struct.AddModifiers(Token(SyntaxKind.PartialKeyword));
            @struct = @struct.AddDocumentationComments(cppType.Comment, nativeName);

            return @struct;
        }

        private MemberDeclarationSyntax BuildField(CppField cppField)
        {
            if (TypeMap.TryResolveType(cppField.Type, out var typeInfo))
            {
                if (typeInfo.IsFunction)
                {
                    return FieldDeclaration(
                            VariableDeclaration(IdentifierName(nameof(IntPtr)))
                                .AddVariables(VariableDeclarator(cppField.Name)))
                        .AddModifiers(Token(SyntaxKind.PublicKeyword))
                        .AddManagedTypeAttribute(typeInfo.TypeSyntax);
                }

                return FieldDeclaration(
                    VariableDeclaration(typeInfo.TypeSyntax)
                        .AddVariables(VariableDeclarator(cppField.Name)))
                    .AddModifiers(Token(SyntaxKind.PublicKeyword));
            }

            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
            throw new NotImplementedException();
        }

        protected override string GetRelativeNamespace(CppClass cppElement) => $"{base.GetRelativeNamespace(cppElement)}.Internal";

        protected override string GetNativeName(CppClass type) => type.Name;

        protected override string GetManagedName(string nativeName)
        {
            var managedName = base.GetManagedName(nativeName);
            if (managedName.EndsWith("_t"))
            {
                managedName = managedName[..^2];
            }

            return managedName;
        }
    }
}
