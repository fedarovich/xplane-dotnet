namespace XP.SDK.XPLM
{
    /// <summary>
    /// Encapsulates event arguments for <see cref="Command.BeforeExecute"/> event.
    /// </summary>
    public struct CommandBeforeExecuteEventArgs
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CommandBeforeExecuteEventArgs"/>.
        /// </summary>
        /// <param name="commandPhase">The command phase.</param>
        public CommandBeforeExecuteEventArgs(CommandPhase commandPhase)
        {
            CommandPhase = commandPhase;
            Handled = false;
        }

        /// <summary>
        /// Gets the command phase.
        /// </summary>
        public readonly CommandPhase CommandPhase;

        /// <summary>
        /// Set to <see langword="true"/> to prevent the command from being handled by X-Plane and other plugins.
        /// </summary>
        public bool Handled;
    }
}
