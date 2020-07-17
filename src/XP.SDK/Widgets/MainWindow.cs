#nullable enable
using System;
using System.Collections.Generic;
using System.Text;

namespace XP.SDK.Widgets
{
    public class MainWindow : StandardWidget
    {
        public const int Class = 1;

        public MainWindow(in Rect geometry, bool isVisible = true, string descriptor = "") 
            : base(in geometry, descriptor, isVisible, null, true, Class)
        {
        }

        public MainWindowType Type
        {
            get => (MainWindowType) GetProperty((int) MainWindowProperty.Type);
            set => SetProperty((int) MainWindowProperty.Type, (IntPtr) value);
        }

        public bool HasCloseBoxes
        {
            get => GetProperty((int) MainWindowProperty.HasCloseBoxes) != IntPtr.Zero;
            set => SetProperty((int) MainWindowProperty.HasCloseBoxes, new IntPtr(value.ToInt()));
        }

        protected override bool HandleMessage(WidgetMessage message, IntPtr param1, IntPtr param2) =>
            (MainWindowMessage) message switch
            {
                MainWindowMessage.CloseButtonPushed => OnCloseButtonPushed(),
                _ => base.HandleMessage(message, param1, param2)
            };

        protected virtual bool OnCloseButtonPushed()
        {
            bool result = false;
            CloseButtonPushed?.Invoke(this, ref result);
            return result;
        }

        public event WidgetEventHandler<MainWindow>? CloseButtonPushed;
    }
}
