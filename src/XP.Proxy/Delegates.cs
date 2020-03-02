using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using XP.SDK.XPLM.Internal;

namespace XP.Proxy
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public delegate void DebugStringDelegate([MarshalAs(UnmanagedType.LPUTF8Str)] string message);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public delegate IntPtr GetPluginPathDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public delegate int StartDelegate(ref StartParameters parameters);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public delegate int EnableDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public delegate void DisableDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public delegate void StopDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public delegate void ReceiveMessage(int pluginId, int message, IntPtr param);
}
