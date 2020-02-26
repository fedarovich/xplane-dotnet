using System;

namespace XP.SDK.XPLM
{
    
    /// <summary>
    /// <para>
    /// XPLMWindowLayer describes where in the ordering of windows X-Plane should
    /// place a particular window. Windows in higher layers cover windows in lower
    /// layers. So, a given window might be at the top of its particular layer, but
    /// it might still be obscured by a window in a higher layer. (This happens
    /// frequently when floating windows, like X-Plane's map, are covered by a
    /// modal alert.)
    /// </para>
    /// <para>
    /// Your window's layer can only be specified when you create the window (in
    /// the XPLMCreateWindow_t you pass to XPLMCreateWindowEx()). For this reason,
    /// layering only applies to windows created with new X-Plane 11 GUI features.
    /// (Windows created using the older XPLMCreateWindow(), or windows compiled
    /// against a pre-XPLM300 version of the SDK will simply be placed in the
    /// flight overlay window layer.)
    /// </para>
    /// </summary>
    public enum WindowLayer
    {
        FlightOverlay = 0,
        FloatingWindows = 1,
        Modal = 2,
        GrowlNotifications = 3
    }
}