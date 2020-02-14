using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CppAst;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace BindingsGenerator
{
    public abstract class BuilderBase<T> where T : CppType
    {
        protected readonly AdhocWorkspace Workspace;
        protected readonly ProjectId ProjectId;
        protected readonly string Directory;
        protected readonly TypeMap TypeMap;

        protected BuilderBase(AdhocWorkspace workspace, ProjectId projectId, string directory, TypeMap typeMap)
        {
            Workspace = workspace;
            ProjectId = projectId;
            Directory = directory;
            TypeMap = typeMap;
        }

        public virtual void Build(CppContainerList<T> cppTypes, string @namespace)
        {
            foreach (var cppType in cppTypes)
            {
                if (!CanProcess(cppType))
                    continue;

                string nativeName = GetNativeName(cppType);
                if (TypeMap.TryGetType(nativeName, out var typeInfo))
                {
                    if (IsSameType(typeInfo, cppType))
                        continue;

                    throw new ArgumentException($"Duplicate type: '{nativeName}'.");
                }

                var managedName = GetManagedName(nativeName);

                Console.Write($"  Building type '{managedName}' from '{nativeName}'...");
                var type = BuildType(cppType, nativeName, managedName);
                if (type != null)
                {
                    WriteLineColored("  Done.", ConsoleColor.Green);
                }
                else
                {
                    WriteLineColored("  Skipped.", ConsoleColor.Yellow);
                    continue;
                }
                
                BuildDocument(@namespace, type, managedName);
            }
        }

        protected abstract MemberDeclarationSyntax BuildType(T cppType, string nativeName, string managedName);

        protected abstract string GetNativeName(T type);

        protected virtual bool CanProcess(T cppType) => true;

        protected virtual string GetManagedName(string nativeName)
        {
            if (nativeName.StartsWith("XPLM"))
                return nativeName[4..];

            if (nativeName.StartsWith("XP"))
                return nativeName[2..];

            return nativeName;
        }

        protected virtual bool IsSameType(TypeInfo typeInfo, T cppType) => typeInfo.IsSame(cppType);

        protected virtual void BuildDocument(string @namespace, MemberDeclarationSyntax type, string managedName)
        {
            var ns = NamespaceDeclaration(IdentifierName(@namespace))
                .WithMembers(SingletonList(type));

            var unit = CompilationUnit()
                .AddUsings(
                    UsingDirective(SyntaxExtensions.System),
                    UsingDirective(SyntaxExtensions.CompilerServices))
                .AddMembers(ns)
                .NormalizeWhitespace();

            var filename = $"{managedName}.cs";
            var path = Path.Combine(Directory, filename);

            var documentInfo = DocumentInfo.Create(
                DocumentId.CreateNewId(ProjectId, filename),
                filename,
                loader: TextLoader.From(TextAndVersion.Create(
                    SourceText.From(unit.ToFullString()), VersionStamp.Create(), path)),
                filePath: path);

            var doc = Workspace.AddDocument(documentInfo);
        }

        protected void WriteLineColored(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}
