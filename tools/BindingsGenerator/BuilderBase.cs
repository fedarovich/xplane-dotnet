using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CppAst;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace BindingsGenerator
{
    public abstract class BuilderBase<T> where T : CppElement
    {
        protected AdhocWorkspace Workspace;
        protected ProjectId ProjectId;
        protected string Directory;
        protected TypeMap TypeMap;

        protected BuilderBase(AdhocWorkspace workspace, ProjectId projectId, string directory, TypeMap typeMap)
        {
            Workspace = workspace;
            ProjectId = projectId;
            Directory = directory;
            TypeMap = typeMap;
        }

        public abstract void Build(CppContainerList<T> cppElements);

        protected string DefaultNamespace => Workspace.CurrentSolution.Projects.Single(p => p.Id == ProjectId).DefaultNamespace;

        protected abstract string GetNativeName(T cppElement);
        
        protected virtual bool CanProcess(T cppElement) => true;

        protected virtual string GetRelativeNamespace(T cppElement)
        {
            var file = cppElement.Span.Start.File;
            var ns = Path.GetFileName(Path.GetDirectoryName(file));
            return ns;
        }

        protected virtual string GetManagedName(string nativeName)
        {
            if (nativeName.StartsWith("XPLM"))
                return nativeName[4..];

            if (nativeName.StartsWith("XP"))
                return nativeName[2..];

            return nativeName;
        }

        protected virtual void BuildDocument(string relativeNamespace, MemberDeclarationSyntax type, string managedName)
        {
            var ns = NamespaceDeclaration(BuildNamespaceNameSyntax())
                .WithMembers(SingletonList(type));

            var unit = CompilationUnit()
                .AddUsings(
                    UsingDirective(SyntaxExtensions.SystemNamespace),
                    UsingDirective(SyntaxExtensions.CompilerServices),
                    UsingDirective(SyntaxExtensions.InteropServices))
                .AddMembers(ns)
                .NormalizeWhitespace();

            var directoryParts = new List<string>(4) {Directory};
            directoryParts.AddRange(relativeNamespace.Split(".", StringSplitOptions.RemoveEmptyEntries));
            var directory = Path.Combine(directoryParts.ToArray());
            System.IO.Directory.CreateDirectory(directory);
            var filename = $"{managedName}.cs";
            var path = Path.Combine(directory, filename);

            var documentInfo = DocumentInfo.Create(
                DocumentId.CreateNewId(ProjectId, filename),
                filename,
                loader: TextLoader.From(TextAndVersion.Create(
                    SourceText.From(unit.ToFullString()), VersionStamp.Create(), path)),
                filePath: path);

            var doc = Workspace.AddDocument(documentInfo);

            NameSyntax BuildNamespaceNameSyntax()
            {
                var parts = new List<string>(DefaultNamespace.Split(".", StringSplitOptions.RemoveEmptyEntries));
                parts.AddRange(relativeNamespace.Split(".", StringSplitOptions.RemoveEmptyEntries));
                var queue = new Queue<string>(parts);
                NameSyntax name = IdentifierName(queue.Dequeue());
                while (queue.TryDequeue(out var part))
                {
                    name = QualifiedName(name, IdentifierName(part));
                }
                return name;
            }
        }

        protected void WriteLineColored(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}