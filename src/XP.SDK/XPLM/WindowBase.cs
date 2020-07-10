using System;
using System.Diagnostics.CodeAnalysis;
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
                (Utils.TryGetObject<WindowBase>(inrefcon)?.OnMouseLeftButtonEvent(x, y, inmouse) == true).ToInt();

            static int HandleMouseRightClick(WindowID inwindowid, int x, int y, MouseStatus inmouse, void* inrefcon) =>
                (Utils.TryGetObject<WindowBase>(inrefcon)?.OnMouseRightButtonEvent(x, y, inmouse) == true).ToInt();

            static int HandleMouseWheel(WindowID inwindowid, int x, int y, int wheel, int clicks, void* inrefcon) =>
                (Utils.TryGetObject<WindowBase>(inrefcon)?.OnMouseWheelEvent(x, y, (MouseWheel)wheel, clicks) == true).ToInt();

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
                visible = visible.ToInt(),
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
            set => DisplayAPI.SetWindowIsVisible(_id, value.ToInt());
        }

        /// <summary>
        /// <para>Gets or sets the position and size of the windows.</para>
        /// <para>The units are global desktop boxels.</para>
        /// </summary>
        /// <remarks>
        ///  <para>
        /// Note that the setter only applies to "floating" windows (that is, windows that
        /// are drawn within the X-Plane simulation windows, rather than being "popped
        /// out" into their own first-class operating system windows). To set the
        /// position of windows whose positioning mode is <see cref="WindowPositioningMode.PopOut"/>, you'll
        /// need to instead set <see cref="GeometryOS"/>.
        /// </para>
        /// </remarks>
        public unsafe Rect Geometry
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                int left, top, right, bottom;
                DisplayAPI.GetWindowGeometry(_id, &left, &top, &right, &bottom);
                return new Rect(left, top, right, bottom);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                DisplayAPI.SetWindowGeometry(_id, value.Left, value.Top, value.Right, value.Bottom);
            }
        }

        /// <summary>
        /// <para>
        /// Gets or sets the position and size of a "popped out" window
        /// (i.e. a window whose positioning mode is <see cref="WindowPositioningMode.PopOut"/>).
        /// </para>
        /// <para>The unites are in operating system pixels.</para>
        /// </summary>
        /// <remarks>
        /// <para>
        /// If the window is not popped out, the property returns an empty rectangle.
        /// </para>
        /// <para>
        /// Note that when setting the value, you are responsible for ensuring that a monitor really exists at the
        /// OS coordinates you provide (using XPLMGetAllMonitorBoundsOS()).
        /// </para>
        /// </remarks>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public unsafe Rect GeometryOS
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (!IsPoppedOut)
                    return default;

                int left, top, right, bottom;
                DisplayAPI.GetWindowGeometryOS(_id, &left, &top, &right, &bottom);
                return new Rect(left, top, right, bottom);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (IsPoppedOut)
                {
                    DisplayAPI.SetWindowGeometryOS(_id, value.Left, value.Top, value.Right, value.Bottom);
                }
            }
        }

        /// <summary>
        /// Gets or sets the size, in boxels, of a window in VR.
        /// </summary>
        /// <remarks>
        /// <para>If the window is not in virtual reality, the property returns a zero size.</para>
        /// </remarks>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public unsafe Size GeometryVR
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (!IsInVirtualReality)
                    return default;

                int width, height;
                DisplayAPI.GetWindowGeometryVR(_id, &width, &height);
                return new Size(width, height);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (IsInVirtualReality)
                {
                    DisplayAPI.SetWindowGeometryVR(_id, value.Width, value.Height);
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
        /// Returns the current mouse location in global desktop boxels.
        /// </summary>
        /// <remarks>
        /// The bottom left of the main X-Plane window is not
        /// guaranteed to be (0, 0). Instead, the origin is the lower left of the
        /// entire global desktop space. In addition, this routine gives the real mouse
        /// location when the mouse goes to X-Plane windows other than the primary
        /// display. Thus, it can be used with both pop-out windows and secondary
        /// </remarks>
        public (int X, int Y) MouseLocationGlobal
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Screen.MouseLocationGlobal;
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
