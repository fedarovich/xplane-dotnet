using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace BindingsGenerator
{
    internal static class SyntaxExtensions
    {
        internal static readonly NameSyntax SystemNamespace = IdentifierName(nameof(System));
        internal static readonly NameSyntax CompilerServices = BuildQualifiedName("System.Runtime.CompilerServices");
        internal static readonly NameSyntax InteropServices = BuildQualifiedName("System.Runtime.InteropServices");

        internal static readonly ISet<string> Keywords;

        static SyntaxExtensions()
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
                attributeType = QualifiedName(CompilerServices, (IdentifierNameSyntax) argumentType);
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

        public static T AddManagedTypeAttribute<T>(this T member, TypeSyntax type) where T : MemberDeclarationSyntax
        {
            return (T)member.AddAttributeLists(
                AttributeList(SingletonSeparatedList(
                        Attribute(IdentifierName("ManagedTypeAttribute"))
                            .AddArgumentListArguments(AttributeArgument(
                                TypeOfExpression(type)))))
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

        public static bool IsKeyword(this string name) => Keywords.Contains(name);
    }
}
