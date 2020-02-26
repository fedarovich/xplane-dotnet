using System;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Internal
{
    
    /// <summary>
    /// <para>
    /// This is the icon drawing callback that enables plugin-created map layers to
    /// draw icons using X-Plane's built-in icon drawing functionality. You can
    /// request an arbitrary number of PNG icons to be drawn via
    /// XPLMDrawMapIconFromSheet() from within this callback, but you may not
    /// perform any OpenGL drawing here.
    /// </para>
    /// <para>
    /// Icons enqueued by this function will appear above all OpenGL drawing
    /// (performed by your optional XPLMMapDrawingCallback_f), and above all
    /// built-in X-Plane map icons of the same layer type ("fill" or "markings," as
    /// determined by the XPLMMapLayerType in your XPLMCreateMapLayer_t). Note,
    /// however, that the relative ordering between the drawing callbacks of
    /// different plugins is not guaranteed.
    /// </para>
    /// </summary>
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public unsafe delegate void MapIconDrawingCallback(MapLayerID inLayer, float* inMapBoundsLeftTopRightBottom, float zoomRatio, float mapUnitsPerUserInterfaceUnit, MapStyle mapStyle, MapProjectionID projection, void* inRefcon);
}