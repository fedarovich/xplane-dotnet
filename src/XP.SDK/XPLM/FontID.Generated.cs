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
        Menus = 1,
        Metal = 2,
        Led = 3,
        LedWide = 4,
        PanelHUD = 5,
        PanelEFIS = 6,
        PanelGPS = 7,
        RadiosGA = 8,
        RadiosBC = 9,
        RadiosHM = 10,
        RadiosGANarrow = 11,
        RadiosBCNarrow = 12,
        RadiosHMNarrow = 13,
        Timer = 14,
        FullRound = 15,
        SmallRound = 16,
        MenusLocalized = 17,
        Proportional = 18
    }
}