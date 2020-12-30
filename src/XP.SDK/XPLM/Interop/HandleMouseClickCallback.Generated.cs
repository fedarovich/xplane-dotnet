using System;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Interop
{
    
    /// <summary>
    /// <para>
    /// You receive this call for one of three events:
    /// </para>
    /// <para>
    /// - when the user clicks the mouse button down
    /// - (optionally) when the user drags the mouse after a down-click, but before
    /// the up-click
    /// - when the user releases the down-clicked mouse button.
    /// </para>
    /// <para>
    /// You receive the x and y of the click, your window, and a refcon.  Return 1
    /// to consume the click, or 0 to pass it through.
    /// </para>
    /// <para>
    /// WARNING: passing clicks through windows (as of this writing) causes mouse
    /// tracking problems in X-Plane; do not use this feature!
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
    public unsafe delegate int HandleMouseClickCallback(WindowID inWindowID, int x, int y, MouseStatus inMouse, void* inRefcon);
}