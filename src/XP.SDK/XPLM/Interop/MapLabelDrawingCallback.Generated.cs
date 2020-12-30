using System;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Interop
{
    
    /// <summary>
    /// <para>
    /// This is the label drawing callback that enables plugin-created map layers
    /// to draw text labels using X-Plane's built-in labeling functionality. You
    /// can request an arbitrary number of text labels to be drawn via
    /// XPLMDrawMapLabel() from within this callback, but you may not perform any
    /// OpenGL drawing here.
    /// </para>
    /// <para>
    /// Labels enqueued by this function will appear above all OpenGL drawing
    /// (performed by your optional XPLMMapDrawingCallback_f), and above all
    /// built-in map icons and labels of the same layer type ("fill" or "markings,"
    /// as determined by the XPLMMapLayerType in your XPLMCreateMapLayer_t). Note,
    /// however, that the relative ordering between the drawing callbacks of
    /// different plugins is not guaranteed.
    /// </para>
    /// </summary>
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public unsafe delegate void MapLabelDrawingCallback(MapLayerID inLayer, float* inMapBoundsLeftTopRightBottom, float zoomRatio, float mapUnitsPerUserInterfaceUnit, MapStyle mapStyle, MapProjectionID projection, void* inRefcon);
}