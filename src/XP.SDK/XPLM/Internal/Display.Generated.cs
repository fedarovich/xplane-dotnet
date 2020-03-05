using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using InlineIL;

namespace XP.SDK.XPLM.Internal
{
    public static partial class Display
    {
        private static IntPtr RegisterDrawCallbackPtr;
        private static IntPtr UnregisterDrawCallbackPtr;
        private static IntPtr CreateWindowExPtr;
        private static IntPtr CreateWindowPtr;
        private static IntPtr DestroyWindowPtr;
        private static IntPtr GetScreenSizePtr;
        private static IntPtr GetScreenBoundsGlobalPtr;
        private static IntPtr GetAllMonitorBoundsGlobalPtr;
        private static IntPtr GetAllMonitorBoundsOSPtr;
        private static IntPtr GetMouseLocationPtr;
        private static IntPtr GetMouseLocationGlobalPtr;
        private static IntPtr GetWindowGeometryPtr;
        private static IntPtr SetWindowGeometryPtr;
        private static IntPtr GetWindowGeometryOSPtr;
        private static IntPtr SetWindowGeometryOSPtr;
        private static IntPtr GetWindowGeometryVRPtr;
        private static IntPtr SetWindowGeometryVRPtr;
        private static IntPtr GetWindowIsVisiblePtr;
        private static IntPtr SetWindowIsVisiblePtr;
        private static IntPtr WindowIsPoppedOutPtr;
        private static IntPtr WindowIsInVRPtr;
        private static IntPtr SetWindowGravityPtr;
        private static IntPtr SetWindowResizingLimitsPtr;
        private static IntPtr SetWindowPositioningModePtr;
        private static IntPtr SetWindowTitlePtr;
        private static IntPtr GetWindowRefConPtr;
        private static IntPtr SetWindowRefConPtr;
        private static IntPtr TakeKeyboardFocusPtr;
        private static IntPtr HasKeyboardFocusPtr;
        private static IntPtr BringWindowToFrontPtr;
        private static IntPtr IsWindowInFrontPtr;
        private static IntPtr RegisterKeySnifferPtr;
        private static IntPtr UnregisterKeySnifferPtr;
        private static IntPtr RegisterHotKeyPtr;
        private static IntPtr UnregisterHotKeyPtr;
        private static IntPtr CountHotKeysPtr;
        private static IntPtr GetNthHotKeyPtr;
        private static IntPtr GetHotKeyInfoPtr;
        private static IntPtr SetHotKeyCombinationPtr;

        static Display()
        {
            RegisterDrawCallbackPtr = Lib.GetExport("XPLMRegisterDrawCallback");
            UnregisterDrawCallbackPtr = Lib.GetExport("XPLMUnregisterDrawCallback");
            CreateWindowExPtr = Lib.GetExport("XPLMCreateWindowEx");
            CreateWindowPtr = Lib.GetExport("XPLMCreateWindow");
            DestroyWindowPtr = Lib.GetExport("XPLMDestroyWindow");
            GetScreenSizePtr = Lib.GetExport("XPLMGetScreenSize");
            GetScreenBoundsGlobalPtr = Lib.GetExport("XPLMGetScreenBoundsGlobal");
            GetAllMonitorBoundsGlobalPtr = Lib.GetExport("XPLMGetAllMonitorBoundsGlobal");
            GetAllMonitorBoundsOSPtr = Lib.GetExport("XPLMGetAllMonitorBoundsOS");
            GetMouseLocationPtr = Lib.GetExport("XPLMGetMouseLocation");
            GetMouseLocationGlobalPtr = Lib.GetExport("XPLMGetMouseLocationGlobal");
            GetWindowGeometryPtr = Lib.GetExport("XPLMGetWindowGeometry");
            SetWindowGeometryPtr = Lib.GetExport("XPLMSetWindowGeometry");
            GetWindowGeometryOSPtr = Lib.GetExport("XPLMGetWindowGeometryOS");
            SetWindowGeometryOSPtr = Lib.GetExport("XPLMSetWindowGeometryOS");
            GetWindowGeometryVRPtr = Lib.GetExport("XPLMGetWindowGeometryVR");
            SetWindowGeometryVRPtr = Lib.GetExport("XPLMSetWindowGeometryVR");
            GetWindowIsVisiblePtr = Lib.GetExport("XPLMGetWindowIsVisible");
            SetWindowIsVisiblePtr = Lib.GetExport("XPLMSetWindowIsVisible");
            WindowIsPoppedOutPtr = Lib.GetExport("XPLMWindowIsPoppedOut");
            WindowIsInVRPtr = Lib.GetExport("XPLMWindowIsInVR");
            SetWindowGravityPtr = Lib.GetExport("XPLMSetWindowGravity");
            SetWindowResizingLimitsPtr = Lib.GetExport("XPLMSetWindowResizingLimits");
            SetWindowPositioningModePtr = Lib.GetExport("XPLMSetWindowPositioningMode");
            SetWindowTitlePtr = Lib.GetExport("XPLMSetWindowTitle");
            GetWindowRefConPtr = Lib.GetExport("XPLMGetWindowRefCon");
            SetWindowRefConPtr = Lib.GetExport("XPLMSetWindowRefCon");
            TakeKeyboardFocusPtr = Lib.GetExport("XPLMTakeKeyboardFocus");
            HasKeyboardFocusPtr = Lib.GetExport("XPLMHasKeyboardFocus");
            BringWindowToFrontPtr = Lib.GetExport("XPLMBringWindowToFront");
            IsWindowInFrontPtr = Lib.GetExport("XPLMIsWindowInFront");
            RegisterKeySnifferPtr = Lib.GetExport("XPLMRegisterKeySniffer");
            UnregisterKeySnifferPtr = Lib.GetExport("XPLMUnregisterKeySniffer");
            RegisterHotKeyPtr = Lib.GetExport("XPLMRegisterHotKey");
            UnregisterHotKeyPtr = Lib.GetExport("XPLMUnregisterHotKey");
            CountHotKeysPtr = Lib.GetExport("XPLMCountHotKeys");
            GetNthHotKeyPtr = Lib.GetExport("XPLMGetNthHotKey");
            GetHotKeyInfoPtr = Lib.GetExport("XPLMGetHotKeyInfo");
            SetHotKeyCombinationPtr = Lib.GetExport("XPLMSetHotKeyCombination");
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static unsafe int RegisterDrawCallbackPrivate(IntPtr inCallback, DrawingPhase inPhase, int inWantsBefore, void* inRefcon)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(RegisterDrawCallbackPtr);
            int result;
            IL.Push(inCallback);
            IL.Push(inPhase);
            IL.Push(inWantsBefore);
            IL.Push(inRefcon);
            IL.Push(RegisterDrawCallbackPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(DrawCallback), typeof(DrawingPhase), typeof(int), typeof(void*)));
            IL.Pop(out result);
            return result;
        }

        
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int RegisterDrawCallback(DrawCallback inCallback, DrawingPhase inPhase, int inWantsBefore, void* inRefcon)
        {
            IL.DeclareLocals(false);
            IntPtr inCallbackPtr = Marshal.GetFunctionPointerForDelegate(inCallback);
            int result = RegisterDrawCallbackPrivate(inCallbackPtr, inPhase, inWantsBefore, inRefcon);
            GC.KeepAlive(inCallbackPtr);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static unsafe int UnregisterDrawCallbackPrivate(IntPtr inCallback, DrawingPhase inPhase, int inWantsBefore, void* inRefcon)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(UnregisterDrawCallbackPtr);
            int result;
            IL.Push(inCallback);
            IL.Push(inPhase);
            IL.Push(inWantsBefore);
            IL.Push(inRefcon);
            IL.Push(UnregisterDrawCallbackPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(DrawCallback), typeof(DrawingPhase), typeof(int), typeof(void*)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine unregisters a draw callback.  You must unregister a callback
        /// for each  time you register a callback if you have registered it multiple
        /// times with different refcons.  The routine returns 1 if it can find the
        /// callback to unregister, 0 otherwise.
        /// </para>
        /// <para>
        /// Note that this function will likely be removed during the X-Plane 11 run as
        /// part of the transition to Vulkan/Metal/etc. See the XPLMInstance API for
        /// future-proof drawing of 3-D objects.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int UnregisterDrawCallback(DrawCallback inCallback, DrawingPhase inPhase, int inWantsBefore, void* inRefcon)
        {
            IL.DeclareLocals(false);
            IntPtr inCallbackPtr = Marshal.GetFunctionPointerForDelegate(inCallback);
            int result = UnregisterDrawCallbackPrivate(inCallbackPtr, inPhase, inWantsBefore, inRefcon);
            GC.KeepAlive(inCallbackPtr);
            return result;
        }

        
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe WindowID CreateWindowEx(CreateWindow* inParams)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(CreateWindowExPtr);
            WindowID result;
            IL.Push(inParams);
            IL.Push(CreateWindowExPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(WindowID), typeof(CreateWindow*)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static unsafe WindowID CreateWindowPrivate(int inLeft, int inTop, int inRight, int inBottom, int inIsVisible, IntPtr inDrawCallback, IntPtr inKeyCallback, IntPtr inMouseCallback, void* inRefcon)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(CreateWindowPtr);
            WindowID result;
            IL.Push(inLeft);
            IL.Push(inTop);
            IL.Push(inRight);
            IL.Push(inBottom);
            IL.Push(inIsVisible);
            IL.Push(inDrawCallback);
            IL.Push(inKeyCallback);
            IL.Push(inMouseCallback);
            IL.Push(inRefcon);
            IL.Push(CreateWindowPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(WindowID), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(DrawWindowCallback), typeof(HandleKeyCallback), typeof(HandleMouseClickCallback), typeof(void*)));
            IL.Pop(out result);
            return result;
        }

        
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe WindowID CreateWindow(int inLeft, int inTop, int inRight, int inBottom, int inIsVisible, DrawWindowCallback inDrawCallback, HandleKeyCallback inKeyCallback, HandleMouseClickCallback inMouseCallback, void* inRefcon)
        {
            IL.DeclareLocals(false);
            IntPtr inDrawCallbackPtr = Marshal.GetFunctionPointerForDelegate(inDrawCallback);
            IntPtr inKeyCallbackPtr = Marshal.GetFunctionPointerForDelegate(inKeyCallback);
            IntPtr inMouseCallbackPtr = Marshal.GetFunctionPointerForDelegate(inMouseCallback);
            WindowID result = CreateWindowPrivate(inLeft, inTop, inRight, inBottom, inIsVisible, inDrawCallbackPtr, inKeyCallbackPtr, inMouseCallbackPtr, inRefcon);
            GC.KeepAlive(inMouseCallbackPtr);
            GC.KeepAlive(inKeyCallbackPtr);
            GC.KeepAlive(inDrawCallbackPtr);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine destroys a window.  The window's callbacks are not called
        /// after this call. Keyboard focus is removed from the window before
        /// destroying it.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void DestroyWindow(WindowID inWindowID)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(DestroyWindowPtr);
            IL.Push(inWindowID);
            IL.Push(DestroyWindowPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WindowID)));
        }

        
        /// <summary>
        /// <para>
        /// This routine returns the size of the main X-Plane OpenGL window in pixels.
        /// This number can be used to get a rough idea of the amount of detail the
        /// user will be able to see when drawing in 3-d.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetScreenSize(int* outWidth, int* outHeight)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(GetScreenSizePtr);
            IL.Push(outWidth);
            IL.Push(outHeight);
            IL.Push(GetScreenSizePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int*), typeof(int*)));
        }

        
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetScreenBoundsGlobal(int* outLeft, int* outTop, int* outRight, int* outBottom)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(GetScreenBoundsGlobalPtr);
            IL.Push(outLeft);
            IL.Push(outTop);
            IL.Push(outRight);
            IL.Push(outBottom);
            IL.Push(GetScreenBoundsGlobalPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int*), typeof(int*), typeof(int*), typeof(int*)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static unsafe void GetAllMonitorBoundsGlobalPrivate(IntPtr inMonitorBoundsCallback, void* inRefcon)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(GetAllMonitorBoundsGlobalPtr);
            IL.Push(inMonitorBoundsCallback);
            IL.Push(inRefcon);
            IL.Push(GetAllMonitorBoundsGlobalPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(ReceiveMonitorBoundsGlobalCallback), typeof(void*)));
        }

        
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetAllMonitorBoundsGlobal(ReceiveMonitorBoundsGlobalCallback inMonitorBoundsCallback, void* inRefcon)
        {
            IL.DeclareLocals(false);
            IntPtr inMonitorBoundsCallbackPtr = Marshal.GetFunctionPointerForDelegate(inMonitorBoundsCallback);
            GetAllMonitorBoundsGlobalPrivate(inMonitorBoundsCallbackPtr, inRefcon);
            GC.KeepAlive(inMonitorBoundsCallbackPtr);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static unsafe void GetAllMonitorBoundsOSPrivate(IntPtr inMonitorBoundsCallback, void* inRefcon)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(GetAllMonitorBoundsOSPtr);
            IL.Push(inMonitorBoundsCallback);
            IL.Push(inRefcon);
            IL.Push(GetAllMonitorBoundsOSPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(ReceiveMonitorBoundsOSCallback), typeof(void*)));
        }

        
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetAllMonitorBoundsOS(ReceiveMonitorBoundsOSCallback inMonitorBoundsCallback, void* inRefcon)
        {
            IL.DeclareLocals(false);
            IntPtr inMonitorBoundsCallbackPtr = Marshal.GetFunctionPointerForDelegate(inMonitorBoundsCallback);
            GetAllMonitorBoundsOSPrivate(inMonitorBoundsCallbackPtr, inRefcon);
            GC.KeepAlive(inMonitorBoundsCallbackPtr);
        }

        
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetMouseLocation(int* outX, int* outY)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(GetMouseLocationPtr);
            IL.Push(outX);
            IL.Push(outY);
            IL.Push(GetMouseLocationPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int*), typeof(int*)));
        }

        
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetMouseLocationGlobal(int* outX, int* outY)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(GetMouseLocationGlobalPtr);
            IL.Push(outX);
            IL.Push(outY);
            IL.Push(GetMouseLocationGlobalPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int*), typeof(int*)));
        }

        
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetWindowGeometry(WindowID inWindowID, int* outLeft, int* outTop, int* outRight, int* outBottom)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(GetWindowGeometryPtr);
            IL.Push(inWindowID);
            IL.Push(outLeft);
            IL.Push(outTop);
            IL.Push(outRight);
            IL.Push(outBottom);
            IL.Push(GetWindowGeometryPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WindowID), typeof(int*), typeof(int*), typeof(int*), typeof(int*)));
        }

        
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetWindowGeometry(WindowID inWindowID, int inLeft, int inTop, int inRight, int inBottom)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(SetWindowGeometryPtr);
            IL.Push(inWindowID);
            IL.Push(inLeft);
            IL.Push(inTop);
            IL.Push(inRight);
            IL.Push(inBottom);
            IL.Push(SetWindowGeometryPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WindowID), typeof(int), typeof(int), typeof(int), typeof(int)));
        }

        
        /// <summary>
        /// <para>
        /// This routine returns the position and size of a "popped out" window (i.e.,
        /// a window whose positioning mode is xplm_WindowPopOut), in operating system
        /// pixels.  Pass NULL to not receive any parameter.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetWindowGeometryOS(WindowID inWindowID, int* outLeft, int* outTop, int* outRight, int* outBottom)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(GetWindowGeometryOSPtr);
            IL.Push(inWindowID);
            IL.Push(outLeft);
            IL.Push(outTop);
            IL.Push(outRight);
            IL.Push(outBottom);
            IL.Push(GetWindowGeometryOSPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WindowID), typeof(int*), typeof(int*), typeof(int*), typeof(int*)));
        }

        
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetWindowGeometryOS(WindowID inWindowID, int inLeft, int inTop, int inRight, int inBottom)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(SetWindowGeometryOSPtr);
            IL.Push(inWindowID);
            IL.Push(inLeft);
            IL.Push(inTop);
            IL.Push(inRight);
            IL.Push(inBottom);
            IL.Push(SetWindowGeometryOSPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WindowID), typeof(int), typeof(int), typeof(int), typeof(int)));
        }

        
        /// <summary>
        /// <para>
        /// Returns the width and height, in boxels, of a window in VR. Note that you
        /// are responsible for ensuring your window is in VR (using
        /// XPLMWindowIsInVR()).
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetWindowGeometryVR(WindowID inWindowID, int* outWidthBoxels, int* outHeightBoxels)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(GetWindowGeometryVRPtr);
            IL.Push(inWindowID);
            IL.Push(outWidthBoxels);
            IL.Push(outHeightBoxels);
            IL.Push(GetWindowGeometryVRPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WindowID), typeof(int*), typeof(int*)));
        }

        
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetWindowGeometryVR(WindowID inWindowID, int widthBoxels, int heightBoxels)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(SetWindowGeometryVRPtr);
            IL.Push(inWindowID);
            IL.Push(widthBoxels);
            IL.Push(heightBoxels);
            IL.Push(SetWindowGeometryVRPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WindowID), typeof(int), typeof(int)));
        }

        
        /// <summary>
        /// <para>
        /// This routine returns whether a window is visible.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int GetWindowIsVisible(WindowID inWindowID)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(GetWindowIsVisiblePtr);
            int result;
            IL.Push(inWindowID);
            IL.Push(GetWindowIsVisiblePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(WindowID)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine shows or hides a window.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetWindowIsVisible(WindowID inWindowID, int inIsVisible)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(SetWindowIsVisiblePtr);
            IL.Push(inWindowID);
            IL.Push(inIsVisible);
            IL.Push(SetWindowIsVisiblePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WindowID), typeof(int)));
        }

        
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int WindowIsPoppedOut(WindowID inWindowID)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(WindowIsPoppedOutPtr);
            int result;
            IL.Push(inWindowID);
            IL.Push(WindowIsPoppedOutPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(WindowID)));
            IL.Pop(out result);
            return result;
        }

        
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int WindowIsInVR(WindowID inWindowID)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(WindowIsInVRPtr);
            int result;
            IL.Push(inWindowID);
            IL.Push(WindowIsInVRPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(WindowID)));
            IL.Pop(out result);
            return result;
        }

        
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetWindowGravity(WindowID inWindowID, float inLeftGravity, float inTopGravity, float inRightGravity, float inBottomGravity)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(SetWindowGravityPtr);
            IL.Push(inWindowID);
            IL.Push(inLeftGravity);
            IL.Push(inTopGravity);
            IL.Push(inRightGravity);
            IL.Push(inBottomGravity);
            IL.Push(SetWindowGravityPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WindowID), typeof(float), typeof(float), typeof(float), typeof(float)));
        }

        
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetWindowResizingLimits(WindowID inWindowID, int inMinWidthBoxels, int inMinHeightBoxels, int inMaxWidthBoxels, int inMaxHeightBoxels)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(SetWindowResizingLimitsPtr);
            IL.Push(inWindowID);
            IL.Push(inMinWidthBoxels);
            IL.Push(inMinHeightBoxels);
            IL.Push(inMaxWidthBoxels);
            IL.Push(inMaxHeightBoxels);
            IL.Push(SetWindowResizingLimitsPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WindowID), typeof(int), typeof(int), typeof(int), typeof(int)));
        }

        
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetWindowPositioningMode(WindowID inWindowID, WindowPositioningMode inPositioningMode, int inMonitorIndex)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(SetWindowPositioningModePtr);
            IL.Push(inWindowID);
            IL.Push(inPositioningMode);
            IL.Push(inMonitorIndex);
            IL.Push(SetWindowPositioningModePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WindowID), typeof(WindowPositioningMode), typeof(int)));
        }

        
        /// <summary>
        /// <para>
        /// Sets the name for a window. This only applies to windows that opted-in to
        /// styling as an X-Plane 11 floating window (i.e., with styling mode
        /// xplm_WindowDecorationRoundRectangle) when they were created using
        /// XPLMCreateWindowEx().
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetWindowTitle(WindowID inWindowID, byte* inWindowTitle)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(SetWindowTitlePtr);
            IL.Push(inWindowID);
            IL.Push(inWindowTitle);
            IL.Push(SetWindowTitlePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WindowID), typeof(byte*)));
        }

        
        /// <summary>
        /// <para>
        /// Sets the name for a window. This only applies to windows that opted-in to
        /// styling as an X-Plane 11 floating window (i.e., with styling mode
        /// xplm_WindowDecorationRoundRectangle) when they were created using
        /// XPLMCreateWindowEx().
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetWindowTitle(WindowID inWindowID, in ReadOnlySpan<char> inWindowTitle)
        {
            IL.DeclareLocals(false);
            Span<byte> inWindowTitleUtf8 = stackalloc byte[(inWindowTitle.Length << 1) | 1];
            var inWindowTitlePtr = Utils.ToUtf8Unsafe(inWindowTitle, inWindowTitleUtf8);
            SetWindowTitle(inWindowID, inWindowTitlePtr);
        }

        
        /// <summary>
        /// <para>
        /// This routine returns a window's reference constant, the unique value you
        /// can use for your own purposes.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void* GetWindowRefCon(WindowID inWindowID)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(GetWindowRefConPtr);
            void* result;
            IL.Push(inWindowID);
            IL.Push(GetWindowRefConPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void*), typeof(WindowID)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine sets a window's reference constant.  Use this to pass data to
        /// yourself in the callbacks.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetWindowRefCon(WindowID inWindowID, void* inRefcon)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(SetWindowRefConPtr);
            IL.Push(inWindowID);
            IL.Push(inRefcon);
            IL.Push(SetWindowRefConPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WindowID), typeof(void*)));
        }

        
        /// <summary>
        /// <para>
        /// This routine gives a specific window keyboard focus.  Keystrokes will be
        /// sent to  that window.  Pass a window ID of 0 to remove keyboard focus from
        /// any plugin-created windows and instead pass keyboard strokes directly to
        /// X-Plane.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void TakeKeyboardFocus(WindowID inWindow)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(TakeKeyboardFocusPtr);
            IL.Push(inWindow);
            IL.Push(TakeKeyboardFocusPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WindowID)));
        }

        
        /// <summary>
        /// <para>
        /// Returns true (1) if the indicated window has keyboard focus. Pass a window
        /// ID of 0 to see if no plugin window has focus, and all keystrokes will go
        /// directly to X-Plane.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int HasKeyboardFocus(WindowID inWindow)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(HasKeyboardFocusPtr);
            int result;
            IL.Push(inWindow);
            IL.Push(HasKeyboardFocusPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(WindowID)));
            IL.Pop(out result);
            return result;
        }

        
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void BringWindowToFront(WindowID inWindow)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(BringWindowToFrontPtr);
            IL.Push(inWindow);
            IL.Push(BringWindowToFrontPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WindowID)));
        }

        
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
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int IsWindowInFront(WindowID inWindow)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(IsWindowInFrontPtr);
            int result;
            IL.Push(inWindow);
            IL.Push(IsWindowInFrontPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(WindowID)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static unsafe int RegisterKeySnifferPrivate(IntPtr inCallback, int inBeforeWindows, void* inRefcon)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(RegisterKeySnifferPtr);
            int result;
            IL.Push(inCallback);
            IL.Push(inBeforeWindows);
            IL.Push(inRefcon);
            IL.Push(RegisterKeySnifferPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(KeySnifferCallback), typeof(int), typeof(void*)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine registers a key sniffing callback.  You specify whether you
        /// want to sniff before the window system, or only sniff keys the window
        /// system does not consume.  You should ALMOST ALWAYS sniff non-control keys
        /// after the window system.  When the window system consumes a key, it is
        /// because the user has "focused" a window.  Consuming the key or taking
        /// action based on the key will produce very weird results.  Returns 1 if
        /// successful.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int RegisterKeySniffer(KeySnifferCallback inCallback, int inBeforeWindows, void* inRefcon)
        {
            IL.DeclareLocals(false);
            IntPtr inCallbackPtr = Marshal.GetFunctionPointerForDelegate(inCallback);
            int result = RegisterKeySnifferPrivate(inCallbackPtr, inBeforeWindows, inRefcon);
            GC.KeepAlive(inCallbackPtr);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static unsafe int UnregisterKeySnifferPrivate(IntPtr inCallback, int inBeforeWindows, void* inRefcon)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(UnregisterKeySnifferPtr);
            int result;
            IL.Push(inCallback);
            IL.Push(inBeforeWindows);
            IL.Push(inRefcon);
            IL.Push(UnregisterKeySnifferPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(KeySnifferCallback), typeof(int), typeof(void*)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine unregisters a key sniffer.  You must unregister a key sniffer
        /// for every time you register one with the exact same signature.  Returns 1
        /// if successful.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int UnregisterKeySniffer(KeySnifferCallback inCallback, int inBeforeWindows, void* inRefcon)
        {
            IL.DeclareLocals(false);
            IntPtr inCallbackPtr = Marshal.GetFunctionPointerForDelegate(inCallback);
            int result = UnregisterKeySnifferPrivate(inCallbackPtr, inBeforeWindows, inRefcon);
            GC.KeepAlive(inCallbackPtr);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static unsafe HotKeyID RegisterHotKeyPrivate(byte inVirtualKey, KeyFlags inFlags, byte* inDescription, IntPtr inCallback, void* inRefcon)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(RegisterHotKeyPtr);
            HotKeyID result;
            IL.Push(inVirtualKey);
            IL.Push(inFlags);
            IL.Push(inDescription);
            IL.Push(inCallback);
            IL.Push(inRefcon);
            IL.Push(RegisterHotKeyPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(HotKeyID), typeof(byte), typeof(KeyFlags), typeof(byte*), typeof(HotKeyCallback), typeof(void*)));
            IL.Pop(out result);
            return result;
        }

        
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe HotKeyID RegisterHotKey(byte inVirtualKey, KeyFlags inFlags, byte* inDescription, HotKeyCallback inCallback, void* inRefcon)
        {
            IL.DeclareLocals(false);
            IntPtr inCallbackPtr = Marshal.GetFunctionPointerForDelegate(inCallback);
            HotKeyID result = RegisterHotKeyPrivate(inVirtualKey, inFlags, inDescription, inCallbackPtr, inRefcon);
            GC.KeepAlive(inCallbackPtr);
            return result;
        }

        
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe HotKeyID RegisterHotKey(byte inVirtualKey, KeyFlags inFlags, in ReadOnlySpan<char> inDescription, HotKeyCallback inCallback, void* inRefcon)
        {
            IL.DeclareLocals(false);
            Span<byte> inDescriptionUtf8 = stackalloc byte[(inDescription.Length << 1) | 1];
            var inDescriptionPtr = Utils.ToUtf8Unsafe(inDescription, inDescriptionUtf8);
            return RegisterHotKey(inVirtualKey, inFlags, inDescriptionPtr, inCallback, inRefcon);
        }

        
        /// <summary>
        /// <para>
        /// This API unregisters a hot key.  You can only unregister your own hot keys.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void UnregisterHotKey(HotKeyID inHotKey)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(UnregisterHotKeyPtr);
            IL.Push(inHotKey);
            IL.Push(UnregisterHotKeyPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(HotKeyID)));
        }

        
        /// <summary>
        /// <para>
        /// Returns the number of current hot keys.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int CountHotKeys()
        {
            IL.DeclareLocals(false);
            Guard.NotNull(CountHotKeysPtr);
            int result;
            IL.Push(CountHotKeysPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// Returns a hot key by index, for iteration on all hot keys.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static HotKeyID GetNthHotKey(int inIndex)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(GetNthHotKeyPtr);
            HotKeyID result;
            IL.Push(inIndex);
            IL.Push(GetNthHotKeyPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(HotKeyID), typeof(int)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// Returns information about the hot key.  Return NULL for any  parameter you
        /// don't want info about.  The description should be at least 512 chars long.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetHotKeyInfo(HotKeyID inHotKey, byte* outVirtualKey, KeyFlags* outFlags, byte* outDescription, PluginID* outPlugin)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(GetHotKeyInfoPtr);
            IL.Push(inHotKey);
            IL.Push(outVirtualKey);
            IL.Push(outFlags);
            IL.Push(outDescription);
            IL.Push(outPlugin);
            IL.Push(GetHotKeyInfoPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(HotKeyID), typeof(byte*), typeof(KeyFlags*), typeof(byte*), typeof(PluginID*)));
        }

        
        /// <summary>
        /// <para>
        /// XPLMSetHotKeyCombination remaps a hot keys keystrokes.  You may remap
        /// another plugin's keystrokes.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetHotKeyCombination(HotKeyID inHotKey, byte inVirtualKey, KeyFlags inFlags)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(SetHotKeyCombinationPtr);
            IL.Push(inHotKey);
            IL.Push(inVirtualKey);
            IL.Push(inFlags);
            IL.Push(SetHotKeyCombinationPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(HotKeyID), typeof(byte), typeof(KeyFlags)));
        }
    }
}