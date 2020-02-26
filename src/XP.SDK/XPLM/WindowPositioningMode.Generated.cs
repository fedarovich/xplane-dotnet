using System;

namespace XP.SDK.XPLM
{
    
    /// <summary>
    /// <para>
    /// XPLMWindowPositionMode describes how X-Plane will position your window on
    /// the user's screen. X-Plane will maintain this positioning mode even as the
    /// user resizes their window or adds/removes full-screen monitors.
    /// </para>
    /// <para>
    /// Positioning mode can only be set for "modern" windows (that is, windows
    /// created using XPLMCreateWindowEx() and compiled against the XPLM300 SDK).
    /// Windows created using the deprecated XPLMCreateWindow(), or windows
    /// compiled against a pre-XPLM300 version of the SDK will simply get the
    /// "free" positioning mode.
    /// </para>
    /// </summary>
    public enum WindowPositioningMode
    {
        PositionFree = 0,
        CenterOnMonitor = 1,
        FullScreenOnMonitor = 2,
        FullScreenOnAllMonitors = 3,
        PopOut = 4,
        VR = 5
    }
}