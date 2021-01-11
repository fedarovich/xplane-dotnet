namespace XP.SDK.XPLM
{
    /// <summary>
    /// Encapsulates arguments for <see cref="Command.AfterExecute"/> event.
    /// </summary>
    public readonly struct CommandAfterExecuteEventArgs
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CommandAfterExecuteEventArgs"/>.
        /// </summary>
        /// <param name="commandPhase">The command phase.</param>
        public CommandAfterExecuteEventArgs(CommandPhase commandPhase)
        {
            CommandPhase = commandPhase;
        }

        /// <summary>
        /// Gets the command phase.
        /// </summary>
        public readonly CommandPhase CommandPhase;
    }
}
