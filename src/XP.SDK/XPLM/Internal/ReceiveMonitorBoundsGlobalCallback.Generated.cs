using System;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Internal
{
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public unsafe delegate void ReceiveMonitorBoundsGlobalCallback(int inMonitorIndex, int inLeftBx, int inTopBx, int inRightBx, int inBottomBx, void *inRefcon);
}