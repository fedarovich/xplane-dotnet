using System;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Interop
{
    
    /// <summary>
    /// <para>
    /// An XPLMDataChanged_f is a callback that the XPLM calls whenever any other
    /// plug-in modifies shared data. A refcon you provide is passed back to help
    /// identify which data is being changed. In response, you may want to call one
    /// of the XPLMGetDataxxx routines to find the new value of the data.
    /// </para>
    /// </summary>
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public unsafe delegate void DataChangedCallback(void* inRefcon);
}