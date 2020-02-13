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
            var projectPath = Path.IsPathFullyQualified(OutputProjectDir)
                ? OutputProjectDir
                : Path.GetFullPath(OutputProjectDir);
            Directory.CreateDirectory(projectPath);

            var workspace = new AdhocWorkspace();
            var project = workspace.AddProject("XP.SDK", "C#");

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
            foreach (var (@namespace, headers) in nsHeaders)
            {
                foreach (var header in headers)
                {
                    Console.WriteLine("Parsing header {0}", Path.GetFileName(header));
                    var compilation = CppParser.ParseFile(header, parserOptions);
                    BuildEnums(compilation.Enums, workspace, @namespace);
                }
            }

            return 0;
        }

        private void BuildEnums(CppContainerList<CppEnum> enums, AdhocWorkspace workspace, string @namespace)
        {
            foreach (var cppEnum in enums)
            {
                var name = GetName(cppEnum);
                if (name.StartsWith("XPLM"))
                {
                    name = name[4..];
                }
                else if (name.StartsWith("XP"))
                {
                    name = name[2..];
                }

                var prefixLength = GetItemsPrefixLength(cppEnum);

                var @enum = EnumDeclaration(name)
                    .AddModifiers(Token(SyntaxKind.PublicKeyword))
                    .AddMembers(cppEnum.Items.Select(
                        cppEnumItem => EnumMemberDeclaration(PrettyItemName(cppEnumItem.Name, prefixLength))
                            .WithEqualsValue(EqualsValueClause(
                                LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal((int) cppEnumItem.Value))))
                        ).ToArray());

                if (cppEnum.Items.All(i => IsPowerOf2(i.Value)))
                {
                    @enum = @enum.AddAttributeLists(
                        AttributeList(
                            SingletonSeparatedList(Attribute(IdentifierName("System.Flags")))));
                }

                var ns = NamespaceDeclaration(IdentifierName(@namespace))
                    .WithMembers(SingletonList<MemberDeclarationSyntax>(@enum))
                    .NormalizeWhitespace();

                var filename = $"{name}.cs";
                var path = Path.Combine(OutputProjectDir, filename);

                var projectId = workspace.CurrentSolution.ProjectIds.First();
                var documentInfo = DocumentInfo.Create(
                    DocumentId.CreateNewId(projectId, filename), 
                    filename,
                    loader: TextLoader.From(TextAndVersion.Create(
                        SourceText.From(ns.ToFullString()), VersionStamp.Default, path)),
                    filePath: path);
                var doc = workspace.AddDocument(documentInfo);
                if (doc.TryGetText(out var text))
                {
                    using var writer = new StreamWriter(path, false);
                    text.Write(writer);
                }
            }

            static string GetName(CppEnum cppEnum)
            {
                if (!string.IsNullOrEmpty(cppEnum.Name))
                    return cppEnum.Name;

                var comment = cppEnum.Comment.ToString();
                if (string.IsNullOrEmpty(comment))
                    return $"Enum_{Guid.NewGuid():N}";

                var match = Regex.Match(comment, @"\w+");
                return match.Value;
            }

            static bool IsPowerOf2(long value) => ((value - 1) & value) == 0;

            static string PrettyItemName(string itemName, int prefixLength)
            {
                var parts = itemName[prefixLength..].Split("_");
                return string.Concat(parts.Select(p => p[..1].ToUpperInvariant() + p[1..]));
            }

            static int GetItemsPrefixLength(CppEnum cppEnum)
            {
                if (cppEnum.Items.Count < 2)
                    return 0;

                var item1 = cppEnum.Items[0].Name;
                var item2 = cppEnum.Items[^1].Name;
                var len = Math.Min(item1.Length, item2.Length);
                int prefixLength;
                for (prefixLength = 0; prefixLength < len; prefixLength++)
                {
                    if (item1[prefixLength] != item2[prefixLength])
                        break;
                }

                while (prefixLength > 0)
                {
                    var prefix = item1[..prefixLength];
                    if (cppEnum.Items.All(i => i.Name.StartsWith(prefix)))
                    {
                        return prefixLength;
                    }

                    prefixLength -= 1;
                }

                return 0;
            }
        }

        
    }
}
