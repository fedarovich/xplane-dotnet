using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using XP.SDK.XPLM.Internal;

namespace XP.SDK.XPLM
{
    public abstract class WindowBase : IDisposable
    {
        private static readonly DrawWindowCallback _drawWindowCallback;
        private static readonly HandleMouseClickCallback _handleLeftClickCallback;
        private static readonly HandleMouseClickCallback _handleRightClickCallback;
        private static readonly HandleMouseWheelCallback _handleMouseWheelCallback;
        private static readonly HandleKeyCallback _handleKeyCallback;
        private static readonly HandleCursorCallback _handleCursorCallback;

        private int _disposed;
        private GCHandle _handle;
        private WindowID _id;
        private string _title;

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

        public unsafe WindowBase(in Rect rect, bool visible,
            WindowLayer layer = WindowLayer.FloatingWindows,
            WindowDecoration decoration = WindowDecoration.None,
            MouseHandlers mouseHandlers = MouseHandlers.All)
        {
            _handle = GCHandle.Alloc(this);
            
            var parameters = new CreateWindow
            {
                structSize = Unsafe.SizeOf<CreateWindow>(),
                left = rect.Left,
                top = rect.Top,
                right = rect.Right,
                bottom = rect.Bottom,
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

            _id = Display.CreateWindowEx(&parameters);

            // TODO: Register window in global context or plugin base
        }

        public bool HasKeyboardFocus => Display.HasKeyboardFocus(_id) != 0;

        public WindowID Id => _id;

        public bool IsInFront => Display.IsWindowInFront(_id) != 0;

        public bool IsInVirtualReality => Display.WindowIsInVR(_id) != 0;

        public bool IsPoppedOut => Display.WindowIsPoppedOut(_id) != 0;

        public bool IsVisible
        {
            get => Display.GetWindowIsVisible(_id) != 0;
            set => Display.SetWindowIsVisible(_id, value ? 1 : 0);
        }

        public unsafe Rect Geometry
        {
            get
            {
                int left, top, right, bottom;
                if (IsPoppedOut)
                {
                    Display.GetWindowGeometryOS(_id, &left, &top, &right, &bottom);
                }
                else if (IsInVirtualReality)
                {
                    left = 0;
                    top = 0;
                    Display.GetWindowGeometryVR(_id, &right, &bottom);
                }
                else
                {
                    Display.GetWindowGeometry(_id, &left, &top, &right, &bottom);
                }
                return new Rect(left, top, right, bottom);
            }
            set
            {
                if (IsPoppedOut)
                {
                    Display.SetWindowGeometryOS(_id, value.Left, value.Top, value.Right, value.Bottom);
                }
                else if (IsInVirtualReality)
                {
                    Display.SetWindowGeometryVR(_id, value.Width, value.Height);
                }
                else
                {
                    Display.SetWindowGeometry(_id, value.Left, value.Top, value.Right, value.Bottom);
                }
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                Display.SetWindowTitle(_id, value);
            }
        }

        public void BringToFront() => Display.BringWindowToFront(_id);

        public void ReleaseKeyboardFocus() => Display.TakeKeyboardFocus(default);

        public void SetGravity(float left = 0, float top = 1, float right = 0, float bottom = 1) => Display.SetWindowGravity(_id, left, top, right, bottom);
        
        public void SetPositioningMode(WindowPositioningMode mode, int monitorIndex = -1) => Display.SetWindowPositioningMode(_id, mode, monitorIndex);

        public void SetResizingLimits(int minWidth, int minHeight, int maxWidth, int maxHeight) => 
            Display.SetWindowResizingLimits(_id, minWidth, minHeight, maxWidth, maxHeight);

        public void TakeKeyboardFocus() => Display.TakeKeyboardFocus(_id);

        protected virtual void OnDrawWindow()
        {
        }

        protected virtual bool OnMouseLeftButtonEvent(int x, int y, MouseStatus mouseStatus) => true;

        protected virtual bool OnMouseRightButtonEvent(int x, int y, MouseStatus mouseStatus) => true;

        protected virtual bool OnMouseWheelEvent(int x, int y, MouseWheel wheel, int clicks) => true;

        protected virtual void OnKeyEvent(byte key, KeyFlags flags, byte virtualKey, bool losingFocus)
        {
        }

        protected virtual CursorStatus OnCursorRequested(int x, int y) => CursorStatus.Default;

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Display.DestroyWindow(_id);
                _id = default;
                _handle.Free();
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
    }
}
