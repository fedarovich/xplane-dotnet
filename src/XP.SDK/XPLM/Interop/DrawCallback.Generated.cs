using System;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Interop
{
    
    /// <summary>
    /// <para>
    /// This is the prototype for a low level drawing callback.  You are passed in
    /// the phase and whether it is before or after.  If you are before the phase,
    /// return 1 to let X-Plane draw or 0 to suppress X-Plane drawing.  If you are
    /// after the phase the return value is ignored.
    /// </para>
    /// <para>
    /// Refcon is a unique value that you specify when registering the callback,
    /// allowing you to slip a pointer to your own data to the callback.
    /// </para>
    /// <para>
    /// Upon entry the OpenGL context will be correctly set up for you and OpenGL
    /// will be in 'local' coordinates for 3d drawing and panel coordinates for 2d
    /// drawing.  The OpenGL state (texturing, etc.) will be unknown.
    /// </para>
    /// </summary>
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public unsafe delegate int DrawCallback(DrawingPhase inPhase, int inIsBefore, void* inRefcon);
}