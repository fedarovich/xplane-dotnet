using System;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Interop
{
    
    /// <summary>
    /// </summary>
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public unsafe delegate int GetDataviCallback(void* inRefcon, int* outValues, int inOffset, int inMax);
}