using System;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Interop
{
    
    /// <summary>
    /// <para>
    /// An XPLM error callback is a function that you provide to receive debugging
    /// information from the plugin SDK. See XPLMSetErrorCallback for more
    /// information. NOTE: for the sake of debugging, your error callback will be
    /// called even if your plugin is not enabled, allowing you to receive debug
    /// info in your XPluginStart and XPluginStop callbacks. To avoid causing logic
    /// errors in the management code, do not call any other plugin routines from
    /// your error callback - it is only meant for catching errors in the
    /// debugging.
    /// </para>
    /// </summary>
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public unsafe delegate void ErrorCallback(byte* inMessage);
}