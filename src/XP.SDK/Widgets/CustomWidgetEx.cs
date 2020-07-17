using System;
using System.Runtime.CompilerServices;
using XP.SDK.Widgets.Internal;
using XP.SDK.XPLM;

namespace XP.SDK.Widgets
{
    public abstract class CustomWidgetEx : CustomWidget
    {
        protected CustomWidgetEx(in Rect geometry, bool isVisible, string descriptor, bool isRoot, Widget? parent) : base(in geometry, isVisible, descriptor, isRoot, parent)
        {
        }

        protected override bool HandleMessage(WidgetMessage message, IntPtr param1, IntPtr param2) =>
            message switch
            {
                WidgetMessage.None => false,
                WidgetMessage.Create => OnCreated(param1 != default),
                WidgetMessage.Destroy => OnDestroyed(param1 != default),
                WidgetMessage.Paint => OnPaint(),
                WidgetMessage.Draw => OnDraw(),
                WidgetMessage.KeyPress => OnKeyPress(ref AsRef<KeyState>(param1)),
                WidgetMessage.KeyTakeFocus => OnTakingFocus(param1 != default),
                WidgetMessage.KeyLoseFocus => OnLoosingFocus(param1 != default),
                WidgetMessage.MouseDown => OnMouseDown(ref AsRef<MouseState>(param1)),
                WidgetMessage.MouseDrag => OnMouseDrag(ref AsRef<MouseState>(param1)),
                WidgetMessage.MouseUp => OnMouseUp(ref AsRef<MouseState>(param1)),
                WidgetMessage.Reshape => OnReshape(GetOrCreate(new WidgetID(param1)), ref AsRef<WidgetGeometryChange>(param2)),
                WidgetMessage.ExposedChanged => OnExposedChanged(),
                WidgetMessage.AcceptChild => OnChildAdded(GetOrCreate(new WidgetID(param1))),
                WidgetMessage.LoseChild => OnChildRemoved(GetOrCreate(new WidgetID(param1))),
                WidgetMessage.AcceptParent => OnParentChanged(param1 != default ? GetOrCreate(new WidgetID(param1)) : null),
                WidgetMessage.Shown => OnShown(GetOrCreate(new WidgetID(param1))),
                WidgetMessage.Hidden => OnHidden(GetOrCreate(new WidgetID(param1))),
                WidgetMessage.DescriptorChanged => OnDescriptorChanged(),
                WidgetMessage.PropertyChanged => OnPropertyChanged((WidgetPropertyID) param1.ToInt32(), param2),
                WidgetMessage.MouseWheel => OnMouseWheel(ref AsRef<MouseState>(param1)), 
                WidgetMessage.CursorAdjust => OnCursorAdjust(ref AsRef<MouseState>(param1), ref AsRef<CursorStatus>(param2)),
                _ => OnCustomMessage((int) message, param1, param2)
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe ref T AsRef<T>(IntPtr ptr) where T : unmanaged => ref Unsafe.AsRef<T>(ptr.ToPointer());

        protected virtual bool OnCreated(bool isSubclass) => false;

        protected virtual bool OnDestroyed(bool recursive) => false;

        protected virtual bool OnPaint() => false;

        protected virtual bool OnDraw() => false;

        protected virtual bool OnKeyPress(ref KeyState keyState) => false;

        protected virtual bool OnTakingFocus(bool fromChild) => false;

        protected virtual bool OnLoosingFocus(bool takenByOtherWidget) => false;

        protected virtual bool OnMouseDown(ref MouseState mouseState) => false;
        
        protected virtual bool OnMouseDrag(ref MouseState mouseState) => false;
        
        protected virtual bool OnMouseUp(ref MouseState mouseState) => false;

        protected virtual bool OnReshape(Widget originalWidget, ref WidgetGeometryChange change) => false;

        protected virtual bool OnExposedChanged() => false;

        protected virtual bool OnChildAdded(Widget child) => false;

        protected virtual bool OnChildRemoved(Widget child) => false;

        protected virtual bool OnParentChanged(Widget? parent) => false;

        protected virtual bool OnShown(Widget shownWidget) => false;

        protected virtual bool OnHidden(Widget hiddenWidget) => false;

        protected virtual bool OnDescriptorChanged() => false;

        protected virtual bool OnPropertyChanged(WidgetPropertyID property, IntPtr value) => false;

        protected virtual bool OnMouseWheel(ref MouseState mouseState) => false;

        protected virtual bool OnCursorAdjust(ref MouseState mouseState, ref CursorStatus cursorStatus) => false;

        protected virtual bool OnCustomMessage(int message, IntPtr param1, IntPtr param2) => false;
    }
}
