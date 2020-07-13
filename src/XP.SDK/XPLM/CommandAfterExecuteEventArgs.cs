using System;
using System.Collections.Generic;
using System.Text;

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
