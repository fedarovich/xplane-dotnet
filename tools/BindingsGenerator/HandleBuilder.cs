using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CppAst;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace BindingsGenerator
{
    public class HandleBuilder
    {
        private readonly AdhocWorkspace _workspace;
        private readonly ProjectId _projectId;
        private readonly string _directory;
        private readonly TypeMap _typeMap;

        public HandleBuilder(AdhocWorkspace workspace, ProjectId projectId, string directory, TypeMap typeMap)
        {
            _workspace = workspace;
            _projectId = projectId;
            _directory = directory;
            _typeMap = typeMap;
        }

        public void Build(CppContainerList<CppTypedef> typedefs, string @namespace)
        {
            foreach (var cppTypedef in typedefs)
            {
                if (_typeMap.TryGetType(cppTypedef.Name, out var typeInfo))
                {
                    if (typeInfo.IsSame(cppTypedef) || typeInfo.IsEnum)
                        continue;

                    throw new ArgumentException("Duplicate type " + cppTypedef.Name);
                }

                TypeSyntax managedType = null;

                if (cppTypedef.ElementType is CppPrimitiveType primitiveType)
                {
                    managedType = _typeMap.GetType(primitiveType.GetDisplayName()).TypeSyntax;
                }
                else if (cppTypedef.ElementType is CppPointerType pointerType &&
                         pointerType.ElementType is CppPrimitiveType p && p.Kind == CppPrimitiveKind.Void)
                {
                    managedType = QualifiedName(IdentifierName("System"), IdentifierName("IntPtr"));
                }
                
                if (managedType == null)
                    continue;

                var managedName = cppTypedef.Name;

                var field = IdentifierName("_value");
                var param = IdentifierName("value");

                var @struct = StructDeclaration(managedName)
                    .AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.ReadOnlyKeyword))
                    .AddMembers(
                        FieldDeclaration(VariableDeclaration(managedType).AddVariables(VariableDeclarator("_value")))
                            .AddModifiers(Token(SyntaxKind.PrivateKeyword), Token(SyntaxKind.ReadOnlyKeyword)),

                        ConstructorDeclaration(managedName)
                            .AddModifiers(Token(SyntaxKind.PublicKeyword))
                            .AddParameterListParameters(Parameter(Identifier("value")).WithType(managedType))
                            .WithExpressionBody(
                                ArrowExpressionClause(
                                    AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, field, param)))
                            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),

                        ConversionOperatorDeclaration(Token(SyntaxKind.ImplicitKeyword), IdentifierName(managedName))
                            .AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword))
                            .AddParameterListParameters(Parameter(Identifier("value")).WithType(managedType))
                            .WithExpressionBody(
                                ArrowExpressionClause(
                                    ObjectCreationExpression(IdentifierName(managedName))
                                        .AddArgumentListArguments(Argument(param))))
                            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),

                        ConversionOperatorDeclaration(Token(SyntaxKind.ExplicitKeyword), managedType)
                            .AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword))
                            .AddParameterListParameters(Parameter(Identifier("value")).WithType(IdentifierName(managedName)))
                            .WithExpressionBody(ArrowExpressionClause(field))
                            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),

                        // TODO: Override Equals and add IEquatable interface

                        MethodDeclaration(PredefinedType(Token(SyntaxKind.IntKeyword)), "GetHashCode")
                            .AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.OverrideKeyword))
                            .WithExpressionBody(ArrowExpressionClause(
                                InvocationExpression(
                                    MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, field, IdentifierName("GetHashCode")))))
                            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),

                        MethodDeclaration(PredefinedType(Token(SyntaxKind.BoolKeyword)), "Equals")
                            .AddModifiers(Token(SyntaxKind.PublicKeyword))
                            .AddParameterListParameters(Parameter(Identifier("other")).WithType(IdentifierName(managedName)))
                            .WithExpressionBody(ArrowExpressionClause(
                                BinaryExpression(SyntaxKind.EqualsExpression, 
                                    field,
                                    MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName("other"), field))))
                            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))

                        );

                BuildDocument(@namespace, @struct, managedName);

                _typeMap.RegisterHandleType(cppTypedef.Name, cppTypedef, managedName);
            }
        }

        private void BuildDocument(string @namespace, StructDeclarationSyntax @struct, string managedName)
        {
            var ns = NamespaceDeclaration(IdentifierName(@namespace))
                .WithMembers(SingletonList<MemberDeclarationSyntax>(@struct))
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
