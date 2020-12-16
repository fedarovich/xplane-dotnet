﻿using System;

namespace XP.SDK.XPLM
{
    [Flags]
    public enum MouseHandlers : byte
    {
        None = 0,
        LeftClick = 1,
        RightClick = 2,

        All = 0xFF
    }
}
