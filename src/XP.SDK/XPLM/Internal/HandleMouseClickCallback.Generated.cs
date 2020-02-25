using System;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Internal
{
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public unsafe delegate int HandleMouseClickCallback(WindowID inWindowID, int x, int y, MouseStatus inMouse, void *inRefcon);
}