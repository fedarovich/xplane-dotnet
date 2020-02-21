using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using CppAst;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace BindingsGenerator
{
    class Program
    {
        private EnumBuilder _enumBuilder;
        private HandleBuilder _handleBuilder;
        private DelegateBuilder _delegateBuilder;
        private StructBuilder _structBuilder;
        static Task<int> Main(string[] args) => CommandLineApplication.ExecuteAsync<Program>(args);

        [DirectoryExists]
        [Option("-s|--source <DIR>", "X-Plane SDK root directory.", CommandOptionType.SingleValue)]
        [Required]
        public string SdkRoot { get; set; }

        [Option("-o|--output-dir <CSPROJ>", "Output directory.", CommandOptionType.SingleValue)]
        [Required]
        public string OutputProjectDir { get; set; }

        private async Task<int> OnExecuteAsync(CommandLineApplication app,
            CancellationToken cancellationToken = default)
        {
            var outputDir = Path.IsPathFullyQualified(OutputProjectDir)
                ? OutputProjectDir
                : Path.GetFullPath(OutputProjectDir);
            Directory.CreateDirectory(outputDir);

            var workspace = new AdhocWorkspace();
            var projectId = ProjectId.CreateNewId();
            var projectInfo = ProjectInfo.Create(projectId, VersionStamp.Create(), "XP.SDK", "XP.SDK", LanguageNames.CSharp)
                .WithDefaultNamespace("XP.SDK");
            var project = workspace.AddProject(projectInfo);

            var typeMap = new TypeMap();
            _enumBuilder = new EnumBuilder(workspace, projectId, outputDir, typeMap);
            _handleBuilder = new HandleBuilder(workspace, projectId, outputDir, typeMap);
            _delegateBuilder = new DelegateBuilder(workspace, projectId, outputDir, typeMap);
            _structBuilder = new StructBuilder(workspace, projectId, outputDir, typeMap);
            var functionBuilder = new FunctionBuilder(workspace, projectId, outputDir, typeMap);

            var xplmHeadersPath = Path.Combine(SdkRoot, "CHeaders", "XPLM");
            if (!Directory.Exists(xplmHeadersPath))
                throw new DirectoryNotFoundException($"Directory '{xplmHeadersPath}' does not exist.");
            var xmplHeaders = Directory.EnumerateFiles(xplmHeadersPath, "*.h");
            
            var xpWidgetsHeadersPath = Path.Combine(SdkRoot, "CHeaders", "Widgets");
            if (!Directory.Exists(xpWidgetsHeadersPath))
                throw new DirectoryNotFoundException($"Directory '{xpWidgetsHeadersPath}' does not exist.");
            var xpWidgetsHeaders = Directory.EnumerateFiles(xpWidgetsHeadersPath, "*.h");
            var headers = xmplHeaders.Concat(xpWidgetsHeaders)
                .Where(x => Path.GetFileName(x) != "XPStandardWidgets.h");

            var parserOptions = new CppParserOptions
            {
                Defines = {"IBM", "XPLM301", "XPLM300", "XPLM210", "XPLM200"},
                ParseSystemIncludes = false,
                TargetCpu = CppTargetCpu.X86_64,
                IncludeFolders = { xplmHeadersPath },
            };


            var compilation = CppParser.ParseFiles(headers.ToList(), parserOptions);
            foreach (var child in compilation.Children().OfType<CppType>().OrderBy(x => x, new FilePathComparer()))
            {
                BuildType(child);
            }
            //_enumBuilder.Build(compilation.Enums);
            //_handleBuilder.Build(compilation.Typedefs);
            //_delegateBuilder.Build(compilation.Typedefs);
            //_structBuilder.Build(compilation.Classes);
            //functionBuilder.Build(compilation.Functions);

            foreach (var document in workspace.CurrentSolution.Projects.SelectMany(p => p.Documents))
            {
                var text = await document.GetTextAsync(cancellationToken);
                await using var writer = new StreamWriter(document.FilePath, false);
                text.Write(writer, cancellationToken);
            }

            return 0;
        }

        private void BuildType(dynamic item)
        {
            Process(item);
        }

        private void Process(CppEnum item)
        {
            _enumBuilder.Build(new [] { item });
        }

        private void Process(CppTypedef item)
        {
            _handleBuilder.Build(new[] { item });
            _delegateBuilder.Build(new[] { item });
        }

        private void Process(CppClass item)
        {
            _structBuilder.Build(new[] { item });
        }

        private void Process<T>(T item) where T : ICppDeclaration
        {
            // Fallback. Do nothing.
        }

        private class FilePathComparer : IComparer<CppType>
        {
            public int Compare(CppType x, CppType y)
            {
                var xName = Path.GetFileNameWithoutExtension(x.Span.Start.File);
                var yName = Path.GetFileNameWithoutExtension(y.Span.Start.File);

                if (xName == yName)
                    return x.Span.Start.Line.CompareTo(y.Span.Start.Line);

                if (xName == "XPLMDefs")
                    return -1;

                if (yName == "XPLMDefs")
                    return 1;

                return StringComparer.OrdinalIgnoreCase.Compare(xName, yName);
            }
        }
    }
}
