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
    public class DelegateBuilder : TypeBuilderBase<CppTypedef>
    {
        public DelegateBuilder(AdhocWorkspace workspace, ProjectId projectId, string directory, TypeMap typeMap) 
            : base(workspace, projectId, directory, typeMap)
        {
        }

        protected override MemberDeclarationSyntax BuildType(CppTypedef cppType, string nativeName, string managedName)
        {
            if (!(cppType.ElementType is CppPointerType cppPointerType &&
                  cppPointerType.ElementType is CppFunctionType function))
                return null;

            var returnType = TypeMap.GetType(function.ReturnType.GetDisplayName()).TypeSyntax;
            var @delegate = DelegateDeclaration(returnType, managedName)
                .AddModifiers(Token(SyntaxKind.PublicKeyword))
                .AddParameterListParameters(function.Parameters.Select(BuildParameter).ToArray());

            if (@delegate.DescendantNodes().OfType<PointerTypeSyntax>().Any())
            {
                @delegate = @delegate.AddModifiers(Token(SyntaxKind.UnsafeKeyword));
            }

            return @delegate;
        }

        private ParameterSyntax BuildParameter(CppParameter cppParameter)
        {
            var name = Identifier(cppParameter.Name);

            if (TypeMap.TryResolveType(cppParameter.Type, out var typeInfo))
            {
                return Parameter(name).WithType(typeInfo.TypeSyntax);
            }

            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
            throw new NotSupportedException();
        }

        protected override string GetRelativeNamespace(CppTypedef cppElement) => $"{base.GetRelativeNamespace(cppElement)}.Internal";

        protected override string GetNativeName(CppTypedef type) => type.Name;

        protected override string GetManagedName(string nativeName)
        {
            var managedName = base.GetManagedName(nativeName);
            if (managedName.EndsWith("_f") || managedName.EndsWith("_t"))
            {
                managedName = managedName[..^2];
            }

            if (!managedName.EndsWith("Callback"))
            {
                managedName += "Callback";
            }

            return managedName;
        }

        protected override bool CanProcess(CppTypedef cppElement)
        {
            return cppElement.ElementType is CppPointerType cppPointerType && cppPointerType.ElementType is CppFunctionType;
        }
    }
}
