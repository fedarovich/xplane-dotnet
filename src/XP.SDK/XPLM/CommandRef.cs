using System;
using System.Runtime.CompilerServices;
using XP.SDK.XPLM.Internal;

namespace XP.SDK.XPLM
{
    public partial struct CommandRef
    {
        /// <summary>
        /// Looks up a command by name, and returns its command
        /// reference or <see langword="default"/> if the command does not exist.
        /// </summary>
        /// <param name="name">The command name.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CommandRef Find(in ReadOnlySpan<char> name) => UtilitiesAPI.FindCommand(name);
    }
}
