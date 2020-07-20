#nullable enable
using System;

namespace XP.SDK.Widgets
{
    /// <summary>
    /// A standard scroll bar or slider control. The scroll bar has a minimum,
    /// maximum and current value that is updated when the user drags it.The
    /// scroll bar sends continuous messages as it is dragged.
    /// </summary>
    public class ScrollBar : StandardWidget
    {
        /// <summary>
        /// The scroll bar class.
        /// </summary>
        public const int Class = 5;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollBar"/> class.
        /// </summary>
        /// <param name="geometry">The widget geometry.</param>
        /// <param name="descriptor">The widget descriptor.</param>
        /// <param name="isVisible">The widget visibility.</param>
        /// <param name="parent">The parent widget.</param>
        /// <param name="isRoot">The value indicating whether this widget is a root one.</param>
        public ScrollBar(in Rect geometry, string descriptor = "", bool isVisible = true, Widget? parent = null, bool isRoot = false) 
            : base(in geometry, descriptor, isVisible, parent, isRoot, Class)
        {
        }

        /// <summary>
        /// Gets or sets the type of the scroll bar.
        /// </summary>
        public ScrollBarType Type
        {
            get => (ScrollBarType) GetProperty((int) ScrollBarProperty.Type);
            set => SetProperty((int) ScrollBarProperty.Type, (IntPtr) value);
        }

        /// <summary>
        /// Gets or sets the current value.
        /// </summary>
        public long Value
        {
            get => (long) GetProperty((int) ScrollBarProperty.SliderPosition);
            set => SetProperty((int) ScrollBarProperty.SliderPosition, new IntPtr(value));
        }

        /// <summary>
        /// Gets or sets the minimum value.
        /// </summary>
        public long MinValue
        {
            get => (long) GetProperty((int) ScrollBarProperty.Min);
            set => SetProperty((int) ScrollBarProperty.Min, new IntPtr(value));
        }

        /// <summary>
        /// Gets or sets the maximum value.
        /// </summary>
        public long MaxValue
        {
            get => (long) GetProperty((int) ScrollBarProperty.Max);
            set => SetProperty((int) ScrollBarProperty.Max, new IntPtr(value));
        }

        /// <summary>
        /// Gets or sets how many units to move the scroll bar when clicking next to the thumb.
        /// </summary>
        public long PageSize
        {
            get => (long) GetProperty((int) ScrollBarProperty.PageAmount);
            set => SetProperty((int) ScrollBarProperty.PageAmount, new IntPtr(value));
        }

        /// <inheritdoc />
        protected override bool HandleMessage(WidgetMessage message, IntPtr param1, IntPtr param2) =>
            (ScrollBarMessage) message switch
            {
                ScrollBarMessage.ScrollBarSliderPositionChanged => OnValueChanged(),
                _ => base.HandleMessage(message, param1, param2)
            };

        /// <summary>
        /// Handles the <see cref="ScrollBarMessage.ScrollBarSliderPositionChanged"/> message.
        /// </summary>
        /// <returns><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</returns>
        protected virtual bool OnValueChanged()
        {
            bool handled = false;
            ValueChanged?.Invoke(this, ref handled);
            return handled;
        }

        /// <summary>
        /// Raised when the slider position changes.
        /// </summary>
        public event WidgetEventHandler<ScrollBar>? ValueChanged;
    }
}
