using System;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Internal
{
    
    /// <summary>
    /// </summary>
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public unsafe delegate float GetDatafCallback(void* inRefcon);
}