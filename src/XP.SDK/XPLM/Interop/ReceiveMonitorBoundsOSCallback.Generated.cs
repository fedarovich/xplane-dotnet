using System;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Interop
{
    
    /// <summary>
    /// <para>
    /// This function is informed of the global bounds (in pixels) of a particular
    /// monitor within the operating system's global desktop space. Note that a
    /// monitor index being passed to you here does not indicate that X-Plane is
    /// running in full screen on this monitor, or even that any X-Plane windows
    /// exist on this monitor.
    /// </para>
    /// </summary>
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public unsafe delegate void ReceiveMonitorBoundsOSCallback(int inMonitorIndex, int inLeftPx, int inTopPx, int inRightPx, int inBottomPx, void* inRefcon);
}