#nullable enable
using System;
using System.Collections.Generic;
using System.Text;

namespace XP.SDK.Widgets
{
    public class Button : StandardWidget
    {
        public const int Class = 2;

        public Button(in Rect geometry, string descriptor = "", bool isVisible = true, Widget? parent = null, bool isRoot = false) 
            : base(in geometry, descriptor, isVisible, parent, isRoot, Class)
        {
        }

        public virtual ButtonType Type
        {
            get => (ButtonType) GetProperty((int) ButtonProperty.Type);
            set => SetProperty((int) ButtonProperty.Type, (IntPtr) value);
        }

        public virtual ButtonBehavior ButtonBehavior
        {
            get => (ButtonBehavior) GetProperty((int) ButtonProperty.Behavior);
            set => SetProperty((int) ButtonProperty.Behavior, (IntPtr) value);
        }

        public bool IsChecked
        {
            get => GetProperty((int) ButtonProperty.State) != default;
            set => SetProperty((int) ButtonProperty.State, new IntPtr(value.ToInt()));
        }

        protected override bool HandleMessage(WidgetMessage message, IntPtr param1, IntPtr param2) =>
            (ButtonMessage) message switch
            {
                ButtonMessage.PushButtonPressed => OnPressed(),
                ButtonMessage.ButtonStateChanged => OnIsCheckedChanged(param2 != default),
                _ => base.HandleMessage(message, param1, param2)
            };

        protected virtual bool OnPressed()
        {
            bool handled = false;
            Pressed?.Invoke(this, ref handled);
            return handled;
        }

        protected virtual bool OnIsCheckedChanged(bool isChecked)
        {
            bool handled = false;
            IsCheckedChanged?.Invoke(this, ref handled, isChecked);
            return handled;
        }

        public event WidgetEventHandler<Button>? Pressed;

        public event WidgetValueEventHandler<Button, bool>? IsCheckedChanged;
    }
}
