using System;

namespace XP.SDK.XPLM
{
    
    /// <summary>
    /// <para>
    /// Indicates whether a map element should be match its rotation to the map
    /// itself, or to the user interface. For instance, the map itself may be
    /// rotated such that "up" matches the user's aircraft, but you may want to
    /// draw a text label such that it is always rotated zero degrees relative to
    /// the user's perspective. In that case, you would have it draw with UI
    /// orientation.
    /// </para>
    /// </summary>
    public enum MapOrientation
    {
        Map = 0,
        UI = 1
    }
}