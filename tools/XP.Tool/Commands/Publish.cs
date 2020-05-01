using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using McMaster.Extensions.CommandLineUtils;

namespace XP.Tool.Commands
{
    [Command("publish")]
    public class Publish
    {
        [Argument(0, "PROJECT", "The project or solution file to operate on. If a file is not specified, the command will search the current directory for one.")]
        [LegalFilePath]
        public string Project { get; set; }

        [Option("-o|--output <OUTPUT_DIR>", "The output directory to place the published artifacts in.", CommandOptionType.SingleValue)]
        [LegalFilePath]
        [Required]
        public string OutputDir { get; set; }

        [Option("-c|--configuration <CONFIGURATION>", "The configuration to publish for. The default for most projects is 'Debug'.", CommandOptionType.SingleValue)]
        public string Configuration { get; set; }

        [Option("-v|--version <VERSION>", "The plugin version.", CommandOptionType.SingleValue)]
        public Version Version { get; set; } = new Version(1, 0, 0);

        [Option("-p|--platform <PLATFORM>", "The plugin platforms.", CommandOptionType.SingleValue)]
        public Platform Platform { get; set; } = Platform.All;

        public void OnExecute()
        {
            var platforms = new Dictionary<Platform, (string prefix, string rid)>
            {
                [Platform.Windows] = ("win_x64", "win-x64"),
                [Platform.Linux] = ("lin_x64", "linux-x64"),
                [Platform.Macos] = ("mac_x64", "osx-x64")
            };

            foreach (var (platform, (prefix, rid)) in platforms)
            {
                if (!Platform.HasFlag(platform))
                    continue;

                string args = "publish ";
                if (Project != null)
                {
                    args += $" \"{Project}\"";
                }

                args += $" -o \"{Path.Combine(OutputDir, prefix)}\"";
                args += $" -r \"{rid}\"";
                args += $" -p:Version={Version.ToString(3)}";

                if (Configuration != null)
                {
                    args += $" -c \"{Configuration}\"";
                }

                var process = Process.Start("dotnet", args);
                process.WaitForExit();
            }
        }
    }
}
