#nullable enable
using System;
using System.Runtime.CompilerServices;
using XP.SDK.Widgets.Internal;
using XP.SDK.XPLM;

namespace XP.SDK.Widgets.Behaviors
{
    /// <summary>
    /// Provides .Net events for all common widget messages.
    /// </summary>
    /// <seealso cref="WidgetExtensions"/>
    public sealed class CommonEventsBehavior : Behavior
    {
        /// <inheritdoc />
        protected override bool HandleMessage(WidgetMessage message, Widget widget, IntPtr param1, IntPtr param2)
        {
            bool handled = false;
            switch (message)
            {
                case WidgetMessage.Create:
                    Created?.Invoke(widget, param1 != default, ref handled);
                    break;
                case WidgetMessage.Destroy:
                    Destroyed?.Invoke(widget, param1 != default, ref handled);
                    break;
                case WidgetMessage.Paint:
                    Paint?.Invoke(widget, ref handled);
                    break;
                case WidgetMessage.Draw:
                    Draw?.Invoke(widget, ref handled);
                    break;
                case WidgetMessage.KeyPress:
                    KeyPressed?.Invoke(widget, ref AsRef<KeyState>(param1), ref handled);
                    break;
                case WidgetMessage.KeyTakeFocus:
                    TakingFocus?.Invoke(widget, param1 != default, ref handled);
                    break;
                case WidgetMessage.KeyLoseFocus:
                    LostFocus?.Invoke(widget, param1 != default, ref handled);
                    break;
                case WidgetMessage.MouseDown:
                    MouseDown?.Invoke(widget, ref AsRef<MouseState>(param1), ref handled);
                    break;
                case WidgetMessage.MouseDrag:
                    MouseDrag?.Invoke(widget, ref AsRef<MouseState>(param1), ref handled);
                    break;
                case WidgetMessage.MouseUp:
                    MouseUp?.Invoke(widget, ref AsRef<MouseState>(param1), ref handled);
                    break;
                case WidgetMessage.Reshape:
                    Reshape?.Invoke(widget, Widget.GetOrCreate(param1), ref AsRef<WidgetGeometryChange>(param2), ref handled);
                    break;
                case WidgetMessage.ExposedChanged:
                    ExposedChanged?.Invoke(widget, ref handled);
                    break;
                case WidgetMessage.AcceptChild:
                    ChildAdded?.Invoke(widget, Widget.GetOrCreate(param1), ref handled);
                    break;
                case WidgetMessage.LoseChild:
                    ChildRemoved?.Invoke(widget, Widget.GetOrCreate(param1), ref handled);
                    break;
                case WidgetMessage.AcceptParent:
                    ParentChanged?.Invoke(widget, param1 != default ? Widget.GetOrCreate(param1) : null, ref handled);
                    break;
                case WidgetMessage.Shown:
                    Shown?.Invoke(widget, Widget.GetOrCreate(param1), ref handled);
                    break;
                case WidgetMessage.Hidden:
                    Hidden?.Invoke(widget, Widget.GetOrCreate(param1), ref handled);
                    break;
                case WidgetMessage.DescriptorChanged:
                    DescriptorChanged?.Invoke(widget, ref handled);
                    break;
                case WidgetMessage.PropertyChanged:
                    PropertyChanged?.Invoke(widget, param1.ToInt32(), param2, ref handled);
                    break;
                case WidgetMessage.MouseWheel:
                    MouseWheel?.Invoke(widget, ref AsRef<MouseState>(param1), ref handled);
                    break;
                case WidgetMessage.CursorAdjust:
                    CursorAdjust?.Invoke(widget, ref AsRef<MouseState>(param1), ref AsRef<CursorStatus>(param2), ref handled);
                    break;
            }
            return handled;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe ref T AsRef<T>(IntPtr ptr) where T : unmanaged => ref Unsafe.AsRef<T>(ptr.ToPointer());

        public event WidgetCreatedEventHandler<Widget>? Created;

        public event WidgetDestroyedEventHandler<Widget>? Destroyed;

        public event WidgetEventHandler<Widget>? Paint;

        public event WidgetEventHandler<Widget>? Draw;

        public event WidgetRefEventHandler<Widget, KeyState>? KeyPressed;

        public event WidgetTakingFocusEventHandler<Widget>? TakingFocus;

        public event WidgetLostFocusEventHandler<Widget>? LostFocus;

        public event WidgetRefEventHandler<Widget, MouseState>? MouseUp;
        
        public event WidgetRefEventHandler<Widget, MouseState>? MouseDown;
        
        public event WidgetRefEventHandler<Widget, MouseState>? MouseDrag;

        public event WidgetRefEventHandler<Widget, MouseState>? MouseWheel;

        public event WidgetReshapeEventHandler<Widget>? Reshape;

        public event WidgetEventHandler<Widget>? ExposedChanged;

        public event WidgetValueEventHandler<Widget, Widget>? ChildAdded;
        
        public event WidgetValueEventHandler<Widget, Widget>? ChildRemoved;

        public event WidgetValueEventHandler<Widget, Widget?>? ParentChanged;

        public event WidgetValueEventHandler<Widget, Widget>? Shown;

        public event WidgetValueEventHandler<Widget, Widget>? Hidden;

        public event WidgetEventHandler<Widget>? DescriptorChanged;

        public event WidgetPropertyChangeEventHandler<Widget>? PropertyChanged;

        public event WidgetCursorAdjustEventHandler<Widget>? CursorAdjust;
    }
}
