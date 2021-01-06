using System;
using System.Diagnostics;
using System.Linq;
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
                .AddParameterListParameters(function.Parameters.Select(BuildParameter).ToArray())
                .AddUnmanagedFunctionPointerAttribute();

            if (@delegate.DescendantNodes().OfType<PointerTypeSyntax>().Any())
            {
                @delegate = @delegate.AddModifiers(Token(SyntaxKind.UnsafeKeyword));
            }

            @delegate = @delegate.AddDocumentationComments(cppType.Comment, nativeName);

            return @delegate;
        }

        private ParameterSyntax BuildParameter(CppParameter cppParameter)
        {
            var name = Identifier(cppParameter.Name);

            if (TypeMap.TryResolveType(cppParameter.Type, out var typeInfo))
            {
                return typeInfo.IsFunction
                    ? Parameter(name).WithType(typeInfo.FunctionPointerTypeSyntax)
                    : Parameter(name).WithType(typeInfo.TypeSyntax);
            }

            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
            throw new NotSupportedException();
        }

        protected override string GetRelativeNamespace(CppTypedef cppElement) => $"{base.GetRelativeNamespace(cppElement)}.Interop";

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
            return cppElement.ElementType is CppPointerType {ElementType: CppFunctionType};
        }

        protected override FunctionPointerTypeSyntax GetFunctionPointerTypeSyntax(CppType cppType)
        {
            if (cppType is not CppTypedef {ElementType: CppPointerType {ElementType: CppFunctionType function}})
                return null;
                
            var returnType = TypeMap.GetType(function.ReturnType.GetDisplayName()).TypeSyntax;

            return FunctionPointerType()
                .WithUnmanagedCallingConvention()
                .AddParameterListParameters(BuildFunctionPointerParameterList());

            FunctionPointerParameterSyntax[] BuildFunctionPointerParameterList()
            {
                var parameters = new FunctionPointerParameterSyntax[function.Parameters.Count + 1];

                for (int i = 0; i < function.Parameters.Count; i++)
                {
                    if (TypeMap.TryResolveType(function.Parameters[i].Type, out var paramTypeInfo))
                    { 
                        parameters[i] = FunctionPointerParameter(paramTypeInfo.IsFunction
                            ? paramTypeInfo.FunctionPointerTypeSyntax
                            : paramTypeInfo.TypeSyntax);

                    }
                }
                
                parameters[^1] = FunctionPointerParameter(returnType);
                return parameters;
            }
        }
    }
}
