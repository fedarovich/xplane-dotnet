using System;

namespace XP.SDK.XPLM
{
    
    /// <summary>
    /// <para>
    /// Indicates the visual style being drawn by the map. In X-Plane, the user can
    /// choose between a number of map types, and different map types may have use
    /// a different visual representation for the same elements (for instance, the
    /// visual style of the terrain layer changes drastically between the VFR and
    /// IFR layers), or certain layers may be disabled entirely in some map types
    /// (e.g., localizers are only visible in the IFR low-enroute style).
    /// </para>
    /// </summary>
    public enum MapStyle
    {
        VFRSectional = 0,
        IFRLowEnroute = 1,
        IFRHighEnroute = 2
    }
}