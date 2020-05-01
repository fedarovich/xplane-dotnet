using System;
using System.Collections.Generic;
using System.Text;

namespace XP.Tool
{
    [Flags]
    public enum Platform
    {
        Windows = 1,
        Linux = 2,
        Macos = 4,
        All = 7
    }
}
