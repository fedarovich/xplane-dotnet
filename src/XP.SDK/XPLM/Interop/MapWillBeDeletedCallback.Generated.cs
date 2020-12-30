using System;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Interop
{
    
    /// <summary>
    /// <para>
    /// Called just before your map layer gets deleted. Because SDK-created map
    /// layers have the same lifetime as the X-Plane map that contains them, if the
    /// map gets unloaded from memory, your layer will too.
    /// </para>
    /// </summary>
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public unsafe delegate void MapWillBeDeletedCallback(MapLayerID inLayer, void* inRefcon);
}