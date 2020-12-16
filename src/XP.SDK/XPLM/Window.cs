using System;

namespace XP.SDK.XPLM
{
    /// <summary>
    /// Represents the modern (XPLM300) X-Plane window.
    /// </summary>
    public sealed class Window : WindowBase
    {
        /// <summary>
        /// Creates a new instance of Window.
        /// </summary>
        /// <param name="bounds">Window bounds in global desktop boxels.</param>
        /// <param name="visible">Window visibility.</param>
        /// <param name="layer">Window layer.</param>
        /// <param name="decoration">The type of X-Plane 11-style "wrapper" you want around your window, if any.</param>
        /// <param name="mouseHandlers">The mouse events, that the window must handle.</param>
        public Window(in Rect bounds, 
            bool visible = true, 
            WindowLayer layer = WindowLayer.FloatingWindows, 
            WindowDecoration decoration = WindowDecoration.None, 
            MouseHandlers mouseHandlers = MouseHandlers.All) 
            : base(in bounds, visible, layer, decoration, mouseHandlers)
        {
        }

        /// <inheritdoc />
        protected override void OnDrawWindow()
        {
            DrawWindow?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc />
        protected override bool OnMouseLeftButtonEvent(int x, int y, MouseStatus mouseStatus)
        {
            var args = new MouseButtonEventArgs(x, y, mouseStatus);
            MouseLeftButtonEvent?.Invoke(this, ref args);
            return !args.PassThrough;
        }

        /// <inheritdoc />
        protected override bool OnMouseRightButtonEvent(int x, int y, MouseStatus mouseStatus)
        {
            var args = new MouseButtonEventArgs(x, y, mouseStatus);
            MouseRightButtonEvent?.Invoke(this, ref args);
            return !args.PassThrough;

        }

        /// <inheritdoc />
        protected override bool OnMouseWheelEvent(int x, int y, MouseWheel wheel, int clicks)
        {
            var args = new MouseWheelEventArgs(x, y, wheel, clicks);
            MouseWheelEvent?.Invoke(this, ref args);
            return !args.PassThrough;

        }

        /// <inheritdoc />
        protected override void OnKeyEvent(byte key, KeyFlags flags, byte virtualKey, bool losingFocus)
        {
            var args = new KeyEventArgs(flags, key, virtualKey, losingFocus);
            KeyEvent?.Invoke(this, in args);
        }

        /// <inheritdoc />
        protected override CursorStatus OnCursorRequested(int x, int y)
        {
            var args = new CursorRequestEventArgs(x, y);
            CursorRequested?.Invoke(this, ref args);
            return args.Cursor;
        }

        /// <summary>
        /// Occurs when the window content must be drawn.
        /// </summary>
        public event TypedEventHandler<Window> DrawWindow;

        /// <summary>
        /// Occurs on left mouse button input event.
        /// </summary>
        public event RefStructEventHandler<Window, MouseButtonEventArgs> MouseLeftButtonEvent;

        /// <summary>
        /// Occurs on right mouse button input event.
        /// </summary>
        public event RefStructEventHandler<Window, MouseButtonEventArgs> MouseRightButtonEvent;

        /// <summary>
        /// Occurs on mouse wheel input event.
        /// </summary>
        public event RefStructEventHandler<Window, MouseWheelEventArgs> MouseWheelEvent;

        /// <summary>
        /// Occurs on keyboard input event.
        /// </summary>
        public event InStructEventHandler<Window, KeyEventArgs> KeyEvent;

        /// <summary>
        /// Occurs when the cursor for the current mouse coordinates is requested.
        /// </summary>
        public event RefStructEventHandler<Window, CursorRequestEventArgs> CursorRequested;
    }
}
