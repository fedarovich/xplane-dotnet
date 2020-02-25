using System;

namespace XP.SDK.XPLM
{
    [Flags]
    public enum KeyFlags
    {
        ShiftFlag = 1,
        OptionAltFlag = 2,
        ControlFlag = 4,
        DownFlag = 8,
        UpFlag = 16
    }
}