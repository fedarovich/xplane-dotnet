using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using XP.SDK.XPLM.Internal;

namespace XP.SDK.XPLM
{
    /// <summary>
    /// The base class for modern (XPLM300) X-Plane windows.
    /// </summary>
    public abstract class WindowBase : IDisposable
    {
        #region Callbacks

        private static readonly DrawWindowCallback _drawWindowCallback;
        private static readonly HandleMouseClickCallback _handleLeftClickCallback;
        private static readonly HandleMouseClickCallback _handleRightClickCallback;
        private static readonly HandleMouseWheelCallback _handleMouseWheelCallback;
        private static readonly HandleKeyCallback _handleKeyCallback;
        private static readonly HandleCursorCallback _handleCursorCallback;

        #endregion

        private int _disposed;
        private GCHandle _handle;
        private WindowID _id;
        private string _title;

        #region Constructors

        static unsafe WindowBase()
        {
            _drawWindowCallback = DrawWindow;
            _handleLeftClickCallback = HandleMouseLeftClick;
            _handleRightClickCallback = HandleMouseRightClick;
            _handleMouseWheelCallback = HandleMouseWheel;
            _handleKeyCallback = HandleKey;
            _handleCursorCallback = HandleCursor;

            static void DrawWindow(WindowID inwindowid, void* inrefcon) => 
                Utils.TryGetObject<WindowBase>(inrefcon)?.OnDrawWindow();

            static int HandleMouseLeftClick(WindowID inwindowid, int x, int y, MouseStatus inmouse, void* inrefcon) =>
                Utils.TryGetObject<WindowBase>(inrefcon)?.OnMouseLeftButtonEvent(x, y, inmouse) == true ? 1 : 0;

            static int HandleMouseRightClick(WindowID inwindowid, int x, int y, MouseStatus inmouse, void* inrefcon) =>
                Utils.TryGetObject<WindowBase>(inrefcon)?.OnMouseRightButtonEvent(x, y, inmouse) == true ? 1 : 0;

            static int HandleMouseWheel(WindowID inwindowid, int x, int y, int wheel, int clicks, void* inrefcon) =>
                Utils.TryGetObject<WindowBase>(inrefcon)?.OnMouseWheelEvent(x, y, (MouseWheel)wheel, clicks) == true ? 1 : 0;

            static void HandleKey(WindowID inwindowid, byte inkey, KeyFlags inflags, byte invirtualkey, void* inrefcon, int losingfocus) =>
                Utils.TryGetObject<WindowBase>(inrefcon)?.OnKeyEvent(inkey, inflags, invirtualkey, losingfocus == 1);

            static CursorStatus HandleCursor(WindowID inwindowid, int x, int y, void* inrefcon) =>
                Utils.TryGetObject<WindowBase>(inrefcon)?.OnCursorRequested(x, y) ?? CursorStatus.Default;
            
        }

        /// <summary>
        /// Creates a new instance of WindowBase.
        /// </summary>
        /// <param name="bounds">Window bounds in global desktop boxels.</param>
        /// <param name="visible">Window visibility.</param>
        /// <param name="layer">Window layer.</param>
        /// <param name="decoration">The type of X-Plane 11-style "wrapper" you want around your window, if any.</param>
        /// <param name="mouseHandlers">The mouse events, that the window must handle.</param>
        protected unsafe WindowBase(in Rect bounds, bool visible,
            WindowLayer layer = WindowLayer.FloatingWindows,
            WindowDecoration decoration = WindowDecoration.None,
            MouseHandlers mouseHandlers = MouseHandlers.All)
        {
            _handle = GCHandle.Alloc(this);
            
            var parameters = new CreateWindow
            {
                structSize = Unsafe.SizeOf<CreateWindow>(),
                left = bounds.Left,
                top = bounds.Top,
                right = bounds.Right,
                bottom = bounds.Bottom,
                visible = visible ? 1 : 0,
                drawWindowFunc = Marshal.GetFunctionPointerForDelegate(_drawWindowCallback),
                handleMouseClickFunc = (mouseHandlers & MouseHandlers.LeftClick) != default 
                    ? Marshal.GetFunctionPointerForDelegate(_handleLeftClickCallback) 
                    : IntPtr.Zero,
                handleKeyFunc = Marshal.GetFunctionPointerForDelegate(_handleKeyCallback),
                handleCursorFunc = Marshal.GetFunctionPointerForDelegate(_handleCursorCallback),
                handleMouseWheelFunc = Marshal.GetFunctionPointerForDelegate(_handleMouseWheelCallback),
                refcon = GCHandle.ToIntPtr(_handle).ToPointer(),
                decorateAsFloatingWindow = decoration,
                layer = layer,
                handleRightClickFunc = (mouseHandlers & MouseHandlers.RightClick) != default
                   ? Marshal.GetFunctionPointerForDelegate(_handleRightClickCallback)
                   : IntPtr.Zero
            };

            _id = DisplayAPI.CreateWindowEx(&parameters);

            // TODO: Register window in global context or plugin base
        }

        private WindowBase(WindowID windowId)
        {
            _id = windowId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the value indicating whether this window has keyboard focus.
        /// </summary>
        public bool HasKeyboardFocus => DisplayAPI.HasKeyboardFocus(_id) != 0;

        /// <summary>
        /// Gets the window ID.
        /// </summary>
        public WindowID Id => _id;

        /// <summary>
        /// Gets the value indicating whether the window is the frontmost
        /// visible window in its layer.
        /// </summary>
        /// <remarks>
        /// If you have a window at the front of the floating window layer
        /// (<see cref="WindowLayer.FloatingWindows"/>), this will return true even if there is a
        /// modal window (in layer <see cref="WindowLayer.Modal"/>) above you. (Not to worry,
        /// though: in such a case, X-Plane will not pass clicks or keyboard input down
        /// to your layer until the window above stops "eating" the input.)
        /// </remarks>
        public bool IsInFront => DisplayAPI.IsWindowInFront(_id) != 0;

        /// <summary>
        /// True if this window has been moved to the virtual reality (VR) headset,
        /// which in turn is true if and only if you have set the window's positioning
        /// mode to <see cref="WindowPositioningMode.VR"/>.
        /// </summary>
        public bool IsInVirtualReality => DisplayAPI.WindowIsInVR(_id) != 0;

        /// <summary>
        /// True if this window has been popped out (making it a first-class window in
        /// the operating system), which in turn is true if and only if you have set
        /// the window's positioning mode to <see cref="WindowPositioningMode.PopOut"/>.
        /// </summary>
        public bool IsPoppedOut => DisplayAPI.WindowIsPoppedOut(_id) != 0;

        /// <summary>
        /// Gets or sets the window visibility.
        /// </summary>
        public bool IsVisible
        {
            get => DisplayAPI.GetWindowIsVisible(_id) != 0;
            set => DisplayAPI.SetWindowIsVisible(_id, value ? 1 : 0);
        }

        /// <summary>
        /// Gets or sets the position and size of the windows.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If the window is popped out, the values are in the OS pixels.
        /// </para>
        /// <para>
        /// If the window is in virtual reality, the <see cref="Rect.Left"/> and <see cref="Rect.Bottom"/> are set to zero,
        /// and the size is in boxels.
        /// </para>
        /// <para>
        /// Otherwise, the values are in boxels.
        /// </para>
        /// </remarks>
        public unsafe Rect Geometry
        {
            get
            {
                int left, top, right, bottom;
                if (IsPoppedOut)
                {
                    DisplayAPI.GetWindowGeometryOS(_id, &left, &top, &right, &bottom);
                }
                else if (IsInVirtualReality)
                {
                    left = 0;
                    bottom = 0;
                    DisplayAPI.GetWindowGeometryVR(_id, &right, &top);
                }
                else
                {
                    DisplayAPI.GetWindowGeometry(_id, &left, &top, &right, &bottom);
                }
                return new Rect(left, top, right, bottom);
            }
            set
            {
                if (IsPoppedOut)
                {
                    DisplayAPI.SetWindowGeometryOS(_id, value.Left, value.Top, value.Right, value.Bottom);
                }
                else if (IsInVirtualReality)
                {
                    DisplayAPI.SetWindowGeometryVR(_id, value.Width, value.Height);
                }
                else
                {
                    DisplayAPI.SetWindowGeometry(_id, value.Left, value.Top, value.Right, value.Bottom);
                }
            }
        }

        /// <summary>
        /// Gets or sets the window title.
        /// </summary>
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                DisplayAPI.SetWindowTitle(_id, value);
            }
        }

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
        public static unsafe Rect ScreenBoundsGlobal
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
        /// </remarks>
        public static unsafe IReadOnlyDictionary<int, Rect> AllMonitorBoundsGlobal
        {
            get
            {
                var dict = new Dictionary<int, Rect>();
                var dictHandle = GCHandle.Alloc(dict);
                DisplayAPI.GetAllMonitorBoundsGlobal(Callback, GCHandle.ToIntPtr(dictHandle).ToPointer());
                return dict;

                static void Callback(int index, int left, int top, int right, int bottom, void* inrefcon)
                {
                    var dict = (Dictionary<int, Rect>) GCHandle.FromIntPtr(new IntPtr(inrefcon)).Target;
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
        public static unsafe IReadOnlyDictionary<int, Rect> AllMonitorBoundsOS
        {
            get
            {
                var dict = new Dictionary<int, Rect>();
                var dictHandle = GCHandle.Alloc(dict);
                DisplayAPI.GetAllMonitorBoundsOS(Callback, GCHandle.ToIntPtr(dictHandle).ToPointer());
                return dict;

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
        /// DisplayAPI. Thus, it can be used with both pop-out windows and secondary
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

        /// <summary>
        /// Gets the value indicating if XPlane itself has the keyboard focus, and thus no window has focus.
        /// </summary>
        public static bool XPlaneHasKeyboardFocus => DisplayAPI.HasKeyboardFocus(default) != 0;

        #endregion

        #region Public Methods

        /// <summary>
        /// This routine brings the window to the front of the Z-order for its layer.
        /// Windows are brought to the front automatically when they are created.
        /// Beyond that, you should make sure you are front before handling mouse
        /// clicks.
        /// </summary>
        /// <remarks>
        /// Note that this only brings your window to the front of its layer
        /// (<see cref="WindowLayer"/>). Thus, if you have a window in the floating window layer
        /// (<see cref="WindowLayer.FloatingWindows"/>), but there is a modal window (in layer
        /// <see cref="WindowLayer.Modal"/>) above you, you would still not be the true frontmost
        /// window after calling this. (After all, the window layers are strictly
        /// ordered, and no window in a lower layer can ever be above any window in a
        /// higher one.)
        /// </remarks>
        public void BringToFront() => DisplayAPI.BringWindowToFront(_id);

        /// <summary>
        /// Releases keyboard focus from this window.
        /// </summary>
        public void ReleaseKeyboardFocus()
        {
            if (HasKeyboardFocus)
            {
                RemoveKeyboardFocus();
            }
        }

        /// <summary>
        /// Set the window gravity.
        /// </summary>
        /// <remarks>
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
        /// </remarks>
        public void SetGravity(float left = 0, float top = 1, float right = 0, float bottom = 1) => DisplayAPI.SetWindowGravity(_id, left, top, right, bottom);


        /// <summary>
        /// Sets the policy for how X-Plane will position your window.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Some positioning modes apply to a particular monitor. For those modes, you
        /// can pass a negative monitor index to position the window on the main
        /// X-Plane monitor (the screen with the X-Plane menu bar at the top). Or, if
        /// you have a specific monitor you want to position your window on, you can
        /// pass a real monitor index as received from, e.g., <see cref="AllMonitorBoundsOS"/>.
        /// </para>
        /// </remarks>
        public void SetPositioningMode(WindowPositioningMode mode, int monitorIndex = -1) => DisplayAPI.SetWindowPositioningMode(_id, mode, monitorIndex);

        /// <summary>
        /// Sets the minimum and maximum size of the client rectangle of the given
        /// window. (That is, it does not include any window styling that you might
        /// have asked X-Plane to apply on your behalf.) All resizing operations are
        /// constrained to these sizes.
        /// </summary>
        public void SetResizingLimits(int minWidth, int minHeight, int maxWidth, int maxHeight) => 
            DisplayAPI.SetWindowResizingLimits(_id, minWidth, minHeight, maxWidth, maxHeight);

        /// <summary>
        /// Gives the keyboard focus to this window.
        /// </summary>
        public void TakeKeyboardFocus() => DisplayAPI.TakeKeyboardFocus(_id);

        /// <summary>
        /// Removes keyboard focus from currently focused window.
        /// </summary>
        public static void RemoveKeyboardFocus() => DisplayAPI.TakeKeyboardFocus(default);

        /// <summary>
        /// Gets the window with the specified <paramref name="id"/>.
        /// </summary>
        public static WindowBase FromId(WindowID id) => new ThirdPartyWindow(id);

        #endregion

        #region Event Handlers

        /// <summary>
        /// Override this method to handle drawing inside the window.
        /// </summary>
        protected virtual void OnDrawWindow()
        {
        }

        /// <summary>
        /// Override this method to handle mouse left button events.
        /// </summary>
        protected virtual bool OnMouseLeftButtonEvent(int x, int y, MouseStatus mouseStatus) => true;

        /// <summary>
        /// Override this method to handle mouse right button events.
        /// </summary>
        protected virtual bool OnMouseRightButtonEvent(int x, int y, MouseStatus mouseStatus) => true;

        /// <summary>
        /// Override this method to handle mouse wheel button events.
        /// </summary>
        protected virtual bool OnMouseWheelEvent(int x, int y, MouseWheel wheel, int clicks) => true;

        /// <summary>
        /// Override this method to handle keyboard events.
        /// </summary>
        protected virtual void OnKeyEvent(byte key, KeyFlags flags, byte virtualKey, bool losingFocus)
        {
        }

        /// <summary>
        /// Override this method to change the cursor depending on the mouse coordinates..
        /// </summary>
        protected virtual CursorStatus OnCursorRequested(int x, int y) => CursorStatus.Default;

        #endregion

        #region Disposal

        /// <summary>
        /// Frees the resources allocated for this window.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // If handle is not allocated, we don't own this windows, e.g. it has been received by FromID(id) method call. 
                if (_handle.IsAllocated)
                {
                    DisplayAPI.DestroyWindow(_id);
                    _handle.Free();
                }

                _id = default;
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 0)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        #endregion

        private sealed class ThirdPartyWindow : WindowBase
        {
            public ThirdPartyWindow(WindowID windowId) : base(windowId)
            {
            }
        }
    }
}
