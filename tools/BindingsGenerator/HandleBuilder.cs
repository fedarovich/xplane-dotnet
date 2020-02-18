using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using CppAst;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace BindingsGenerator
{
    public class HandleBuilder : TypeBuilderBase<CppTypedef>
    {
        public HandleBuilder(AdhocWorkspace workspace, ProjectId projectId, string directory, TypeMap typeMap) 
            : base(workspace, projectId, directory, typeMap)
        {
        }

        protected override MemberDeclarationSyntax BuildType(CppTypedef cppType, string nativeName, string managedName)
        {
            TypeSyntax baseManagedType = null;

            if (cppType.ElementType is CppPrimitiveType primitiveType)
            {
                baseManagedType = TypeMap.GetType(primitiveType.GetDisplayName()).TypeSyntax;
            }
            else if (cppType.ElementType is CppPointerType pointerType &&
                     pointerType.ElementType is CppPrimitiveType p && p.Kind == CppPrimitiveKind.Void)
            {
                baseManagedType = QualifiedName(IdentifierName("System"), IdentifierName("IntPtr"));
            }

            if (baseManagedType == null)
                return null;

            var managedType = IdentifierName(managedName);
            var field = IdentifierName("_value");
            var param = IdentifierName("value");

            var @struct = StructDeclaration(managedName)
                    .AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.ReadOnlyKeyword))
                    .AddBaseListTypes(SimpleBaseType(
                        QualifiedName(IdentifierName("System"), GenericName("IEquatable").AddTypeArgumentListArguments(managedType))))
                    .AddMembers(
                        FieldDeclaration(VariableDeclaration(baseManagedType).AddVariables(VariableDeclarator("_value")))
                            .AddModifiers(Token(SyntaxKind.PrivateKeyword), Token(SyntaxKind.ReadOnlyKeyword)),

                        ConstructorDeclaration(managedName)
                            .AddModifiers(Token(SyntaxKind.PublicKeyword))
                            .AddParameterListParameters(Parameter(Identifier("value")).WithType(baseManagedType))
                            .AddAggressiveInlining()
                            .WithExpressionBody(
                                ArrowExpressionClause(
                                    AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, field, param)))
                            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),

                        ConversionOperatorDeclaration(Token(SyntaxKind.ImplicitKeyword), managedType)
                            .AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword))
                            .AddParameterListParameters(Parameter(Identifier("value")).WithType(baseManagedType))
                            .AddAggressiveInlining()
                            .WithExpressionBody(
                                ArrowExpressionClause(
                                    ObjectCreationExpression(managedType)
                                        .AddArgumentListArguments(Argument(param))))
                            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),

                        ConversionOperatorDeclaration(Token(SyntaxKind.ExplicitKeyword), baseManagedType)
                            .AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword))
                            .AddParameterListParameters(Parameter(Identifier("value")).WithType(managedType))
                            .AddAggressiveInlining()
                            .WithExpressionBody(ArrowExpressionClause(field))
                            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),

                        MethodDeclaration(PredefinedType(Token(SyntaxKind.BoolKeyword)), "Equals")
                            .AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.OverrideKeyword))
                            .AddParameterListParameters(Parameter(Identifier("obj")).WithType(PredefinedType(Token(SyntaxKind.ObjectKeyword))))
                            .AddAggressiveInlining()
                            .WithExpressionBody(ArrowExpressionClause(
                                BinaryExpression(SyntaxKind.LogicalAndExpression,
                                    IsPatternExpression(
                                        IdentifierName("obj"),
                                        DeclarationPattern(
                                            managedType,
                                            SingleVariableDesignation(Identifier("other")))),
                                    InvocationExpression(
                                        IdentifierName("Equals")).AddArgumentListArguments(Argument(IdentifierName("other"))))))
                            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),

                        MethodDeclaration(PredefinedType(Token(SyntaxKind.IntKeyword)), "GetHashCode")
                            .AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.OverrideKeyword))
                            .AddAggressiveInlining()
                            .WithExpressionBody(ArrowExpressionClause(
                                InvocationExpression(
                                    MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, field, IdentifierName("GetHashCode")))))
                            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),

                        MethodDeclaration(PredefinedType(Token(SyntaxKind.BoolKeyword)), "Equals")
                            .AddModifiers(Token(SyntaxKind.PublicKeyword))
                            .AddParameterListParameters(Parameter(Identifier("other")).WithType(managedType))
                            .AddAggressiveInlining()
                            .WithExpressionBody(ArrowExpressionClause(
                                BinaryExpression(SyntaxKind.EqualsExpression,
                                    field,
                                    MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName("other"), field))))
                            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),

                        OperatorDeclaration(PredefinedType(Token(SyntaxKind.BoolKeyword)), Token(SyntaxKind.EqualsEqualsToken))
                            .AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword))
                            .AddParameterListParameters(
                                Parameter(Identifier("left")).WithType(managedType),
                                Parameter(Identifier("right")).WithType(managedType))
                            .AddAggressiveInlining()
                            .WithExpressionBody(ArrowExpressionClause(
                                InvocationExpression(
                                    MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        IdentifierName("left"),
                                        IdentifierName("Equals")))
                                    .AddArgumentListArguments(Argument(IdentifierName("right")))))
                            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),

                        OperatorDeclaration(PredefinedType(Token(SyntaxKind.BoolKeyword)), Token(SyntaxKind.ExclamationEqualsToken))
                            .AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword))
                            .AddParameterListParameters(
                                Parameter(Identifier("left")).WithType(managedType),
                                Parameter(Identifier("right")).WithType(managedType))
                            .AddAggressiveInlining()
                            .WithExpressionBody(ArrowExpressionClause(
                                PrefixUnaryExpression(
                                    SyntaxKind.LogicalNotExpression,
                                    InvocationExpression(
                                            MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                IdentifierName("left"),
                                                IdentifierName("Equals")))
                                        .AddArgumentListArguments(Argument(IdentifierName("right"))))))
                            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                        );


            TypeMap.RegisterType(cppType, nativeName, managedName);

            return @struct;
        }

        protected override string GetNativeName(CppTypedef type) => type.Name;

        protected override bool IsSameType(TypeInfo typeInfo, CppTypedef cppType)
        {
            return base.IsSameType(typeInfo, cppType) || typeInfo.IsEnum;
        }

        protected override bool CanProcess(CppTypedef cppElement)
        {
            if (cppElement.ElementType is CppPrimitiveType primitiveType)
                return true;

            if (cppElement.ElementType is CppPointerType pointerType &&
                pointerType.ElementType is CppPrimitiveType p && 
                p.Kind == CppPrimitiveKind.Void)
                return true;

            return false;
        }
    }
}
