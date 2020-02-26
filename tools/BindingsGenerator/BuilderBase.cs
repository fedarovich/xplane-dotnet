using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CppAst;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
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

        public abstract Task BuildAsync(IEnumerable<T> cppElements);

        protected string DefaultNamespace => Workspace.CurrentSolution.Projects.Single(p => p.Id == ProjectId).DefaultNamespace;

        protected abstract string GetNativeName(T cppElement);
        
        protected virtual bool CanProcess(T cppElement) => true;

        protected string GetFullNamespace(T cppElement) => $"{DefaultNamespace}.{GetRelativeNamespace(cppElement)}";

        protected string GetNativeLibrary(T cppElement)
        {
            var file = cppElement.Span.Start.File;
            var ns = Path.GetFileName(Path.GetDirectoryName(file));
            return ns;
        }

        protected virtual string GetRelativeNamespace(T cppElement)
        {
            var file = cppElement.Span.Start.File;
            var ns = Path.GetFileName(Path.GetDirectoryName(file));
            return ns;
        }

        protected virtual string GetManagedName(string nativeName)
        {
            if (nativeName.StartsWith("XPLM"))
            {
                nativeName = nativeName[4..];
            }
            else if (nativeName.StartsWith("XPU") && char.IsUpper(nativeName[3]) && nativeName[3] != 'I')
            {
                nativeName = nativeName[3..];
            }
            else if (nativeName.StartsWith("XP"))
            {
                nativeName = nativeName[2..];
            }

            if (nativeName.IsKeyword())
            {
                nativeName = "@" + nativeName;
            }

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
                .Select(SyntaxBuilder.BuildQualifiedName)
                .Select(UsingDirective);
        }

        protected virtual async Task BuildDocumentAsync(string relativeNamespace, MemberDeclarationSyntax type, string managedName)
        {
            var @namespace = BuildNamespaceNameSyntax();
            var ns = NamespaceDeclaration(@namespace)
                .WithMembers(SingletonList(type));

            var unit = CompilationUnit()
                .WithUsings(List(GetImports(type, @namespace.ToFullString())))
                .AddMembers(ns);

            var formattedUnit = Formatter.Format(unit, Workspace);

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
                    SourceText.From(formattedUnit.ToFullString()), VersionStamp.Create(), path)),
                filePath: path);

            var doc = Workspace.AddDocument(documentInfo);
            doc = await Formatter.OrganizeImportsAsync(doc);
            var changedSolution = Workspace.CurrentSolution.WithDocumentText(
                doc.Id,
                TextAndVersion.Create(
                    SourceText.From((await doc.GetSyntaxRootAsync()).ToFullString()), VersionStamp.Create(), path));
            Workspace.TryApplyChanges(changedSolution);

            NameSyntax BuildNamespaceNameSyntax()
            {
                return SyntaxBuilder.BuildQualifiedName(DefaultNamespace, relativeNamespace);
            }
        }
    }
}