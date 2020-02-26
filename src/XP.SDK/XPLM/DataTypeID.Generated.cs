using System;

namespace XP.SDK.XPLM
{
    
    /// <summary>
    /// <para>
    /// This is an enumeration that defines the type of the data behind a data
    /// reference. This allows you to sanity check that the data type matches what
    /// you expect. But for the most part, you will know the type of data you are
    /// expecting from the online documentation.
    /// </para>
    /// <para>
    /// Data types each take a bit field, so sets of data types may be formed.
    /// </para>
    /// </summary>
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