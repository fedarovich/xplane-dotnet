using System;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Internal
{
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public unsafe delegate void MapPrepareCacheCallback(MapLayerID inLayer, float *inTotalMapBoundsLeftTopRightBottom, MapProjectionID projection, void *inRefcon);
}