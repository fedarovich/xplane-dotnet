using System;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Internal
{
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public unsafe delegate void MapLabelDrawingCallback(MapLayerID inLayer, float *inMapBoundsLeftTopRightBottom, float zoomRatio, float mapUnitsPerUserInterfaceUnit, MapStyle mapStyle, MapProjectionID projection, void *inRefcon);
}