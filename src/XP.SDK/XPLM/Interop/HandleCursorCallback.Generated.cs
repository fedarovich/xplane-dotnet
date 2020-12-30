using System;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Interop
{
    
    /// <summary>
    /// <para>
    /// The SDK calls your cursor status callback when the mouse is over your
    /// plugin window.  Return a cursor status code to indicate how you would like
    /// X-Plane to manage the cursor.  If you return xplm_CursorDefault, the SDK
    /// will try lower-Z-order plugin windows, then let the sim manage the cursor.
    /// </para>
    /// <para>
    /// Note: you should never show or hide the cursor yourself---these APIs are
    /// typically reference-counted and thus cannot safely and predictably be used
    /// by the SDK.  Instead return one of xplm_CursorHidden to hide the cursor or
    /// xplm_CursorArrow/xplm_CursorCustom to show the cursor.
    /// </para>
    /// <para>
    /// If you want to implement a custom cursor by drawing a cursor in OpenGL, use
    /// xplm_CursorHidden to hide the OS cursor and draw the cursor using a 2-d
    /// drawing callback (after xplm_Phase_Window is probably a good choice, but
    /// see deprecation warnings on the drawing APIs!).  If you want to use a
    /// custom OS-based cursor, use xplm_CursorCustom to ask X-Plane to show the
    /// cursor but not affect its image.  You can then use an OS specific call like
    /// SetThemeCursor (Mac) or SetCursor/LoadCursor (Windows).
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
    public unsafe delegate CursorStatus HandleCursorCallback(WindowID inWindowID, int x, int y, void* inRefcon);
}