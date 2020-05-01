using System;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Internal
{
    
    /// <summary>
    /// <para>
    /// Data provider function pointers.
    /// </para>
    /// <para>
    /// These define the function pointers you provide to get or set data. Note
    /// that you are passed a generic pointer for each one. This is the same
    /// pointer you pass in your register routine; you can use it to locate plugin
    /// variables, etc.
    /// </para>
    /// <para>
    /// The semantics of your callbacks are the same as the dataref accessor above
    /// - basically routines like XPLMGetDatai are just pass-throughs from a caller
    /// to your plugin. Be particularly mindful in implementing array dataref
    /// read-write accessors; you are responsible for avoiding overruns, supporting
    /// offset read/writes, and handling a read with a NULL buffer.
    /// </para>
    /// </summary>
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public unsafe delegate int GetDataiCallback(void* inRefcon);
}