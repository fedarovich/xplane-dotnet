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
    public class StructBuilder : BuilderBase<CppClass>
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

            TypeMap.RegisterType(cppType, nativeName, managedName);

            return @struct;
        }

        private MemberDeclarationSyntax BuildField(CppField cppField)
        {
            var name = Identifier(cppField.Name);

            if (TypeMap.TryGetType(cppField.Type.GetDisplayName(), out var typeInfo))
            {
                return FieldDeclaration(
                    VariableDeclaration(typeInfo.TypeSyntax)
                        .AddVariables(VariableDeclarator(cppField.Name)))
                    .AddModifiers(Token(SyntaxKind.PublicKeyword));
            }

            Debugger.Break();
            throw new NotImplementedException();
        }

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
