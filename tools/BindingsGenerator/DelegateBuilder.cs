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

            TypeMap.RegisterType(cppType, nativeName, managedName);

            return @delegate;
        }

        private ParameterSyntax BuildParameter(CppParameter cppParameter)
        {
            var name = Identifier(cppParameter.Name);

            if (TypeMap.TryGetType(cppParameter.Type.GetDisplayName(), out var typeInfo))
            {
                return Parameter(name).WithType(typeInfo.TypeSyntax);
            }

            if (cppParameter.Type is CppPointerType pointerType)
            {
                if (pointerType.ElementType is CppFunctionType)
                    throw new NotSupportedException("Callbacks in callbacks are not supported at the moment.");

                if (TypeMap.TryGetType(pointerType.ElementType.GetDisplayName(), out typeInfo))
                {
                    return Parameter(name).WithType(PointerType(typeInfo.TypeSyntax));
                }

                if (pointerType.ElementType is CppQualifiedType qualifiedType &&
                    TypeMap.TryGetType(qualifiedType.ElementType.GetDisplayName(), out typeInfo))
                {
                    return Parameter(name).WithType(PointerType(typeInfo.TypeSyntax));
                }
            }

            Debugger.Break();
            throw new NotSupportedException();
        }

        protected override string GetNativeName(CppTypedef type) => type.Name;

        protected override string GetManagedName(string nativeName)
        {
            var managedName = base.GetManagedName(nativeName);
            if (managedName.EndsWith("_f"))
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
