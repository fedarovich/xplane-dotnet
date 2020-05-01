using System;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Internal
{
    
    /// <summary>
    /// <para>
    /// The SDK calls your mouse wheel callback when one of the mouse wheels is
    /// scrolled within your window.  Return 1 to consume the mouse wheel movement
    /// or 0 to pass them on to a lower window.  (If your window appears opaque to
    /// the user, you should consume mouse wheel scrolling even if it does
    /// nothing.)  The number of "clicks" indicates how far the wheel was turned
    /// since the last callback. The wheel is 0 for the vertical axis or 1 for the
    /// horizontal axis (for OS/mouse combinations that support this).
    /// </para>
    /// <para>
    /// The units for x and y values match the units used in your window. Thus, for
    /// "modern" windows (those created via XPLMCreateWindowEx() and compiled
    /// against the XPLM300 library), the units are boxels, while legacy windows
    /// will get pixels. Legacy windows have their origin in the lower left of the
    /// main X-Plane window, while modern windows have their origin in the lower
    /// left of the global desktop space. In both cases, x increases as you move
    /// right, and y increases as you move up.
    /// </para>
    /// </summary>
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public unsafe delegate int HandleMouseWheelCallback(WindowID inWindowID, int x, int y, int wheel, int clicks, void* inRefcon);
}