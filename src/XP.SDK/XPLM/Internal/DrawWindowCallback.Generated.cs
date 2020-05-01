using System;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Internal
{
    
    /// <summary>
    /// <para>
    /// A callback to handle 2-D drawing of your window.  You are passed in your
    /// window and its refcon. Draw the window.  You can use other XPLM functions
    /// from this header to find the current dimensions of your window, etc.  When
    /// this callback is called, the OpenGL context will be set properly for 2-D
    /// window drawing.
    /// </para>
    /// <para>
    /// **Note**: Because you are drawing your window over a background, you can
    /// make a translucent window easily by simply not filling in your entire
    /// window's bounds.
    /// </para>
    /// </summary>
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public unsafe delegate void DrawWindowCallback(WindowID inWindowID, void* inRefcon);
}