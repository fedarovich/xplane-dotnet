#nullable enable
using System;

namespace XP.SDK.Widgets
{
    /// <summary>
    /// The main window widget class provides a "window" as the user knows it.
    /// These windows are draggable and can be selected. Use them to create floating
    /// windows and non-modal dialogs.
    /// </summary>
    public class MainWindow : StandardWidget
    {
        /// <summary>
        /// The main window class.
        /// </summary>
        public const int Class = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        /// <param name="geometry">The widget geometry.</param>
        /// <param name="descriptor">The widget descriptor.</param>
        /// <param name="isVisible">The widget visibility.</param>
        public MainWindow(in Rect geometry, bool isVisible = true, string descriptor = "") 
            : base(in geometry, descriptor, isVisible, null, true, Class)
        {
        }

        /// <summary>
        /// Gets or sets the main window type.
        /// </summary>
        public MainWindowType Type
        {
            get => (MainWindowType) GetProperty((int) MainWindowProperty.Type);
            set => SetProperty((int) MainWindowProperty.Type, (IntPtr) value);
        }

        /// <summary>
        /// Gets or sets the value indicating whether the main window has close boxes in its corners.
        /// </summary>
        public bool HasCloseBoxes
        {
            get => GetProperty((int) MainWindowProperty.HasCloseBoxes) != IntPtr.Zero;
            set => SetProperty((int) MainWindowProperty.HasCloseBoxes, new IntPtr(value.ToInt()));
        }

        /// <inheritdoc />
        protected override bool HandleMessage(WidgetMessage message, IntPtr param1, IntPtr param2) =>
            (MainWindowMessage) message switch
            {
                MainWindowMessage.CloseButtonPushed => OnCloseButtonPushed(),
                _ => base.HandleMessage(message, param1, param2)
            };

        /// <summary>
        /// Handles the <see cref="MainWindowMessage.CloseButtonPushed"/> message.
        /// </summary>
        /// <returns><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</returns>
        protected virtual bool OnCloseButtonPushed()
        {
            bool result = false;
            CloseButtonPushed?.Invoke(this, ref result);
            return result;
        }

        /// <summary>
        /// Raised when the close buttons are pressed for your window. 
        /// </summary>
        public event WidgetEventHandler<MainWindow>? CloseButtonPushed;
    }
}
