using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
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

        public override async Task BuildAsync(IEnumerable<CppFunction> cppFunctions)
        {
            var firstFunction = cppFunctions.FirstOrDefault();
            if (firstFunction == null)
                return;
            
            var className = GetClassName(firstFunction);
            Log.WriteLine($"Building type {className}...");
            using (Log.PushIdent())
            {
                var @class = ClassDeclaration(className)
                    .AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword), Token(SyntaxKind.PartialKeyword))
                    //.AddMembers(cppFunctions.Select(BuildFunctionPointer).ToArray())
                    //.AddMembers(BuildStaticConstructor(className, cppFunctions))
                    .AddMembers(cppFunctions.SelectMany(BuildFunctions).ToArray());

                await BuildDocumentAsync(GetRelativeNamespace(firstFunction), @class, className);
            }
            Log.WriteLine("Done.", ConsoleColor.Green);
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
            Log.WriteLine("Building static constructor...", ConsoleColor.DarkGray);
            Log.WriteLine("Done.", ConsoleColor.DarkGreen);
            return ConstructorDeclaration(className)
                .AddModifiers(Token(SyntaxKind.StaticKeyword))
                .WithBody(Block(BuildStaticConstructorExpressions()));

            IEnumerable<StatementSyntax> BuildStaticConstructorExpressions()
            {
                foreach (var cppFunction in cppFunctions)
                {
                    yield return ExpressionStatement(
                        AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            IdentifierName(GetManagedName(cppFunction.Name + "Ptr")),
                            InvocationExpression(
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    IdentifierName("Lib"),
                                    IdentifierName("GetExport")))
                                .AddArgumentListArguments(
                                    Argument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(cppFunction.Name))))));
                }
            }
        }

        private IEnumerable<MemberDeclarationSyntax> BuildFunctions(CppFunction cppFunction)
        {
            Log.WriteLine($"Building function '{GetManagedName(cppFunction.Name)}' from '{cppFunction.Name}'.",
                ConsoleColor.DarkGray);

            if (!TypeMap.TryResolveType(cppFunction.ReturnType, out var returnTypeInfo))
                throw new ArgumentException();

            MethodDeclarationSyntax method;
            if (HasFunctionParameters(cppFunction))
            {
                method = MethodDeclaration(returnTypeInfo.TypeSyntax, GetManagedName(cppFunction.Name) + "Private")
                    .AddModifiers(Token(SyntaxKind.PrivateKeyword), Token(SyntaxKind.StaticKeyword), Token(SyntaxKind.ExternKeyword))
                    .AddParameterListParameters(cppFunction.Parameters.Select(p => BuildParameter(p, false, true)).ToArray())
                    //.AddAggressiveInlining()
                    //.WithBody(Block(BuildBaseMethodBody(cppFunction, returnTypeInfo)))
                    .AddDllImport(cppFunction.Name)
                    .AddUnsafeIfNeeded()
                    .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
                
                yield return method;

                method = MethodDeclaration(returnTypeInfo.TypeSyntax, GetManagedName(cppFunction.Name))
                    .AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword))
                    .AddParameterListParameters(cppFunction.Parameters.Select(p => BuildParameter(p, false)).ToArray())
                    .AddAggressiveInlining()
                    .WithBody(Block(BuildDelegateBody(cppFunction, returnTypeInfo)))
                    .AddUnsafeIfNeeded();
                    
            }
            else
            {
                method = MethodDeclaration(returnTypeInfo.TypeSyntax, GetManagedName(cppFunction.Name))
                    .AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword), Token(SyntaxKind.ExternKeyword))
                    .AddParameterListParameters(cppFunction.Parameters.Select(p => BuildParameter(p, false)).ToArray())
                    //.AddAggressiveInlining()
                    //.WithBody(Block(BuildBaseMethodBody(cppFunction, returnTypeInfo)))
                    .AddDllImport(cppFunction.Name)
                    .AddUnsafeIfNeeded()
                    .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
            }

            method = method.AddDocumentationComments(cppFunction.Comment, cppFunction.Name);
            yield return method;

            if (cppFunction.Parameters.Any(p => p.Type.IsConstCharPtr()))
            {
                method = MethodDeclaration(returnTypeInfo.TypeSyntax, GetManagedName(cppFunction.Name))
                    .AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword), Token(SyntaxKind.UnsafeKeyword))
                    .AddParameterListParameters(cppFunction.Parameters.Select(p => BuildParameter(p, true)).ToArray())
                    .AddAggressiveInlining()
                    .WithBody(Block(BuildStringBody(cppFunction, returnTypeInfo)));
                method = method.AddDocumentationComments(cppFunction.Comment, cppFunction.Name);
                yield return method;
            }

            Log.WriteLine("Done.", ConsoleColor.DarkGreen);
        }

        private bool HasFunctionParameters(CppFunction cppFunction)
        {
            foreach (var cppParameter in cppFunction.Parameters)
            {
                if (TypeMap.TryResolveType(cppParameter.Type, out var typeInfo) && typeInfo.IsFunction)
                {
                    return true;
                }
            }

            return false;
        }

        private ParameterSyntax BuildParameter(CppParameter cppParameter, bool convertConstCharPtr, bool useFunctionPointers = false)
        {
            var name = Identifier(GetManagedName(cppParameter.Name));

            if (TypeMap.TryResolveType(cppParameter.Type, out var typeInfo))
            {
                if (convertConstCharPtr && typeInfo.CppType.IsConstCharPtr())
                {
                    return Parameter(name)
                        .WithType(SyntaxBuilder.BuildReadOnlySpanName(SyntaxKind.CharKeyword))
                        .AddModifiers(Token(SyntaxKind.InKeyword));
                }

                if (useFunctionPointers && typeInfo.IsFunction)
                {
                    return Parameter(name).WithType(SyntaxBuilder.IntPtrName);
                }

                return Parameter(name).WithType(typeInfo.TypeSyntax);
            }

            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
            throw new NotSupportedException();
        }

        private IEnumerable<StatementSyntax> BuildBaseMethodBody(CppFunction cppFunction, TypeInfo returnTypeInfo)
        {
            yield return SyntaxBuilder.DeclareLocals(false);

            var functionPointer = IdentifierName(GetManagedName(cppFunction.Name + "Ptr"));
            yield return SyntaxBuilder.CallGuard(functionPointer);
            
            IdentifierNameSyntax result = null;
            if (!returnTypeInfo.IsVoid)
            {
                if (returnTypeInfo.IsFunction)
                {
                    yield return SyntaxBuilder.DeclareResultVariable(SyntaxBuilder.IntPtrName, out result);
                }
                else
                {
                    yield return SyntaxBuilder.DeclareResultVariable(returnTypeInfo.TypeSyntax, out result);
                }
            }

            var delegates = new Dictionary<string, string>();
            foreach (var cppParameter in cppFunction.Parameters)
            {
                if (TypeMap.TryResolveType(cppParameter.Type, out var typeInfo) && typeInfo.IsFunction)
                {
                    delegates.Add(cppParameter.Name, null);
                }
                yield return SyntaxBuilder.EmitPush(IdentifierName(GetManagedName(cppParameter.Name)));
            }

            yield return SyntaxBuilder.EmitPush(functionPointer);

            yield return SyntaxBuilder.EmitCalli(returnTypeInfo, cppFunction, TypeMap, delegates);

            if (result != null)
            {
                yield return SyntaxBuilder.EmitPop(result);
                if (returnTypeInfo.IsFunction)
                {
                    yield return ReturnStatement(
                        InvocationExpression(
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    IdentifierName(nameof(Marshal)),
                                    GenericName(nameof(Marshal.GetDelegateForFunctionPointer))
                                        .AddTypeArgumentListArguments(returnTypeInfo.TypeSyntax)))
                            .AddArgumentListArguments(Argument(result)));
                }
                else
                {
                    yield return ReturnStatement(result);
                }
            }
        }

        private IEnumerable<StatementSyntax> BuildDelegateBody(CppFunction cppFunction, TypeInfo returnTypeInfo)
        {
            yield return SyntaxBuilder.DeclareLocals(false);

            var delegates = new Dictionary<string, string>();
            foreach (var cppParameter in cppFunction.Parameters)
            {
                if (!TypeMap.TryResolveType(cppParameter.Type, out var paramTypeInfo, false))
                    throw new ArgumentException();

                if (!paramTypeInfo.IsFunction)
                    continue;

                var managedParameterName = GetManagedName(cppParameter.Name);
                var variableName = managedParameterName + "Ptr";
                delegates.Add(cppParameter.Name, variableName);
                yield return SyntaxBuilder.DeclareDelegatePointerVariable(managedParameterName, variableName);
            }

            var call =
                InvocationExpression(IdentifierName(GetManagedName(cppFunction.Name) + "Private"))
                    .AddArgumentListArguments(cppFunction.Parameters.Select(BuildArgument).ToArray());

            if (returnTypeInfo.IsVoid)
            {
                yield return ExpressionStatement(call);
            }
            else
            {
                yield return LocalDeclarationStatement(
                    VariableDeclaration(returnTypeInfo.TypeSyntax)
                        .AddVariables(VariableDeclarator("result").WithInitializer(
                            EqualsValueClause(call))));
            }

            foreach (var cppParameter in cppFunction.Parameters.Reverse())
            {
                if (delegates.ContainsKey(cppParameter.Name))
                {
                    var managedParameterName = GetManagedName(cppParameter.Name);
                    yield return SyntaxBuilder.CallKeepAlive(IdentifierName(managedParameterName));
                }
            }

            if (!returnTypeInfo.IsVoid)
            {
                yield return ReturnStatement(IdentifierName("result"));
            }

            ArgumentSyntax BuildArgument(CppParameter cppParameter)
            {
                var name = delegates.TryGetValue(cppParameter.Name, out string paramName) 
                    ? paramName 
                    : GetManagedName(cppParameter.Name);
                return Argument(IdentifierName(name));
            }
        }

        private IEnumerable<StatementSyntax> BuildStringBody(CppFunction cppFunction, TypeInfo returnTypeInfo)
        {
            yield return SyntaxBuilder.DeclareLocals(false);

            foreach (var cppParameter in cppFunction.Parameters.Where(p => p.Type.IsConstCharPtr()))
            {
                var utf16Name = GetManagedName(cppParameter.Name);
                var utf8Name = utf16Name + "Utf8";
                var ptrName = utf16Name + "Ptr";
                yield return SyntaxBuilder.DeclareSpanForUtf8Variable(utf8Name, utf16Name);
                yield return SyntaxBuilder.DeclarePtrForUtf8Variable(ptrName, utf8Name, utf16Name);
            }

            var call = 
                InvocationExpression(IdentifierName(GetManagedName(cppFunction.Name)))
                    .AddArgumentListArguments(cppFunction.Parameters.Select(BuildArgument).ToArray());

            yield return returnTypeInfo.IsVoid
                ? (StatementSyntax) ExpressionStatement(call)
                : (StatementSyntax) ReturnStatement(call);

            ArgumentSyntax BuildArgument(CppParameter cppParameter)
            {
                var name = GetManagedName(cppParameter.Name);
                if (cppParameter.Type.IsConstCharPtr())
                {
                    name += "Ptr";
                }

                return Argument(IdentifierName(name));
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
            yield return SyntaxBuilder.InteropServices.ToFullString();
        }

        private string GetClassName(CppFunction cppFunction)
        {
            var file = Path.GetFileNameWithoutExtension(cppFunction.Span.Start.File);
            var name = GetManagedName(file);
            return name + "API";
        }
    }
}
