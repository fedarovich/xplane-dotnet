namespace XP.SDK.XPLM
{
    public readonly struct CommandAfterExecuteEventArgs
    {
        public CommandAfterExecuteEventArgs(CommandPhase commandPhase)
        {
            CommandPhase = commandPhase;
        }

        public readonly CommandPhase CommandPhase;
    }
}
