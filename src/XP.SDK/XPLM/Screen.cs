using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using XP.SDK.XPLM.Interop;

namespace XP.SDK.XPLM
{
    /// <summary>
    /// Provides information about the screen.
    /// </summary>
    public static class Screen
    {
        /// <summary>
        /// This property returns the bounds of the "global" X-Plane desktop, in boxels.
        /// It is multi-monitor aware. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// There are three primary consequences of multimonitor awareness.
        /// </para>
        /// <para>
        /// First, if the user is running X-Plane in full-screen on two or more
        /// monitors (typically configured using one full-screen window per monitor),
        /// the global desktop will be sized to include all X-Plane windows.
        /// </para>
        /// <para>
        /// Second, the origin of the screen coordinates is not guaranteed to be (0,
        /// 0). Suppose the user has two displays side-by-side, both running at 1080p.
        /// Suppose further that they've configured their OS to make the left display
        /// their "primary" monitor, and that X-Plane is running in full-screen on
        /// their right monitor only. In this case, the global desktop bounds would be
        /// the rectangle from (1920, 0) to (3840, 1080). If the user later asked
        /// X-Plane to draw on their primary monitor as well, the bounds would change
        /// to (0, 0) to (3840, 1080).
        /// </para>
        /// <para>
        /// Finally, if the usable area of the virtual desktop is not a perfect
        /// rectangle (for instance, because the monitors have different resolutions or
        /// because one monitor is configured in the operating system to be above and
        /// to the right of the other), the global desktop will include any wasted
        /// space. Thus, if you have two 1080p monitors, and monitor 2 is configured to
        /// have its bottom left touch monitor 1's upper right, your global desktop
        /// area would be the rectangle from (0, 0) to (3840, 2160).
        /// </para>
        /// <para>
        /// Note that popped-out windows (windows drawn in their own operating system
        /// windows, rather than "floating" within X-Plane) are not included in these
        /// bounds.
        /// </para>
        /// </remarks>
        public static unsafe Rect BoundsGlobal
        {
            get
            {
                int left, top, right, bottom;
                DisplayAPI.GetScreenBoundsGlobal(&left, &top, &right, &bottom);
                return new Rect(left, top, right, bottom);
            }
        }

        /// <summary>
        /// Gets the bounds (in boxels) of all full-screen X-Plane windows within the X-Plane global desktop space.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Note  that if a monitor is *not* covered by an X-Plane window, you cannot get its
        /// bounds this way. Likewise, monitors with only an X-Plane window (not in
        /// full-screen mode) will not be included.
        /// </para>
        /// <para>
        /// If X-Plane is running in full-screen and your monitors are of the same size
        /// and configured contiguously in the OS, then the combined global bounds of
        /// all full-screen monitors will match the total global desktop bounds, as
        /// returned by <see cref="BoundsGlobal"/>. (Of course, if X-Plane is running
        /// in windowed mode, this will not be the case. Likewise, if you have
        /// differently sized monitors, the global desktop space will include wasted
        /// space.)
        /// </para>
        /// <para>
        /// Note that this function's monitor indices match those provided by
        /// <see cref="AllMonitorBoundsOS"/>, but the coordinates are different (since the
        /// X-Plane global desktop may not match the operating system's global desktop,
        /// and one X-Plane boxel may be larger than one pixel due to 150% or 200%
        /// scaling).
        /// </para>
        /// </remarks>
        public static unsafe IReadOnlyDictionary<int, Rect> AllMonitorBoundsGlobal
        {
            get
            {
                var dict = new Dictionary<int, Rect>();
                var dictHandle = GCHandle.Alloc(dict);
                try
                {
                    DisplayAPI.GetAllMonitorBoundsGlobal(&Callback, GCHandle.ToIntPtr(dictHandle).ToPointer());
                    return dict;
                }
                finally
                {
                    dictHandle.Free();
                }

                [UnmanagedCallersOnly]
                static void Callback(int index, int left, int top, int right, int bottom, void* inrefcon)
                {
                    var dict = (Dictionary<int, Rect>)GCHandle.FromIntPtr(new IntPtr(inrefcon)).Target;
                    dict.Add(index, new Rect(left, top, right, bottom));
                }
            }
        }

        /// <summary>
        /// <para>
        /// This property returns the bounds (in pixels) of each
        /// monitor within the operating system's global desktop space. Note that
        /// unlike <see cref="AllMonitorBoundsGlobal"/>, this may include monitors that have
        /// no X-Plane window on them.
        /// </para>
        /// </summary>
        /// <remarks>
        /// <para>
        /// Note that this function's monitor indices match those provided by
        /// <see cref="AllMonitorBoundsGlobal"/>, but the coordinates are different (since
        /// the X-Plane global desktop may not match the operating system's global
        /// desktop, and one X-Plane boxel may be larger than one pixel).
        /// </para>
        /// </remarks>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public static unsafe IReadOnlyDictionary<int, Rect> AllMonitorBoundsOS
        {
            get
            {
                var dict = new Dictionary<int, Rect>();
                var dictHandle = GCHandle.Alloc(dict);
                try
                {
                    DisplayAPI.GetAllMonitorBoundsOS(&Callback, GCHandle.ToIntPtr(dictHandle).ToPointer());
                    return dict;
                }
                finally
                {
                    dictHandle.Free();
                }

                [UnmanagedCallersOnly]
                static void Callback(int index, int left, int top, int right, int bottom, void* inrefcon)
                {
                    var dict = (Dictionary<int, Rect>)GCHandle.FromIntPtr(new IntPtr(inrefcon)).Target;
                    dict.Add(index, new Rect(left, top, right, bottom));
                }
            }
        }

        /// <summary>
        /// Returns the current mouse location in global desktop boxels.
        /// </summary>
        /// <remarks>
        /// The bottom left of the main X-Plane window is not
        /// guaranteed to be (0, 0). Instead, the origin is the lower left of the
        /// entire global desktop space. In addition, this routine gives the real mouse
        /// location when the mouse goes to X-Plane windows other than the primary
        /// display. Thus, it can be used with both pop-out windows and secondary
        /// </remarks>
        public static unsafe (int X, int Y) MouseLocationGlobal
        {
            get
            {
                int x, y;
                DisplayAPI.GetMouseLocationGlobal(&x, &y);
                return (x, y);
            }
        }
    }
}
