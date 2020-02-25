using System;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Internal
{
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public unsafe delegate int HandleMouseWheelCallback(WindowID inWindowID, int x, int y, int wheel, int clicks, void *inRefcon);
}