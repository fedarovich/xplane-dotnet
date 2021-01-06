using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CppAst;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace BindingsGenerator
{
    internal static class SyntaxBuilder
    {
        internal static readonly NameSyntax SystemNamespace = IdentifierName(nameof(System));
        internal static readonly NameSyntax CompilerServices = BuildQualifiedName("System.Runtime.CompilerServices");
        internal static readonly NameSyntax InteropServices = BuildQualifiedName("System.Runtime.InteropServices");

        internal static readonly NameSyntax IntPtrName = IdentifierName(nameof(IntPtr));
        internal static readonly NameSyntax Utf8StringName = BuildQualifiedName("XP.SDK.Utf8String");

        internal static readonly IReadOnlyCollection<string> Keywords;

        static SyntaxBuilder()
        {
            var keywords = 
                from fieldInfo in typeof(SyntaxKind).GetFields(BindingFlags.Public | BindingFlags.Static)
                where fieldInfo.Name.EndsWith("Keyword")
                select Token((SyntaxKind)fieldInfo.GetRawConstantValue()).ToFullString();

            Keywords = new HashSet<string>(keywords);
        }

        public static T AddAggressiveInlining<T>(this T member, bool fullyQualified = false) where T : MemberDeclarationSyntax
        {
            NameSyntax attributeType = IdentifierName(nameof(MethodImplAttribute));
            NameSyntax argumentType = IdentifierName(nameof(MethodImplOptions));
            if (fullyQualified)
            {
                attributeType = QualifiedName(CompilerServices, (IdentifierNameSyntax) attributeType);
                argumentType = QualifiedName(CompilerServices, (IdentifierNameSyntax) argumentType);
            }

            return (T) member.AddAttributeLists(
                    AttributeList(SingletonSeparatedList(
                        Attribute(attributeType)
                            .AddArgumentListArguments(AttributeArgument(MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                argumentType,
                                IdentifierName(nameof(MethodImplOptions.AggressiveInlining))))))))
                .WithAdditionalAnnotations(new SyntaxAnnotation(Annotations.Namespace, CompilerServices.ToFullString()));
        }

        public static T AddDllImport<T>(this T member, string entryPoint, bool fullyQualified = false) where T : MemberDeclarationSyntax
        {
            NameSyntax attributeType = IdentifierName(nameof(DllImportAttribute));
            if (fullyQualified)
            {
                attributeType = QualifiedName(InteropServices, (IdentifierNameSyntax) attributeType);
            }

            return (T)member.AddAttributeLists(
                    AttributeList(SingletonSeparatedList(
                        Attribute(attributeType)
                            .AddArgumentListArguments(
                                AttributeArgument(MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    IdentifierName("Lib"),
                                    IdentifierName("Name"))),
                                AttributeArgument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(entryPoint)))
                                    .WithNameEquals(NameEquals(nameof(DllImportAttribute.EntryPoint))),
                                AttributeArgument(LiteralExpression(SyntaxKind.TrueLiteralExpression))
                                    .WithNameEquals(NameEquals(nameof(DllImportAttribute.ExactSpelling)))
                            ))))
                .WithAdditionalAnnotations(new SyntaxAnnotation(Annotations.Namespace, InteropServices.ToFullString()));
        }

        public static T AddManagedTypeAttribute<T>(this T member, TypeSyntax type) where T : MemberDeclarationSyntax
        {
            return (T)member.AddAttributeLists(
                AttributeList(SingletonSeparatedList(
                        Attribute(IdentifierName("ManagedTypeAttribute"))
                            .AddArgumentListArguments(AttributeArgument(
                                TypeOfExpression(type)))))
                    .WithAdditionalAnnotations(new SyntaxAnnotation(Annotations.Namespace, CompilerServices.ToFullString())));
        }

        public static T AddSkipLocalsInitAttribute<T>(this T member) where T : MemberDeclarationSyntax
        {
            return (T)member.AddAttributeLists(
                AttributeList(SingletonSeparatedList(
                    Attribute(IdentifierName(nameof(SkipLocalsInitAttribute)))))
                    .WithAdditionalAnnotations(new SyntaxAnnotation(Annotations.Namespace, CompilerServices.ToFullString())));
        }

        public static DelegateDeclarationSyntax AddUnmanagedFunctionPointerAttribute(this DelegateDeclarationSyntax member)
        {
            return member.AddAttributeLists(
                AttributeList(
                    SingletonSeparatedList(
                        Attribute(IdentifierName(nameof(UnmanagedFunctionPointerAttribute)))
                            .AddArgumentListArguments(
                                AttributeArgument(MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    IdentifierName(nameof(CallingConvention)),
                                    IdentifierName(nameof(CallingConvention.Cdecl)))),
                                AttributeArgument(LiteralExpression(SyntaxKind.FalseLiteralExpression))
                                    .WithNameEquals(NameEquals(nameof(UnmanagedFunctionPointerAttribute.BestFitMapping))),
                                AttributeArgument(LiteralExpression(SyntaxKind.FalseLiteralExpression))
                                    .WithNameEquals(NameEquals(nameof(UnmanagedFunctionPointerAttribute.SetLastError))))))
                    .WithAdditionalAnnotations(new SyntaxAnnotation(Annotations.Namespace,
                        InteropServices.ToFullString())));
        }

        public static FunctionPointerTypeSyntax WithUnmanagedCallingConvention(this FunctionPointerTypeSyntax functionPointerTypeSyntax)
        {
            return functionPointerTypeSyntax
                .WithCallingConvention(
                    FunctionPointerCallingConvention(Token(SyntaxKind.UnmanagedKeyword))
                );
        }

        public static FunctionPointerTypeSyntax WithCdeclCallingConvention(this FunctionPointerTypeSyntax functionPointerTypeSyntax)
        {
            return functionPointerTypeSyntax
                .WithCallingConvention(
                    FunctionPointerCallingConvention(Token(SyntaxKind.UnmanagedKeyword))
                        .AddUnmanagedCallingConventionListCallingConventions(
                            FunctionPointerUnmanagedCallingConvention(Identifier("Cdecl")))
                );
        }

        public static ExpressionStatementSyntax DeclareLocals(bool localsInit)
        {
            return ExpressionStatement(
                InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName("IL"),
                        IdentifierName("DeclareLocals")),
                    ArgumentList(SingletonSeparatedList(
                        Argument(LiteralExpression(
                            localsInit ? SyntaxKind.TrueLiteralExpression : SyntaxKind.FalseLiteralExpression))))));
        }

        public static LocalDeclarationStatementSyntax DeclareResultVariable(TypeSyntax type, out IdentifierNameSyntax resultVariableName)
        {
            const string name = "result";
            resultVariableName = IdentifierName(name);
            return LocalDeclarationStatement(
                VariableDeclaration(type).AddVariables(VariableDeclarator(name)));
        }

        public static LocalDeclarationStatementSyntax DeclareDelegatePointerVariable(string parameterName, string variableName)
        {
            return LocalDeclarationStatement(
                VariableDeclaration(IdentifierName("IntPtr"))
                    .WithAdditionalAnnotations(new SyntaxAnnotation(Annotations.Namespace, SyntaxBuilder.InteropServices.ToFullString()))
                    .AddVariables(VariableDeclarator(variableName)
                        .WithInitializer(
                            EqualsValueClause(
                                ConditionalExpression(
                                    BinaryExpression(
                                        SyntaxKind.NotEqualsExpression,
                                        IdentifierName(parameterName),
                                        LiteralExpression(SyntaxKind.NullLiteralExpression)),
                                    InvocationExpression(
                                        MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            IdentifierName(nameof(Marshal)),
                                            IdentifierName(nameof(Marshal.GetFunctionPointerForDelegate))),
                                        ArgumentList(SingletonSeparatedList(Argument(IdentifierName(parameterName))))),
                                    LiteralExpression(
                                        SyntaxKind.DefaultLiteralExpression,
                                        Token(SyntaxKind.DefaultKeyword)))))));
        }

        public static ExpressionStatementSyntax CallKeepAlive(ExpressionSyntax parameter)
        {
            return ExpressionStatement(
                InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName("GC"),
                        IdentifierName("KeepAlive")),
                    ArgumentList(SingletonSeparatedList(Argument(parameter)))));
        }

        public static ExpressionStatementSyntax CallGuard(IdentifierNameSyntax identifier)
        {
            return ExpressionStatement(
                InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName("Guard"),
                        IdentifierName("NotNull")),
                    ArgumentList(SingletonSeparatedList(Argument(identifier)))));
        }

        public static LocalDeclarationStatementSyntax DeclareLengthForUtf8Variable(string lengthName, string utf16Name)
        {
            return LocalDeclarationStatement(
                VariableDeclaration(PredefinedType(Token(SyntaxKind.IntKeyword)))
                    .AddVariables(VariableDeclarator(lengthName)
                        .WithInitializer(EqualsValueClause(
                            BinaryExpression(
                                SyntaxKind.AddExpression,
                                BinaryExpression(
                                        SyntaxKind.MultiplyExpression,
                                        MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            IdentifierName(utf16Name),
                                            IdentifierName(nameof(Span<char>.Length))),
                                        LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(3))),
                                LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(4)))
                        ))));
        }

        public static LocalDeclarationStatementSyntax DeclareSpanForUtf8Variable(string utf8Name, string lengthName)
        {
            return LocalDeclarationStatement(
                VariableDeclaration(BuildSpanName(SyntaxKind.ByteKeyword))
                    .AddVariables(VariableDeclarator(utf8Name)
                        .WithInitializer(EqualsValueClause(
                            ConditionalExpression(
                        BinaryExpression(
                                    SyntaxKind.LessThanOrEqualExpression,
                                    IdentifierName(lengthName),
                                    LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(4096))),
                        StackAllocArrayCreationExpression(
                                ArrayType(PredefinedType(Token(SyntaxKind.ByteKeyword)))
                                        .AddRankSpecifiers(ArrayRankSpecifier(SingletonSeparatedList<ExpressionSyntax>(IdentifierName(lengthName))))),
                        InvocationExpression(
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                IdentifierName(nameof(GC)),
                                GenericName(nameof(GC.AllocateUninitializedArray)).AddTypeArgumentListArguments(PredefinedType(Token(SyntaxKind.ByteKeyword))))
                            )
                            .AddArgumentListArguments(Argument(IdentifierName(lengthName)))
                        )))));
        }

        public static LocalDeclarationStatementSyntax DeclareUtf8StringVariable(string utf8Name, string utf16Name, string utf8StringName)
        {
            return LocalDeclarationStatement(
                VariableDeclaration(IdentifierName("var"))
                    .AddVariables(VariableDeclarator(utf8StringName)
                        .WithInitializer(EqualsValueClause(
                            InvocationExpression(
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    IdentifierName("Utf8String"),
                                    IdentifierName("FromUtf16Unsafe")))
                                .AddArgumentListArguments(Argument(IdentifierName(utf16Name)), Argument(IdentifierName(utf8Name)))
                        ))));
        }

        public static T AddUnsafeIfNeeded<T>(this T method) where T : BaseMethodDeclarationSyntax
        {
            if (method.DescendantNodes().OfType<PointerTypeSyntax>().Any() || method.DescendantNodes().OfType<FunctionPointerTypeSyntax>().Any())
            {
                method = (T)method.AddModifiers(Token(SyntaxKind.UnsafeKeyword));
            }

            return method;
        }

        [SuppressMessage("ReSharper", "CoVariantArrayConversion")]
        public static T AddDocumentationComments<T>(this T member, CppComment comments, string nativeName)
            where T : MemberDeclarationSyntax
        {
            var paragraphs = ProcessComments(comments, nativeName);
            var comment = Trivia(
                DocumentationCommentTrivia(SyntaxKind.SingleLineDocumentationCommentTrivia)
                    .AddContent(
                        XmlNewLine(Environment.NewLine),
                        XmlMultiLineElement("summary", List<XmlNodeSyntax>())
                            .AddContent(XmlNewLine(Environment.NewLine))
                            .AddContent(paragraphs.SelectMany(x =>
                                new XmlNodeSyntax[] 
                                {
                                    XmlMultiLineElement("para", List<XmlNodeSyntax>())
                                        .AddContent(XmlNewLine(Environment.NewLine))
                                        .AddContent(x.SelectMany(l => 
                                            new XmlNodeSyntax[]
                                            {
                                                XmlText(l), 
                                                XmlNewLine(Environment.NewLine)
                                            }).ToArray()),
                                    XmlNewLine(Environment.NewLine)
                                }).ToArray()),
                        XmlText(Environment.NewLine)));
            member = member.WithLeadingTrivia(TriviaList(comment));
            return member;
        }

        private static IList<string[]> ProcessComments(CppComment comments, string nativeName)
        {
            var paragraphs = new List<string[]>();
            ProcessComments(comments);
            return paragraphs;

            void ProcessComments(CppComment comment)
            {
                switch (comment)
                {
                    case CppCommentFull full:
                        foreach (var subComment in full.Children)
                        {
                            ProcessComments(subComment);
                        }
                        break;
                    case CppCommentParagraph para:
                        var paraLines = para.Children
                            .OfType<CppCommentText>()
                            .Select(t => t.Text.Trim())
                            .Where(t => !string.IsNullOrEmpty(t) && t != nativeName)
                            .ToArray();
                        if (paraLines.Length > 0)
                        {
                            paragraphs.Add(paraLines);
                        }
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
        }

        public static NameSyntax BuildQualifiedName(params string[] partialNames)
        {
            return BuildQualifiedName(string.Join(".", partialNames));
        }

        public static NameSyntax BuildQualifiedName(string nameString)
        {
            var queue = new Queue<string>(nameString.Split(".", StringSplitOptions.RemoveEmptyEntries));
            NameSyntax name = IdentifierName(queue.Dequeue());
            while (queue.TryDequeue(out var part))
            {
                name = QualifiedName(name, IdentifierName(part));
            }
            return name;
        }

        public static NameSyntax BuildSpanName(TypeSyntax type)
        {
            return GenericName("Span").AddTypeArgumentListArguments(type);
        }

        public static NameSyntax BuildSpanName(SyntaxKind primitiveType)
        {
            return GenericName("Span").AddTypeArgumentListArguments(PredefinedType(Token(primitiveType)));
        }

        public static NameSyntax BuildReadOnlySpanName(TypeSyntax type)
        {
            return GenericName("ReadOnlySpan").AddTypeArgumentListArguments(type);
        }

        public static NameSyntax BuildReadOnlySpanName(SyntaxKind primitiveType)
        {
            return GenericName("ReadOnlySpan").AddTypeArgumentListArguments(PredefinedType(Token(primitiveType)));
        }

        public static bool IsKeyword(this string name) => Keywords.Contains(name);
    }
}
