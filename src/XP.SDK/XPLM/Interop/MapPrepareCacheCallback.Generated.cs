using System;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Interop
{
    
    /// <summary>
    /// <para>
    /// A callback used to allow you to cache whatever information your layer needs
    /// to draw in the current map area.
    /// </para>
    /// <para>
    /// This is called each time the map's total bounds change. This is typically
    /// triggered by new DSFs being loaded, such that X-Plane discards old,
    /// now-distant DSFs and pulls in new ones. At that point, the available bounds
    /// of the map also change to match the new DSF area.
    /// </para>
    /// <para>
    /// By caching just the information you need to draw in this area, your future
    /// draw calls can be made faster, since you'll be able to simply "splat" your
    /// precomputed information each frame.
    /// </para>
    /// <para>
    /// We guarantee that the map projection will not change between successive
    /// prepare cache calls, nor will any draw call give you bounds outside these
    /// total map bounds. So, if you cache the projected map coordinates of all the
    /// items you might want to draw in the total map area, you can be guaranteed
    /// that no draw call will be asked to do any new work.
    /// </para>
    /// </summary>
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public unsafe delegate void MapPrepareCacheCallback(MapLayerID inLayer, float* inTotalMapBoundsLeftTopRightBottom, MapProjectionID projection, void* inRefcon);
}