using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using InlineIL;

namespace XP.SDK.XPLM.Internal
{
    public static partial class DisplayAPI
    {
        
        /// <summary>
        /// <para>
        /// This routine registers a low level drawing callback.  Pass in the phase you
        /// want to be called for and whether you want to be called before or after.
        /// This routine returns 1 if the registration was successful, or 0 if the
        /// phase does not exist in this version of X-Plane.  You may register a
        /// callback multiple times for the same or different phases as long as the
        /// refcon is unique each time.
        /// </para>
        /// <para>
        /// Note that this function will likely be removed during the X-Plane 11 run as
        /// part of the transition to Vulkan/Metal/etc. See the XPLMInstance API for
        /// future-proof drawing of 3-D objects.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMRegisterDrawCallback", ExactSpelling = true)]
        public static extern unsafe int RegisterDrawCallback(delegate* unmanaged[Cdecl]<DrawingPhase, int, void*, int> inCallback, DrawingPhase inPhase, int inWantsBefore, void* inRefcon);

        
        /// <summary>
        /// <para>
        /// This routine unregisters a draw callback.  You must unregister a callback
        /// for each time you register a callback if you have registered it multiple
        /// times with different refcons.  The routine returns 1 if it can find the
        /// callback to unregister, 0 otherwise.
        /// </para>
        /// <para>
        /// Note that this function will likely be removed during the X-Plane 11 run as
        /// part of the transition to Vulkan/Metal/etc. See the XPLMInstance API for
        /// future-proof drawing of 3-D objects.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMUnregisterDrawCallback", ExactSpelling = true)]
        public static extern unsafe int UnregisterDrawCallback(delegate* unmanaged[Cdecl]<DrawingPhase, int, void*, int> inCallback, DrawingPhase inPhase, int inWantsBefore, void* inRefcon);

        
        /// <summary>
        /// <para>
        /// This routine creates a new "modern" window. You pass in an
        /// XPLMCreateWindow_t structure with all of the fields set in.  You must set
        /// the structSize of the structure to the size of the actual structure you
        /// used.  Also, you must provide functions for every callback---you may not
        /// leave them null!  (If you do not support the cursor or mouse wheel, use
        /// functions that return the default values.)
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMCreateWindowEx", ExactSpelling = true)]
        public static extern unsafe WindowID CreateWindowEx(CreateWindow* inParams);

        
        /// <summary>
        /// <para>
        /// Deprecated as of XPLM300.
        /// </para>
        /// <para>
        /// This routine creates a new legacy window. Unlike modern windows (created
        /// via XPLMCreateWindowEx()), legacy windows do not have access to X-Plane 11
        /// features like automatic scaling for high-DPI screens, native window styles,
        /// or support for being "popped out" into first-class operating system
        /// windows.
        /// </para>
        /// <para>
        /// Pass in the dimensions and offsets to the window's bottom left corner from
        /// the bottom left of the screen.  You can specify whether the window is
        /// initially visible or not.  Also, you pass in three callbacks to run the
        /// window and a refcon.  This function returns a window ID you can use to
        /// refer to the new window.
        /// </para>
        /// <para>
        /// NOTE: Legacy windows do not have "frames"; you are responsible for drawing
        /// the background and frame of the window.  Higher level libraries have
        /// routines which make this easy.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMCreateWindow", ExactSpelling = true)]
        public static extern unsafe WindowID CreateWindow(int inLeft, int inTop, int inRight, int inBottom, int inIsVisible, delegate* unmanaged[Cdecl]<WindowID, void*, void> inDrawCallback, delegate* unmanaged[Cdecl]<WindowID, byte, KeyFlags, byte, void*, int, void> inKeyCallback, delegate* unmanaged[Cdecl]<WindowID, int, int, MouseStatus, void*, int> inMouseCallback, void* inRefcon);

        
        /// <summary>
        /// <para>
        /// This routine destroys a window.  The window's callbacks are not called
        /// after this call. Keyboard focus is removed from the window before
        /// destroying it.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMDestroyWindow", ExactSpelling = true)]
        public static extern void DestroyWindow(WindowID inWindowID);

        
        /// <summary>
        /// <para>
        /// This routine returns the size of the main X-Plane OpenGL window in pixels.
        /// This number can be used to get a rough idea of the amount of detail the
        /// user will be able to see when drawing in 3-d.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMGetScreenSize", ExactSpelling = true)]
        public static extern unsafe void GetScreenSize(int* outWidth, int* outHeight);

        
        /// <summary>
        /// <para>
        /// This routine returns the bounds of the "global" X-Plane desktop, in boxels.
        /// Unlike the non-global version XPLMGetScreenSize(), this is multi-monitor
        /// aware. There are three primary consequences of multimonitor awareness.
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
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMGetScreenBoundsGlobal", ExactSpelling = true)]
        public static extern unsafe void GetScreenBoundsGlobal(int* outLeft, int* outTop, int* outRight, int* outBottom);

        
        /// <summary>
        /// <para>
        /// This routine immediately calls you back with the bounds (in boxels) of each
        /// full-screen X-Plane window within the X-Plane global desktop space. Note
        /// that if a monitor is *not* covered by an X-Plane window, you cannot get its
        /// bounds this way. Likewise, monitors with only an X-Plane window (not in
        /// full-screen mode) will not be included.
        /// </para>
        /// <para>
        /// If X-Plane is running in full-screen and your monitors are of the same size
        /// and configured contiguously in the OS, then the combined global bounds of
        /// all full-screen monitors will match the total global desktop bounds, as
        /// returned by XPLMGetScreenBoundsGlobal(). (Of course, if X-Plane is running
        /// in windowed mode, this will not be the case. Likewise, if you have
        /// differently sized monitors, the global desktop space will include wasted
        /// space.)
        /// </para>
        /// <para>
        /// Note that this function's monitor indices match those provided by
        /// XPLMGetAllMonitorBoundsOS(), but the coordinates are different (since the
        /// X-Plane global desktop may not match the operating system's global desktop,
        /// and one X-Plane boxel may be larger than one pixel due to 150% or 200%
        /// scaling).
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMGetAllMonitorBoundsGlobal", ExactSpelling = true)]
        public static extern unsafe void GetAllMonitorBoundsGlobal(delegate* unmanaged[Cdecl]<int, int, int, int, int, void*, void> inMonitorBoundsCallback, void* inRefcon);

        
        /// <summary>
        /// <para>
        /// This routine immediately calls you back with the bounds (in pixels) of each
        /// monitor within the operating system's global desktop space. Note that
        /// unlike XPLMGetAllMonitorBoundsGlobal(), this may include monitors that have
        /// no X-Plane window on them.
        /// </para>
        /// <para>
        /// Note that this function's monitor indices match those provided by
        /// XPLMGetAllMonitorBoundsGlobal(), but the coordinates are different (since
        /// the X-Plane global desktop may not match the operating system's global
        /// desktop, and one X-Plane boxel may be larger than one pixel).
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMGetAllMonitorBoundsOS", ExactSpelling = true)]
        public static extern unsafe void GetAllMonitorBoundsOS(delegate* unmanaged[Cdecl]<int, int, int, int, int, void*, void> inMonitorBoundsCallback, void* inRefcon);

        
        /// <summary>
        /// <para>
        /// Deprecated in XPLM300. Modern windows should use
        /// XPLMGetMouseLocationGlobal() instead.
        /// </para>
        /// <para>
        /// This routine returns the current mouse location in pixels relative to the
        /// main X-Plane window. The bottom left corner of the main window is (0, 0).
        /// Pass NULL to not receive info about either parameter.
        /// </para>
        /// <para>
        /// Because this function gives the mouse position relative to the main X-Plane
        /// window (rather than in global bounds), this function should only be used by
        /// legacy windows. Modern windows should instead get the mouse position in
        /// global desktop coordinates using XPLMGetMouseLocationGlobal().
        /// </para>
        /// <para>
        /// Note that unlike XPLMGetMouseLocationGlobal(), if the mouse goes outside
        /// the user's main monitor (for instance, to a pop out window or a secondary
        /// monitor), this function will not reflect it.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMGetMouseLocation", ExactSpelling = true)]
        public static extern unsafe void GetMouseLocation(int* outX, int* outY);

        
        /// <summary>
        /// <para>
        /// Returns the current mouse location in global desktop boxels. Unlike
        /// XPLMGetMouseLocation(), the bottom left of the main X-Plane window is not
        /// guaranteed to be (0, 0)---instead, the origin is the lower left of the
        /// entire global desktop space. In addition, this routine gives the real mouse
        /// location when the mouse goes to X-Plane windows other than the primary
        /// display. Thus, it can be used with both pop-out windows and secondary
        /// monitors.
        /// </para>
        /// <para>
        /// This is the mouse location function to use with modern windows (i.e., those
        /// created by XPLMCreateWindowEx()).
        /// </para>
        /// <para>
        /// Pass NULL to not receive info about either parameter.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMGetMouseLocationGlobal", ExactSpelling = true)]
        public static extern unsafe void GetMouseLocationGlobal(int* outX, int* outY);

        
        /// <summary>
        /// <para>
        /// This routine returns the position and size of a window. The units and
        /// coordinate system vary depending on the type of window you have.
        /// </para>
        /// <para>
        /// If this is a legacy window (one compiled against a pre-XPLM300 version of
        /// the SDK, or an XPLM300 window that was not created using
        /// XPLMCreateWindowEx()), the units are pixels relative to the main X-Plane
        /// display.
        /// </para>
        /// <para>
        /// If, on the other hand, this is a new X-Plane 11-style window (compiled
        /// against the XPLM300 SDK and created using XPLMCreateWindowEx()), the units
        /// are global desktop boxels.
        /// </para>
        /// <para>
        /// Pass NULL to not receive any paramter.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMGetWindowGeometry", ExactSpelling = true)]
        public static extern unsafe void GetWindowGeometry(WindowID inWindowID, int* outLeft, int* outTop, int* outRight, int* outBottom);

        
        /// <summary>
        /// <para>
        /// This routine allows you to set the position and size of a window.
        /// </para>
        /// <para>
        /// The units and coordinate system match those of XPLMGetWindowGeometry().
        /// That is, modern windows use global desktop boxel coordinates, while legacy
        /// windows use pixels relative to the main X-Plane display.
        /// </para>
        /// <para>
        /// Note that this only applies to "floating" windows (that is, windows that
        /// are drawn within the X-Plane simulation windows, rather than being "popped
        /// out" into their own first-class operating system windows). To set the
        /// position of windows whose positioning mode is xplm_WindowPopOut, you'll
        /// need to instead use XPLMSetWindowGeometryOS().
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMSetWindowGeometry", ExactSpelling = true)]
        public static extern void SetWindowGeometry(WindowID inWindowID, int inLeft, int inTop, int inRight, int inBottom);

        
        /// <summary>
        /// <para>
        /// This routine returns the position and size of a "popped out" window (i.e.,
        /// a window whose positioning mode is xplm_WindowPopOut), in operating system
        /// pixels.  Pass NULL to not receive any parameter.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMGetWindowGeometryOS", ExactSpelling = true)]
        public static extern unsafe void GetWindowGeometryOS(WindowID inWindowID, int* outLeft, int* outTop, int* outRight, int* outBottom);

        
        /// <summary>
        /// <para>
        /// This routine allows you to set the position and size, in operating system
        /// pixel coordinates, of a popped out window (that is, a window whose
        /// positioning mode is xplm_WindowPopOut, which exists outside the X-Plane
        /// simulation window, in its own first-class operating system window).
        /// </para>
        /// <para>
        /// Note that you are responsible for ensuring both that your window is popped
        /// out (using XPLMWindowIsPoppedOut()) and that a monitor really exists at the
        /// OS coordinates you provide (using XPLMGetAllMonitorBoundsOS()).
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMSetWindowGeometryOS", ExactSpelling = true)]
        public static extern void SetWindowGeometryOS(WindowID inWindowID, int inLeft, int inTop, int inRight, int inBottom);

        
        /// <summary>
        /// <para>
        /// Returns the width and height, in boxels, of a window in VR. Note that you
        /// are responsible for ensuring your window is in VR (using
        /// XPLMWindowIsInVR()).
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMGetWindowGeometryVR", ExactSpelling = true)]
        public static extern unsafe void GetWindowGeometryVR(WindowID inWindowID, int* outWidthBoxels, int* outHeightBoxels);

        
        /// <summary>
        /// <para>
        /// This routine allows you to set the size, in boxels, of a window in VR (that
        /// is, a window whose positioning mode is xplm_WindowVR).
        /// </para>
        /// <para>
        /// Note that you are responsible for ensuring your window is in VR (using
        /// XPLMWindowIsInVR()).
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMSetWindowGeometryVR", ExactSpelling = true)]
        public static extern void SetWindowGeometryVR(WindowID inWindowID, int widthBoxels, int heightBoxels);

        
        /// <summary>
        /// <para>
        /// Returns true (1) if the specified window is visible.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMGetWindowIsVisible", ExactSpelling = true)]
        public static extern int GetWindowIsVisible(WindowID inWindowID);

        
        /// <summary>
        /// <para>
        /// This routine shows or hides a window.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMSetWindowIsVisible", ExactSpelling = true)]
        public static extern void SetWindowIsVisible(WindowID inWindowID, int inIsVisible);

        
        /// <summary>
        /// <para>
        /// True if this window has been popped out (making it a first-class window in
        /// the operating system), which in turn is true if and only if you have set
        /// the window's positioning mode to xplm_WindowPopOut.
        /// </para>
        /// <para>
        /// Only applies to modern windows. (Windows created using the deprecated
        /// XPLMCreateWindow(), or windows compiled against a pre-XPLM300 version of
        /// the SDK cannot be popped out.)
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMWindowIsPoppedOut", ExactSpelling = true)]
        public static extern int WindowIsPoppedOut(WindowID inWindowID);

        
        /// <summary>
        /// <para>
        /// True if this window has been moved to the virtual reality (VR) headset,
        /// which in turn is true if and only if you have set the window's positioning
        /// mode to xplm_WindowVR.
        /// </para>
        /// <para>
        /// Only applies to modern windows. (Windows created using the deprecated
        /// XPLMCreateWindow(), or windows compiled against a pre-XPLM301 version of
        /// the SDK cannot be moved to VR.)
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMWindowIsInVR", ExactSpelling = true)]
        public static extern int WindowIsInVR(WindowID inWindowID);

        
        /// <summary>
        /// <para>
        /// A window's "gravity" controls how the window shifts as the whole X-Plane
        /// window resizes. A gravity of 1 means the window maintains its positioning
        /// relative to the right or top edges, 0 the left/bottom, and 0.5 keeps it
        /// centered.
        /// </para>
        /// <para>
        /// Default gravity is (0, 1, 0, 1), meaning your window will maintain its
        /// position relative to the top left and will not change size as its
        /// containing window grows.
        /// </para>
        /// <para>
        /// If you wanted, say, a window that sticks to the top of the screen (with a
        /// constant height), but which grows to take the full width of the window, you
        /// would pass (0, 1, 1, 1). Because your left and right edges would maintain
        /// their positioning relative to their respective edges of the screen, the
        /// whole width of your window would change with the X-Plane window.
        /// </para>
        /// <para>
        /// Only applies to modern windows. (Windows created using the deprecated
        /// XPLMCreateWindow(), or windows compiled against a pre-XPLM300 version of
        /// the SDK will simply get the default gravity.)
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMSetWindowGravity", ExactSpelling = true)]
        public static extern void SetWindowGravity(WindowID inWindowID, float inLeftGravity, float inTopGravity, float inRightGravity, float inBottomGravity);

        
        /// <summary>
        /// <para>
        /// Sets the minimum and maximum size of the client rectangle of the given
        /// window. (That is, it does not include any window styling that you might
        /// have asked X-Plane to apply on your behalf.) All resizing operations are
        /// constrained to these sizes.
        /// </para>
        /// <para>
        /// Only applies to modern windows. (Windows created using the deprecated
        /// XPLMCreateWindow(), or windows compiled against a pre-XPLM300 version of
        /// the SDK will have no minimum or maximum size.)
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMSetWindowResizingLimits", ExactSpelling = true)]
        public static extern void SetWindowResizingLimits(WindowID inWindowID, int inMinWidthBoxels, int inMinHeightBoxels, int inMaxWidthBoxels, int inMaxHeightBoxels);

        
        /// <summary>
        /// <para>
        /// Sets the policy for how X-Plane will position your window.
        /// </para>
        /// <para>
        /// Some positioning modes apply to a particular monitor. For those modes, you
        /// can pass a negative monitor index to position the window on the main
        /// X-Plane monitor (the screen with the X-Plane menu bar at the top). Or, if
        /// you have a specific monitor you want to position your window on, you can
        /// pass a real monitor index as received from, e.g.,
        /// XPLMGetAllMonitorBoundsOS().
        /// </para>
        /// <para>
        /// Only applies to modern windows. (Windows created using the deprecated
        /// XPLMCreateWindow(), or windows compiled against a pre-XPLM300 version of
        /// the SDK will always use xplm_WindowPositionFree.)
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMSetWindowPositioningMode", ExactSpelling = true)]
        public static extern void SetWindowPositioningMode(WindowID inWindowID, WindowPositioningMode inPositioningMode, int inMonitorIndex);

        
        /// <summary>
        /// <para>
        /// Sets the name for a window. This only applies to windows that opted-in to
        /// styling as an X-Plane 11 floating window (i.e., with styling mode
        /// xplm_WindowDecorationRoundRectangle) when they were created using
        /// XPLMCreateWindowEx().
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMSetWindowTitle", ExactSpelling = true)]
        public static extern unsafe void SetWindowTitle(WindowID inWindowID, byte* inWindowTitle);

        
        /// <summary>
        /// <para>
        /// Sets the name for a window. This only applies to windows that opted-in to
        /// styling as an X-Plane 11 floating window (i.e., with styling mode
        /// xplm_WindowDecorationRoundRectangle) when they were created using
        /// XPLMCreateWindowEx().
        /// </para>
        /// </summary>
        [SkipLocalsInitAttribute]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetWindowTitle(WindowID inWindowID, in ReadOnlySpan<char> inWindowTitle)
        {
            Span<byte> inWindowTitleUtf8 = stackalloc byte[(inWindowTitle.Length << 1) | 1];
            var inWindowTitlePtr = Utils.ToUtf8Unsafe(inWindowTitle, inWindowTitleUtf8);
            SetWindowTitle(inWindowID, inWindowTitlePtr);
        }

        
        /// <summary>
        /// <para>
        /// Returns a window's reference constant, the unique value you can use for
        /// your own purposes.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMGetWindowRefCon", ExactSpelling = true)]
        public static extern unsafe void* GetWindowRefCon(WindowID inWindowID);

        
        /// <summary>
        /// <para>
        /// Sets a window's reference constant.  Use this to pass data to yourself in
        /// the callbacks.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMSetWindowRefCon", ExactSpelling = true)]
        public static extern unsafe void SetWindowRefCon(WindowID inWindowID, void* inRefcon);

        
        /// <summary>
        /// <para>
        /// This routine gives a specific window keyboard focus.  Keystrokes will be
        /// sent to that window.  Pass a window ID of 0 to remove keyboard focus from
        /// any plugin-created windows and instead pass keyboard strokes directly to
        /// X-Plane.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMTakeKeyboardFocus", ExactSpelling = true)]
        public static extern void TakeKeyboardFocus(WindowID inWindow);

        
        /// <summary>
        /// <para>
        /// Returns true (1) if the indicated window has keyboard focus. Pass a window
        /// ID of 0 to see if no plugin window has focus, and all keystrokes will go
        /// directly to X-Plane.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMHasKeyboardFocus", ExactSpelling = true)]
        public static extern int HasKeyboardFocus(WindowID inWindow);

        
        /// <summary>
        /// <para>
        /// This routine brings the window to the front of the Z-order for its layer.
        /// Windows are brought to the front automatically when they are created.
        /// Beyond that, you should make sure you are front before handling mouse
        /// clicks.
        /// </para>
        /// <para>
        /// Note that this only brings your window to the front of its layer
        /// (XPLMWindowLayer). Thus, if you have a window in the floating window layer
        /// (xplm_WindowLayerFloatingWindows), but there is a modal window (in layer
        /// xplm_WindowLayerModal) above you, you would still not be the true frontmost
        /// window after calling this. (After all, the window layers are strictly
        /// ordered, and no window in a lower layer can ever be above any window in a
        /// higher one.)
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMBringWindowToFront", ExactSpelling = true)]
        public static extern void BringWindowToFront(WindowID inWindow);

        
        /// <summary>
        /// <para>
        /// This routine returns true if the window you passed in is the frontmost
        /// visible window in its layer (XPLMWindowLayer).
        /// </para>
        /// <para>
        /// Thus, if you have a window at the front of the floating window layer
        /// (xplm_WindowLayerFloatingWindows), this will return true even if there is a
        /// modal window (in layer xplm_WindowLayerModal) above you. (Not to worry,
        /// though: in such a case, X-Plane will not pass clicks or keyboard input down
        /// to your layer until the window above stops "eating" the input.)
        /// </para>
        /// <para>
        /// Note that legacy windows are always placed in layer
        /// xplm_WindowLayerFlightOverlay, while modern-style windows default to
        /// xplm_WindowLayerFloatingWindows. This means it's perfectly consistent to
        /// have two different plugin-created windows (one legacy, one modern) *both*
        /// be in the front (of their different layers!) at the same time.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMIsWindowInFront", ExactSpelling = true)]
        public static extern int IsWindowInFront(WindowID inWindow);

        
        /// <summary>
        /// <para>
        /// This routine registers a key sniffing callback.  You specify whether you
        /// want to sniff before the window system, or only sniff keys the window
        /// system does not consume.  You should ALMOST ALWAYS sniff non-control keys
        /// after the window system.  When the window system consumes a key, it is
        /// because the user has "focused" a window.  Consuming the key or taking
        /// action based on the key will produce very weird results.  Returns
        /// 1 if successful.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMRegisterKeySniffer", ExactSpelling = true)]
        public static extern unsafe int RegisterKeySniffer(delegate* unmanaged[Cdecl]<byte, KeyFlags, byte, void*, int> inCallback, int inBeforeWindows, void* inRefcon);

        
        /// <summary>
        /// <para>
        /// This routine unregisters a key sniffer.  You must unregister a key sniffer
        /// for every time you register one with the exact same signature.  Returns 1
        /// if successful.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMUnregisterKeySniffer", ExactSpelling = true)]
        public static extern unsafe int UnregisterKeySniffer(delegate* unmanaged[Cdecl]<byte, KeyFlags, byte, void*, int> inCallback, int inBeforeWindows, void* inRefcon);

        
        /// <summary>
        /// <para>
        /// This routine registers a hot key.  You specify your preferred key stroke
        /// virtual key/flag combination, a description of what your callback does (so
        /// other plug-ins can describe the plug-in to the user for remapping) and a
        /// callback function and opaque pointer to pass in).  A new hot key ID is
        /// returned.  During execution, the actual key associated with your hot key
        /// may change, but you are insulated from this.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMRegisterHotKey", ExactSpelling = true)]
        public static extern unsafe HotKeyID RegisterHotKey(byte inVirtualKey, KeyFlags inFlags, byte* inDescription, delegate* unmanaged[Cdecl]<void*, void> inCallback, void* inRefcon);

        
        /// <summary>
        /// <para>
        /// This routine registers a hot key.  You specify your preferred key stroke
        /// virtual key/flag combination, a description of what your callback does (so
        /// other plug-ins can describe the plug-in to the user for remapping) and a
        /// callback function and opaque pointer to pass in).  A new hot key ID is
        /// returned.  During execution, the actual key associated with your hot key
        /// may change, but you are insulated from this.
        /// </para>
        /// </summary>
        [SkipLocalsInitAttribute]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe HotKeyID RegisterHotKey(byte inVirtualKey, KeyFlags inFlags, in ReadOnlySpan<char> inDescription, delegate* unmanaged[Cdecl]<void*, void> inCallback, void* inRefcon)
        {
            Span<byte> inDescriptionUtf8 = stackalloc byte[(inDescription.Length << 1) | 1];
            var inDescriptionPtr = Utils.ToUtf8Unsafe(inDescription, inDescriptionUtf8);
            return RegisterHotKey(inVirtualKey, inFlags, inDescriptionPtr, inCallback, inRefcon);
        }

        
        /// <summary>
        /// <para>
        /// Unregisters a hot key.  You can only unregister your own hot keys.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMUnregisterHotKey", ExactSpelling = true)]
        public static extern void UnregisterHotKey(HotKeyID inHotKey);

        
        /// <summary>
        /// <para>
        /// Returns the number of current hot keys.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMCountHotKeys", ExactSpelling = true)]
        public static extern int CountHotKeys();

        
        /// <summary>
        /// <para>
        /// Returns a hot key by index, for iteration on all hot keys.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMGetNthHotKey", ExactSpelling = true)]
        public static extern HotKeyID GetNthHotKey(int inIndex);

        
        /// <summary>
        /// <para>
        /// Returns information about the hot key.  Return NULL for any parameter you
        /// don't want info about.  The description should be at least 512 chars long.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMGetHotKeyInfo", ExactSpelling = true)]
        public static extern unsafe void GetHotKeyInfo(HotKeyID inHotKey, byte* outVirtualKey, KeyFlags* outFlags, byte* outDescription, PluginID* outPlugin);

        
        /// <summary>
        /// <para>
        /// Remaps a hot key's keystrokes.  You may remap another plugin's keystrokes.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMSetHotKeyCombination", ExactSpelling = true)]
        public static extern void SetHotKeyCombination(HotKeyID inHotKey, byte inVirtualKey, KeyFlags inFlags);
    }
}