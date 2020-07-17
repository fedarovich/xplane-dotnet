#nullable enable
using System;
using System.Collections.Generic;
using System.Text;

namespace XP.SDK.Widgets
{
    public class ScrollBar : StandardWidget
    {
        public const int Class = 5;

        public ScrollBar(in Rect geometry, string descriptor = "", bool isVisible = true, Widget? parent = null, bool isRoot = false) 
            : base(in geometry, descriptor, isVisible, parent, isRoot, Class)
        {
        }

        public ScrollBarType Type
        {
            get => (ScrollBarType) GetProperty((int) ScrollBarProperty.Type);
            set => SetProperty((int) ScrollBarProperty.Type, (IntPtr) value);
        }

        public long Value
        {
            get => (long) GetProperty((int) ScrollBarProperty.SliderPosition);
            set => SetProperty((int) ScrollBarProperty.SliderPosition, new IntPtr(value));
        }

        public long MinValue
        {
            get => (long) GetProperty((int) ScrollBarProperty.Min);
            set => SetProperty((int) ScrollBarProperty.Min, new IntPtr(value));
        }

        public long MaxValue
        {
            get => (long) GetProperty((int) ScrollBarProperty.Max);
            set => SetProperty((int) ScrollBarProperty.Max, new IntPtr(value));
        }

        public long PageSize
        {
            get => (long) GetProperty((int) ScrollBarProperty.PageAmount);
            set => SetProperty((int) ScrollBarProperty.PageAmount, new IntPtr(value));
        }

        protected override bool HandleMessage(WidgetMessage message, IntPtr param1, IntPtr param2) =>
            (ScrollBarMessage) message switch
            {
                ScrollBarMessage.ScrollBarSliderPositionChanged => OnValueChanged(),
                _ => base.HandleMessage(message, param1, param2)
            };

        protected virtual bool OnValueChanged()
        {
            bool handled = false;
            ValueChanged?.Invoke(this, ref handled);
            return handled;
        }

        public event WidgetEventHandler<ScrollBar>? ValueChanged;
    }
}
