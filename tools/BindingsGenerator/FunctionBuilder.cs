using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using CppAst;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace BindingsGenerator
{
    public class FunctionBuilder : BuilderBase<CppFunction>
    {
        public FunctionBuilder(AdhocWorkspace workspace, ProjectId projectId, string directory, TypeMap typeMap) : base(workspace, projectId, directory, typeMap)
        {
        }

        public override void Build(IEnumerable<CppFunction> cppFunctions)
        {
            var firstFunction = cppFunctions.FirstOrDefault();
            if (firstFunction == null)
                return;
            
            var className = GetClassName(firstFunction);
            var @class = ClassDeclaration(className)
                .AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword), Token(SyntaxKind.PartialKeyword))
                .AddMembers(cppFunctions.Select(BuildFunctionPointer).ToArray())
                .AddMembers(BuildStaticConstructor(className, cppFunctions))
                .AddMembers(cppFunctions.Select(BuildUnsafeFunctions).ToArray());

            BuildDocument(GetRelativeNamespace(firstFunction), @class, className);
        }

        private MemberDeclarationSyntax BuildFunctionPointer(CppFunction cppFunction)
        {
            return FieldDeclaration(
                    VariableDeclaration(IdentifierName(nameof(IntPtr)))
                        .AddVariables(VariableDeclarator(GetManagedName(cppFunction.Name + "Ptr"))))
                .AddModifiers(Token(SyntaxKind.PrivateKeyword), Token(SyntaxKind.StaticKeyword));
        }

        private MemberDeclarationSyntax BuildStaticConstructor(string className, IEnumerable<CppFunction> cppFunctions)
        {
            return ConstructorDeclaration(className)
                .AddModifiers(Token(SyntaxKind.StaticKeyword))
                .WithBody(Block(BuildStaticConstructorExpressions()));

            IEnumerable<StatementSyntax> BuildStaticConstructorExpressions()
            {
                yield return LocalDeclarationStatement(VariableDeclaration(PredefinedType(Token(SyntaxKind.StringKeyword)))
                        .AddVariables(VariableDeclarator("libraryName")
                            .WithInitializer(EqualsValueClause(
                                LiteralExpression(SyntaxKind.StringLiteralExpression, 
                                    Literal(GetNativeLibrary(cppFunctions.First())))))))
                    .AddModifiers(Token(SyntaxKind.ConstKeyword));

                foreach (var cppFunction in cppFunctions)
                {
                    yield return ExpressionStatement(
                        AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            IdentifierName(GetManagedName(cppFunction.Name + "Ptr")),
                            InvocationExpression(
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    IdentifierName("FunctionResolver"),
                                    IdentifierName("Resolve")))
                                .AddArgumentListArguments(
                                    Argument(IdentifierName("libraryName")),
                                    Argument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(cppFunction.Name))))));
                }
            }
        }

        private MemberDeclarationSyntax BuildUnsafeFunctions(CppFunction cppFunction)
        {
            if (!TypeMap.TryResolveType(cppFunction.ReturnType, out var returnTypeInfo))
                throw new ArgumentException();

            var method = MethodDeclaration(returnTypeInfo.TypeSyntax, GetManagedName(cppFunction.Name))
                .AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword))
                .AddParameterListParameters(cppFunction.Parameters.Select(BuildParameter).ToArray())
                .AddAggressiveInlining()
                .WithBody(Block(BuildMethodBody(cppFunction, returnTypeInfo)));

            if (method.DescendantNodes().OfType<PointerTypeSyntax>().Any())
            {
                method = method.AddModifiers(Token(SyntaxKind.UnsafeKeyword));
            }

            return method;
        }

        private ParameterSyntax BuildParameter(CppParameter cppParameter)
        {
            var name = Identifier(GetManagedName(cppParameter.Name));

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

        private IEnumerable<StatementSyntax> BuildMethodBody(CppFunction cppFunction, TypeInfo returnTypeInfo)
        {
            yield return ExpressionStatement(
                InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName("IL"),
                        IdentifierName("DeclareLocals")),
                    ArgumentList(SingletonSeparatedList(
                        Argument(LiteralExpression(SyntaxKind.FalseLiteralExpression))))));

            bool hasResult = !returnTypeInfo.IsVoid;

            if (hasResult)
            {
                yield return LocalDeclarationStatement(
                    VariableDeclaration(returnTypeInfo.TypeSyntax).AddVariables(VariableDeclarator("result")));
            }

            var delegates = new Dictionary<string, string>();
            foreach (var cppParameter in cppFunction.Parameters)
            {
                if (!TypeMap.TryResolveType(cppParameter.Type, out var paramTypeInfo, false))
                    throw new ArgumentException();

                if (!paramTypeInfo.IsFunction)
                    continue;

                var managedParameterName = GetManagedName(cppParameter.Name);
                delegates.Add(cppParameter.Name, managedParameterName + "Ptr");
                yield return LocalDeclarationStatement(
                    VariableDeclaration(IdentifierName("IntPtr"))
                        .WithAdditionalAnnotations(new SyntaxAnnotation(Annotations.Namespace, SyntaxExtensions.InteropServices.ToFullString()))
                        .AddVariables(VariableDeclarator(delegates[cppParameter.Name])
                            .WithInitializer(
                                EqualsValueClause(
                                    InvocationExpression(
                                        MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            IdentifierName(nameof(Marshal)),
                                            IdentifierName(nameof(Marshal.GetFunctionPointerForDelegate))),
                                        ArgumentList(SingletonSeparatedList(Argument(IdentifierName(managedParameterName)))))))));
            }

            foreach (var cppParameter in cppFunction.Parameters)
            {
                var parameterName = delegates.TryGetValue(cppParameter.Name, out var ptrName)
                    ? ptrName
                    : GetManagedName(cppParameter.Name);

                yield return ExpressionStatement(
                    InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName("IL"),
                            IdentifierName("Push")),
                        ArgumentList(SingletonSeparatedList(
                            Argument(IdentifierName(parameterName))))));
            }

            yield return ExpressionStatement(
                InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName("IL"),
                        IdentifierName("Push")),
                    ArgumentList(SingletonSeparatedList(
                        Argument(IdentifierName(GetManagedName(cppFunction.Name + "Ptr")))))));

            yield return ExpressionStatement(
                InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName("IL"),
                            IdentifierName("Emit")),
                        IdentifierName("Calli")),
                    ArgumentList(SingletonSeparatedList(Argument(
                        ObjectCreationExpression(
                            IdentifierName("StandAloneMethodSig"),
                            ArgumentList(SeparatedList(GetMethodSigParameters(returnTypeInfo, cppFunction, delegates))),
                            null))))));

            if (hasResult)
            {
                yield return ExpressionStatement(
                    InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName("IL"),
                            IdentifierName("Pop")),
                        ArgumentList(SingletonSeparatedList(
                            Argument(IdentifierName("result"))
                                .WithRefKindKeyword(Token(SyntaxKind.OutKeyword))))));
            }

            foreach (var cppParameter in cppFunction.Parameters.Reverse().Where(p => delegates.ContainsKey(p.Name)))
            {
                var managedParameterName = GetManagedName(cppParameter.Name);
                yield return ExpressionStatement(
                    InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName("GC"),
                            IdentifierName("KeepAlive")),
                        ArgumentList(SingletonSeparatedList(Argument(IdentifierName(managedParameterName))))));
            }

            if (hasResult)
            {
                yield return ReturnStatement(IdentifierName("result"));
            }
        }

        private IEnumerable<ArgumentSyntax> GetMethodSigParameters(TypeInfo returnTypeInfo, CppFunction cppFunction, Dictionary<string, string> delegates)
        {
            yield return Argument(
                MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    IdentifierName(nameof(CallingConvention)),
                    IdentifierName(nameof(CallingConvention.Cdecl))));

            yield return Argument(TypeOfExpression(returnTypeInfo.TypeSyntax));
            foreach (var cppParameter in cppFunction.Parameters)
            {
                if (delegates.TryGetValue(cppParameter.Name, out _))
                {
                    yield return Argument(TypeOfExpression(IdentifierName(nameof(IntPtr))));
                }
                else if (TypeMap.TryResolveType(cppParameter.Type, out var paramTypeInfo))
                {
                    yield return Argument(TypeOfExpression(paramTypeInfo.TypeSyntax));
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }

        protected override string GetRelativeNamespace(CppFunction cppElement) => $"{base.GetRelativeNamespace(cppElement)}.Internal";

        protected override string GetNativeName(CppFunction cppFunction) => cppFunction.Name;

        protected override IEnumerable<string> GetImportsCore(MemberDeclarationSyntax type)
        {
            foreach (var import in base.GetImportsCore(type))
            {
                yield return import;
            }

            yield return "InlineIL";
            yield return SyntaxExtensions.InteropServices.ToFullString();
        }

        private string GetClassName(CppFunction cppFunction)
        {
            var file = Path.GetFileNameWithoutExtension(cppFunction.Span.Start.File);
            var name = GetManagedName(file);
            return name;
        }
    }
}
