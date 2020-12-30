using System;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Interop
{
    
    /// <summary>
    /// <para>
    /// This function is informed of the global bounds (in boxels) of a particular
    /// monitor within the X-Plane global desktop space. Note that X-Plane must be
    /// running in full screen on a monitor in order for that monitor to be passed
    /// to you in this callback.
    /// </para>
    /// </summary>
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public unsafe delegate void ReceiveMonitorBoundsGlobalCallback(int inMonitorIndex, int inLeftBx, int inTopBx, int inRightBx, int inBottomBx, void* inRefcon);
}