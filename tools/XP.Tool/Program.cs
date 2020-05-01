using System;
using McMaster.Extensions.CommandLineUtils;
using XP.Tool.Commands;

namespace XP.Tool
{
    [Subcommand(typeof(Publish))]
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                return CommandLineApplication.Execute<Program>(args);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
                return 1;
            }
        }

        public void OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();
        }
    }
}
