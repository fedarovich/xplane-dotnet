using System;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Internal
{
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public unsafe delegate void HandleKeyCallback(WindowID inWindowID, byte inKey, KeyFlags inFlags, byte inVirtualKey, void *inRefcon, int losingFocus);
}