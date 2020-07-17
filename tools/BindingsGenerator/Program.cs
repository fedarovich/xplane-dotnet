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

            var typeMap = new TypeMap(BuildTypeCallback);
            _enumBuilder = new EnumBuilder(workspace, projectId, outputDir, typeMap)
                .Map("xpMainWindowStyle_MainWindow", "MainWindowType")
                .Map("xpProperty_MainWindowType", "MainWindowProperty")
                .Map("xpMessage_CloseButtonPushed", "MainWindowMessage")
                .Map("xpSubWindowStyle_SubWindow", "SubWindowType")
                .Map("xpProperty_SubWindowType", "SubWindowProperty")
                .Map("xpPushButton", "ButtonType")
                .Map("xpButtonBehaviorPushButton", "ButtonBehavior")
                .Map("xpProperty_ButtonType", "ButtonProperty")
                .Map("xpMsg_PushButtonPressed", "ButtonMessage")
                .Map("xpTextEntryField", "TextFieldType")
                .Map("xpProperty_EditFieldSelStart", "TextFieldProperty")
                .Map("xpMsg_TextFieldChanged", "TextFieldMessage")
                .Map("xpScrollBarTypeScrollBar", "ScrollBarType")
                .Map("xpProperty_ScrollBarSliderPosition", "ScrollBarProperty")
                .Map("xpMsg_ScrollBarSliderPositionChanged", "ScrollBarMessage")
                .Map("xpProperty_CaptionLit", "CaptionProperty")
                .Map("xpShip", "GeneralGraphicsType")
                .Map("xpProperty_GeneralGraphicsType", "GeneralGraphicsProperty")
                .Map("xpProperty_ProgressPosition", "ProgressBarProperty");
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
                /*.Where(x => Path.GetFileName(x) != "XPStandardWidgets.h")*/;

            var parserOptions = new CppParserOptions
            {
                Defines = {"IBM", "XPLM303", "XPLM302", "XPLM301", "XPLM300", "XPLM210", "XPLM200"},
                ParseSystemIncludes = false,
                TargetCpu = CppTargetCpu.X86_64,
                IncludeFolders = { xplmHeadersPath },
            };


            var compilation = CppParser.ParseFiles(headers.ToList(), parserOptions);
            foreach (var child in compilation.Children().OfType<CppType>())
            {
                await BuildTypeAsync(child);
            }

            var functionsByHeader = compilation
                .Functions
                .ToLookup(x => x.Span.Start.File);
            foreach (var functionsInHeader in functionsByHeader)
            {
                await functionBuilder.BuildAsync(functionsInHeader);
            }

            foreach (var document in workspace.CurrentSolution.Projects.SelectMany(p => p.Documents))
            {
                var text = await document.GetTextAsync(cancellationToken);
                await using var writer = new StreamWriter(document.FilePath, false);
                text.Write(writer, cancellationToken);
            }

            return 0;

            async Task BuildTypeCallback(dynamic item)
            {
                using (Log.PushIdent())
                {
                    await BuildTypeAsync(item);
                }
            }
        }

        private async Task BuildTypeAsync(dynamic item)
        {
            await ProcessAsync(item);
        }

        private async Task ProcessAsync(CppEnum item)
        {
            await _enumBuilder.BuildAsync(new [] { item });
        }

        private async Task ProcessAsync(CppTypedef item)
        {
            await _handleBuilder.BuildAsync(new[] { item });
            await _delegateBuilder.BuildAsync(new[] { item });
        }

        private async Task ProcessAsync(CppClass item)
        {
            await _structBuilder.BuildAsync(new[] { item });
        }

        private Task ProcessAsync<T>(T item) where T : CppElement
        {
            Log.WriteLine($"Skipped {item}.", ConsoleColor.DarkYellow);
            return Task.CompletedTask;
        }
    }
}
