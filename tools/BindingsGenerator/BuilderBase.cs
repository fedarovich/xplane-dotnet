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

        public abstract void Build(IEnumerable<T> cppElements);

        protected string DefaultNamespace => Workspace.CurrentSolution.Projects.Single(p => p.Id == ProjectId).DefaultNamespace;

        protected abstract string GetNativeName(T cppElement);
        
        protected virtual bool CanProcess(T cppElement) => true;

        protected string GetFullNamespace(T cppElement) => $"{DefaultNamespace}.{GetRelativeNamespace(cppElement)}";

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

        protected virtual IEnumerable<string> GetImportsCore(MemberDeclarationSyntax type)
        {
            yield return nameof(System);

            var namespaces =
                from node in type.DescendantNodes()
                from annotation in node.GetAnnotations(Annotations.Namespace)
                select annotation.Data;
            foreach (var ns in namespaces)
            {
                yield return ns;
            }
        }

        private IEnumerable<UsingDirectiveSyntax> GetImports(MemberDeclarationSyntax type, string @namespace)
        {
            return GetImportsCore(type)
                .Distinct()
                .Where(ns => !@namespace.StartsWith(ns, StringComparison.Ordinal))
                .OrderBy(ns => ns)
                .Select(SyntaxExtensions.BuildQualifiedName)
                .Select(UsingDirective);
        }

        protected virtual void BuildDocument(string relativeNamespace, MemberDeclarationSyntax type, string managedName)
        {
            var @namespace = BuildNamespaceNameSyntax();
            var ns = NamespaceDeclaration(@namespace)
                .WithMembers(SingletonList(type));

            var unit = CompilationUnit()
                .WithUsings(List(GetImports(type, @namespace.ToFullString())))
                .AddMembers(ns)
                .NormalizeWhitespace();

            var directoryParts = new List<string>(4) {Directory};
            directoryParts.AddRange(relativeNamespace.Split(".", StringSplitOptions.RemoveEmptyEntries));
            var directory = Path.Combine(directoryParts.ToArray());
            System.IO.Directory.CreateDirectory(directory);
            var filename = $"{managedName}.Generated.cs";
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
                return SyntaxExtensions.BuildQualifiedName(DefaultNamespace, relativeNamespace);
            }
        }
    }
}