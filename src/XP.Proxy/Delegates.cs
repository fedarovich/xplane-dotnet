﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace XP.Proxy
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    internal delegate void DebugStringDelegate([MarshalAs(UnmanagedType.LPUTF8Str)] string message);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    internal delegate void GetPluginPathDelegate([MarshalAs(UnmanagedType.LPUTF8Str)] StringBuilder str, int n);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    internal delegate int StartDelegate(ref StartParameters parameters);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    internal delegate int EnableDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    internal delegate void DisableDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    internal delegate void StopDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    internal delegate void ReceiveMessageDelegate(int pluginId, int message, IntPtr param);
}
