using System;
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
            var projectInfo = ProjectInfo.Create(projectId, VersionStamp.Create(), "XP.SDK", "XP.SDK", LanguageNames.CSharp);
            var project = workspace.AddProject(projectInfo);
           
            var xplmHeadersPath = Path.Combine(SdkRoot, "CHeaders", "XPLM");
            if (!Directory.Exists(xplmHeadersPath))
                throw new DirectoryNotFoundException($"Directory '{xplmHeadersPath}' does not exist.");
            var xmplHeaders = Directory.EnumerateFiles(xplmHeadersPath, "*.h");
            
            var xpWidgetsHeadersPath = Path.Combine(SdkRoot, "CHeaders", "Widgets");
            if (!Directory.Exists(xpWidgetsHeadersPath))
                throw new DirectoryNotFoundException($"Directory '{xpWidgetsHeadersPath}' does not exist.");
            var xpWidgetsHeaders = Directory.EnumerateFiles(xpWidgetsHeadersPath, "*.h");

            var parserOptions = new CppParserOptions
            {
                Defines = {"IBM"},
                ParseSystemIncludes = false,
                TargetCpu = CppTargetCpu.X86_64
            };

            var nsHeaders = new[]
            {
                (@namespace: "XP.SDK.XPLM", headers: xmplHeaders),
                (@namespace: "XP.SDK.Widgets", headers: xpWidgetsHeaders),
            };

            var typeMap = new TypeMap();
            var enumBuilder = new EnumBuilder(workspace, projectId, outputDir, typeMap);
            var handleBuilder = new HandleBuilder(workspace, projectId, outputDir, typeMap);
            var structBuilder = new StructBuilder(workspace, projectId, outputDir, typeMap);
            var delegateBuilder = new DelegateBuilder(workspace, projectId, outputDir, typeMap);

            foreach (var (@namespace, headers) in nsHeaders)
            {
                foreach (var header in headers)
                {
                    Console.WriteLine("Parsing header {0}", Path.GetFileName(header));
                    var compilation = CppParser.ParseFile(header, parserOptions);
                    enumBuilder.Build(compilation.Enums, @namespace);
                    handleBuilder.Build(compilation.Typedefs, @namespace);
                    structBuilder.Build(compilation.Classes, @namespace);
                    delegateBuilder.Build(compilation.Typedefs, @namespace);
                }
            }

            foreach (var document in workspace.CurrentSolution.Projects.SelectMany(p => p.Documents))
            {
                var text = await document.GetTextAsync(cancellationToken);
                await using var writer = new StreamWriter(document.FilePath, false);
                text.Write(writer, cancellationToken);
            }

            return 0;
        }

        
    }
}
