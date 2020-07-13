using System;
using System.Collections.Generic;
using System.Text;

namespace XP.SDK.XPLM
{
    public struct CommandBeforeExecuteEventArgs
    {
        public CommandBeforeExecuteEventArgs(CommandPhase commandPhase)
        {
            CommandPhase = commandPhase;
            Handled = false;
        }

        public readonly CommandPhase CommandPhase;

        public bool Handled;
    }
}
