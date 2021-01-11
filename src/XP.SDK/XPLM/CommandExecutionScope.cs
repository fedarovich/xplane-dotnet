using XP.SDK.XPLM.Interop;

namespace XP.SDK.XPLM
{
    /// <summary>
    /// Begins the command after construction and ends it on disposal.
    /// </summary>
    public ref struct CommandExecutionScope
    {
        private CommandRef _commandRef;

        /// <summary>
        /// Begins the command with the specified <paramref name="commandRef"/>.
        /// </summary>
        public CommandExecutionScope(CommandRef commandRef)
        {
            _commandRef = commandRef;
            if (_commandRef != default)
            {
                UtilitiesAPI.CommandBegin(_commandRef);
            }
        }

        /// <summary>
        /// Ends the command.
        /// </summary>
        public void Dispose()
        {
            if (_commandRef != default)
            {
                UtilitiesAPI.CommandEnd(_commandRef);
                _commandRef = default;
            }
        }
    }
}
