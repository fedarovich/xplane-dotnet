#nullable enable
using System;
using System.Collections.Generic;
using System.Text;

namespace XP.SDK.Widgets
{
    public class MainWindow : StandardWidget
    {
        public const int Class = 1;

        public const int CloseButtonPushedMessage = 1200;

        public const int HasCloseBoxesProperty = 1200;

        public MainWindow(in Rect geometry, bool isVisible = true, string descriptor = "") 
            : base(in geometry, isVisible, descriptor, true, null, Class)
        {
        }

        public bool HasCloseBoxes
        {
            get => GetProperty(HasCloseBoxesProperty) != IntPtr.Zero;
            set => SetProperty(HasCloseBoxesProperty, new IntPtr(value.ToInt()));
        }

        protected override bool HandleMessage(WidgetMessage message, IntPtr param1, IntPtr param2)
        {
            switch (message)
            {
                case (WidgetMessage) CloseButtonPushedMessage:
                    bool result = false;
                    CloseButtonPushed?.Invoke(this, ref result);
                    return result;
                default:
                    return base.HandleMessage(message, param1, param2);
            }
        }

        public event RefStructEventHandler<MainWindow, bool>? CloseButtonPushed;
    }
}
