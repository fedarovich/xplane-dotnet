using System;
using System.Collections.Generic;
using System.Text;
using XP.SDK.XPLM.Internal;

namespace XP.SDK.XPLM
{
    public sealed class Window : WindowBase
    {
        public Window(in Rect rect, 
            bool visible = true, 
            WindowLayer layer = WindowLayer.FloatingWindows, 
            WindowDecoration decoration = WindowDecoration.None, 
            MouseHandlers mouseHandlers = MouseHandlers.All) 
            : base(in rect, visible, layer, decoration, mouseHandlers)
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

        public event EventHandler DrawWindow;

        public event RefStructEventHandler<MouseButtonEventArgs> MouseLeftButtonEvent;
        
        public event RefStructEventHandler<MouseButtonEventArgs> MouseRightButtonEvent;

        public event RefStructEventHandler<MouseWheelEventArgs> MouseWheelEvent;

        public event InStructEventHandler<KeyEventArgs> KeyEvent;

        public event RefStructEventHandler<CursorRequestEventArgs> CursorRequested;
    }
}
