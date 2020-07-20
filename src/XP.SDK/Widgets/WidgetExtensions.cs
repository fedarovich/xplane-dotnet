#nullable enable
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using XP.SDK.Widgets.Behaviors;
using XP.SDK.Widgets.Internal;

namespace XP.SDK.Widgets
{
    public static class WidgetExtensions
    {
        /// <summary>
        /// Adds a <see cref="WidgetMessage.Create"/> message handler.
        /// </summary>
        public static TWidget AddCreatedHandler<TWidget>(this TWidget widget, WidgetCreatedEventHandler<Widget> handler)
            where TWidget : Widget
        {
            CheckNotNull(widget);
            widget.Behaviors.GetOrAdd<CommonEventsBehavior>().Created += handler;
            return widget;
        }

        /// <summary>
        /// Adds a <see cref="WidgetMessage.Destroy"/> message handler.
        /// </summary>
        public static TWidget AddDestroyedHandler<TWidget>(this TWidget widget, WidgetDestroyedEventHandler<Widget> handler)
            where TWidget : Widget
        {
            CheckNotNull(widget);
            widget.Behaviors.GetOrAdd<CommonEventsBehavior>().Destroyed += handler;
            return widget;
        }

        /// <summary>
        /// Adds a <see cref="WidgetMessage.Paint"/> message handler.
        /// </summary>
        public static TWidget AddPaintHandler<TWidget>(this TWidget widget, WidgetEventHandler<Widget> handler)
            where TWidget : Widget
        {
            CheckNotNull(widget);
            widget.Behaviors.GetOrAdd<CommonEventsBehavior>().Paint += handler;
            return widget;
        }

        /// <summary>
        /// Adds a <see cref="WidgetMessage.Draw"/> message handler.
        /// </summary>
        public static TWidget AddDrawHandler<TWidget>(this TWidget widget, WidgetEventHandler<Widget> handler)
            where TWidget : Widget
        {
            CheckNotNull(widget);
            widget.Behaviors.GetOrAdd<CommonEventsBehavior>().Draw += handler;
            return widget;
        }

        /// <summary>
        /// Adds a <see cref="WidgetMessage.KeyPress"/> message handler.
        /// </summary>
        public static TWidget AddKeyPressedHandler<TWidget>(this TWidget widget, WidgetRefEventHandler<Widget, KeyState> handler)
            where TWidget : Widget
        {
            CheckNotNull(widget);
            widget.Behaviors.GetOrAdd<CommonEventsBehavior>().KeyPressed += handler;
            return widget;
        }

        /// <summary>
        /// Adds a <see cref="WidgetMessage.KeyTakeFocus"/> message handler.
        /// </summary>
        public static TWidget AddTakingFocusHandler<TWidget>(this TWidget widget, WidgetTakingFocusEventHandler<Widget> handler)
            where TWidget : Widget
        {
            CheckNotNull(widget);
            widget.Behaviors.GetOrAdd<CommonEventsBehavior>().TakingFocus += handler;
            return widget;
        }

        /// <summary>
        /// Adds a <see cref="WidgetMessage.KeyLoseFocus"/> message handler.
        /// </summary>
        public static TWidget AddLostFocusHandler<TWidget>(this TWidget widget, WidgetLostFocusEventHandler<Widget>? handler)
            where TWidget : Widget
        {
            CheckNotNull(widget);
            widget.Behaviors.GetOrAdd<CommonEventsBehavior>().LostFocus += handler;
            return widget;
        }

        /// <summary>
        /// Adds a <see cref="WidgetMessage.MouseDown"/> message handler.
        /// </summary>
        public static TWidget AddMouseDownHandler<TWidget>(this TWidget widget, WidgetRefEventHandler<Widget, MouseState>? handler)
            where TWidget : Widget
        {
            CheckNotNull(widget);
            widget.Behaviors.GetOrAdd<CommonEventsBehavior>().MouseDown += handler;
            return widget;
        }

        /// <summary>
        /// Adds a <see cref="WidgetMessage.MouseUp"/> message handler.
        /// </summary>
        public static TWidget AddMouseUpHandler<TWidget>(this TWidget widget, WidgetRefEventHandler<Widget, MouseState>? handler)
            where TWidget : Widget
        {
            CheckNotNull(widget);
            widget.Behaviors.GetOrAdd<CommonEventsBehavior>().MouseUp += handler;
            return widget;
        }

        /// <summary>
        /// Adds a <see cref="WidgetMessage.MouseDrag"/> message handler.
        /// </summary>
        public static TWidget AddMouseDragHandler<TWidget>(this TWidget widget, WidgetRefEventHandler<Widget, MouseState>? handler)
            where TWidget : Widget
        {
            CheckNotNull(widget);
            widget.Behaviors.GetOrAdd<CommonEventsBehavior>().MouseDrag += handler;
            return widget;
        }

        /// <summary>
        /// Adds a <see cref="WidgetMessage.MouseWheel"/> message handler.
        /// </summary>
        public static TWidget AddMouseWheelHandler<TWidget>(this TWidget widget, WidgetRefEventHandler<Widget, MouseState>? handler)
            where TWidget : Widget
        {
            CheckNotNull(widget);
            widget.Behaviors.GetOrAdd<CommonEventsBehavior>().MouseWheel += handler;
            return widget;
        }

        /// <summary>
        /// Adds a <see cref="WidgetMessage.Reshape"/> message handler.
        /// </summary>
        public static TWidget AddReshapeHandler<TWidget>(this TWidget widget, WidgetReshapeEventHandler<Widget>? handler)
            where TWidget : Widget
        {
            CheckNotNull(widget);
            widget.Behaviors.GetOrAdd<CommonEventsBehavior>().Reshape += handler;
            return widget;
        }

        /// <summary>
        /// Adds a <see cref="WidgetMessage.ExposedChanged"/> message handler.
        /// </summary>
        public static TWidget AddExposedChangedHandler<TWidget>(this TWidget widget, WidgetEventHandler<Widget> handler)
            where TWidget : Widget
        {
            CheckNotNull(widget);
            widget.Behaviors.GetOrAdd<CommonEventsBehavior>().ExposedChanged += handler;
            return widget;
        }

        /// <summary>
        /// Adds a <see cref="WidgetMessage.AcceptChild"/> message handler.
        /// </summary>
        public static TWidget AddChildAddedHandler<TWidget>(this TWidget widget, WidgetValueEventHandler<Widget, Widget>? handler)
            where TWidget : Widget
        {
            CheckNotNull(widget);
            widget.Behaviors.GetOrAdd<CommonEventsBehavior>().ChildAdded += handler;
            return widget;
        }

        /// <summary>
        /// Adds a <see cref="WidgetMessage.LoseChild"/> message handler.
        /// </summary>
        public static TWidget AddChildRemovedHandler<TWidget>(this TWidget widget, WidgetValueEventHandler<Widget, Widget>? handler)
            where TWidget : Widget
        {
            CheckNotNull(widget);
            widget.Behaviors.GetOrAdd<CommonEventsBehavior>().ChildRemoved += handler;
            return widget;
        }

        /// <summary>
        /// Adds a <see cref="WidgetMessage.AcceptParent"/> message handler.
        /// </summary>
        public static TWidget AddParentChangedHandler<TWidget>(this TWidget widget, WidgetValueEventHandler<Widget, Widget?> handler)
            where TWidget : Widget
        {
            CheckNotNull(widget);
            widget.Behaviors.GetOrAdd<CommonEventsBehavior>().ParentChanged += handler;
            return widget;
        }

        /// <summary>
        /// Adds a <see cref="WidgetMessage.Shown"/> message handler.
        /// </summary>
        public static TWidget AddShownHandler<TWidget>(this TWidget widget, WidgetValueEventHandler<Widget, Widget> handler)
            where TWidget : Widget
        {
            CheckNotNull(widget);
            widget.Behaviors.GetOrAdd<CommonEventsBehavior>().Shown += handler;
            return widget;
        }

        /// <summary>
        /// Adds a <see cref="WidgetMessage.Hidden"/> message handler.
        /// </summary>
        public static TWidget AddHiddenHandler<TWidget>(this TWidget widget, WidgetValueEventHandler<Widget, Widget> handler)
            where TWidget : Widget
        {
            CheckNotNull(widget);
            widget.Behaviors.GetOrAdd<CommonEventsBehavior>().Hidden += handler;
            return widget;
        }

        /// <summary>
        /// Adds a <see cref="WidgetMessage.DescriptorChanged"/> message handler.
        /// </summary>
        public static TWidget AddDescriptorChangedHandler<TWidget>(this TWidget widget, WidgetEventHandler<Widget> handler)
            where TWidget : Widget
        {
            CheckNotNull(widget);
            widget.Behaviors.GetOrAdd<CommonEventsBehavior>().DescriptorChanged += handler;
            return widget;
        }

        /// <summary>
        /// Adds a <see cref="WidgetMessage.PropertyChanged"/> message handler.
        /// </summary>
        public static TWidget AddPropertyChangedHandler<TWidget>(this TWidget widget, WidgetPropertyChangeEventHandler<Widget> handler)
            where TWidget : Widget
        {
            CheckNotNull(widget);
            widget.Behaviors.GetOrAdd<CommonEventsBehavior>().PropertyChanged += handler;
            return widget;
        }

        /// <summary>
        /// Adds a <see cref="WidgetMessage.CursorAdjust"/> message handler.
        /// </summary>
        public static TWidget AddCursorAdjustHandler<TWidget>(this TWidget widget, WidgetCursorAdjustEventHandler<Widget> handler)
            where TWidget : Widget
        {
            CheckNotNull(widget);
            widget.Behaviors.GetOrAdd<CommonEventsBehavior>().CursorAdjust += handler;
            return widget;
        }

        /// <summary>
        /// Adds a <see cref="ButtonMessage.PushButtonPressed"/> message handler.
        /// </summary>
        public static TWidget AddPressedHandler<TWidget>(this TWidget widget, WidgetEventHandler<Button> handler)
            where TWidget : Button
        {
            CheckNotNull(widget);
            widget.Pressed += handler;
            return widget;
        }

        /// <summary>
        /// Adds a <see cref="ButtonMessage.ButtonStateChanged"/> message handler.
        /// </summary>
        public static TWidget AddIsCheckedChangedHandler<TWidget>(this TWidget widget, WidgetValueEventHandler<Button, bool> handler)
            where TWidget : Button
        {
            CheckNotNull(widget);
            widget.IsCheckedChanged += handler;
            return widget;
        }

        /// <summary>
        /// Adds a <see cref="MainWindowMessage.CloseButtonPushed"/> message handler.
        /// </summary>
        public static TWidget AddCloseButtonPushedHandler<TWidget>(this TWidget widget, WidgetEventHandler<MainWindow> handler)
            where TWidget : MainWindow
        {
            CheckNotNull(widget);
            widget.CloseButtonPushed += handler;
            return widget;
        }

        /// <summary>
        /// Adds a <see cref="ScrollBarMessage.ScrollBarSliderPositionChanged"/> message handler.
        /// </summary>
        public static TWidget AddValueChangedHandler<TWidget>(this TWidget widget, WidgetEventHandler<ScrollBar> handler)
            where TWidget : ScrollBar
        {
            CheckNotNull(widget);
            widget.ValueChanged += handler;
            return widget;
        }

        /// <summary>
        /// Adds a <see cref="TextFieldMessage.TextFieldChanged"/> message handler.
        /// </summary>
        public static TWidget AddTextChangedHandler<TWidget>(this TWidget widget, WidgetEventHandler<TextField> handler)
            where TWidget : TextField
        {
            CheckNotNull(widget);
            widget.TextChanged += handler;
            return widget;
        }

        /// <summary>
        /// Adds a <see cref="WidgetMessage.KeyPress"/> message handler.
        /// </summary>
        public static TWidget AddKeyPressedHandler<TWidget>(this TWidget widget, WidgetRefEventHandler<TextField, KeyState> handler)
            where TWidget : TextField
        {
            CheckNotNull(widget);
            widget.KeyPressed += handler;
            return widget;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private static void CheckNotNull(Widget widget)
        {
            if (widget == null)
                throw new ArgumentNullException(nameof(widget));
        }
    }
}
