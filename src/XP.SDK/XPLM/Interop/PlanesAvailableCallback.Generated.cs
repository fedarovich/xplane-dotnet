using System;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Interop
{
    
    /// <summary>
    /// <para>
    /// Your airplanes available callback is called when another plugin gives up
    /// access to the multiplayer planes.  Use this to wait for access to
    /// multiplayer.
    /// </para>
    /// </summary>
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public unsafe delegate void PlanesAvailableCallback(void* inRefcon);
}