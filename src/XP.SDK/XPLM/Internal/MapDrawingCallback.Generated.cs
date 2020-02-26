using System;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Internal
{
    
    /// <summary>
    /// <para>
    /// This is the OpenGL map drawing callback for plugin-created map layers. You
    /// can perform arbitrary OpenGL drawing from this callback, with one
    /// exception: changes to the Z-buffer are not permitted, and will result in
    /// map drawing errors.
    /// </para>
    /// <para>
    /// All drawing done from within this callback appears beneath all built-in
    /// X-Plane icons and labels, but above the built-in "fill" layers (layers
    /// providing major details, like terrain and water). Note, however, that the
    /// relative ordering between the drawing callbacks of different plugins is not
    /// guaranteed.
    /// </para>
    /// </summary>
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public unsafe delegate void MapDrawingCallback(MapLayerID inLayer, float* inMapBoundsLeftTopRightBottom, float zoomRatio, float mapUnitsPerUserInterfaceUnit, MapStyle mapStyle, MapProjectionID projection, void* inRefcon);
}