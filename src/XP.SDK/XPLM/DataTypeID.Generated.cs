using System;

namespace XP.SDK.XPLM
{
    [Flags]
    public enum DataTypeID
    {
        Unknown = 0,
        Int = 1,
        Float = 2,
        Double = 4,
        FloatArray = 8,
        IntArray = 16,
        Data = 32
    }
}