﻿using System;
using System.Collections.Generic;
using System.Text;
using XP.SDK.XPLM.Internal;

namespace XP.SDK.XPLM
{
    /// <summary>
    /// Begins the command after construction and ends it on disposal.
    /// </summary>
    public ref struct CommandExecutionScope
    {
        private CommandRef _commandRef;

        public CommandExecutionScope(CommandRef commandRef)
        {
            _commandRef = commandRef;
            if (_commandRef != default)
            {
                UtilitiesAPI.CommandBegin(_commandRef);
            }
        }

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
