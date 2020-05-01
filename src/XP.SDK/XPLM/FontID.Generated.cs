using System;

namespace XP.SDK.XPLM
{
    
    /// <summary>
    /// <para>
    /// X-Plane features some fixed-character fonts.  Each font may have its own
    /// metrics.
    /// </para>
    /// <para>
    /// WARNING: Some of these fonts are no longer supported or may have changed
    /// geometries. For maximum copmatibility, see the comments below.
    /// </para>
    /// <para>
    /// Note: X-Plane 7 supports proportional-spaced fonts.  Since no measuring
    /// routine is available yet, the SDK will normally draw using a fixed-width
    /// font.  You can use a dataref to enable proportional font drawing on XP7 if
    /// you want to.
    /// </para>
    /// </summary>
    public enum FontID
    {
        Basic = 0,
        Proportional = 18
    }
}