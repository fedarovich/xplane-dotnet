#nullable enable
using System;

namespace XP.SDK.Widgets
{
    /// <summary>
    /// Represents a button, check box or radio button.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The button class provides a number of different button styles and
    /// behaviors, including push buttons, radio buttons, check boxes, etc. The
    /// button label appears on or next to the button depending on the button's
    /// appearance, or type.
    /// </para>
    /// <para>
    /// The button's behavior is a separate property that dictates who it hilights
    /// and what kinds of messages it sends. Since behavior and type are different,
    /// you can do strange things like make check boxes that act as push buttons or
    /// push buttons with radio button behavior.
    /// </para>
    /// </remarks>
    public class Button : StandardWidget
    {
        /// <summary>
        /// The button class.
        /// </summary>
        public const int Class = 2;

        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class.
        /// </summary>
        /// <param name="geometry">The widget geometry.</param>
        /// <param name="descriptor">The widget descriptor.</param>
        /// <param name="isVisible">The widget visibility.</param>
        /// <param name="parent">The parent widget.</param>
        /// <param name="isRoot">The value indicating whether this widget is a root one.</param>
        public Button(in Rect geometry, string descriptor = "", bool isVisible = true, Widget? parent = null, bool isRoot = false) 
            : base(in geometry, descriptor, isVisible, parent, isRoot, Class)
        {
        }

        /// <summary>
        /// Gets or sets the button type.
        /// </summary>
        public virtual ButtonType Type
        {
            get => (ButtonType) GetProperty((int) ButtonProperty.Type);
            set => SetProperty((int) ButtonProperty.Type, (IntPtr) value);
        }

        /// <summary>
        /// Gets or sets the button behavior.
        /// </summary>
        public virtual ButtonBehavior ButtonBehavior
        {
            get => (ButtonBehavior) GetProperty((int) ButtonProperty.Behavior);
            set => SetProperty((int) ButtonProperty.Behavior, (IntPtr) value);
        }

        /// <summary>
        /// Gets or sets the value indicating whether the button is checked.
        /// </summary>
        public bool IsChecked
        {
            get => GetProperty((int) ButtonProperty.State) != default;
            set => SetProperty((int) ButtonProperty.State, new IntPtr(value.ToInt()));
        }

        /// <inheritdoc />
        protected override bool HandleMessage(WidgetMessage message, IntPtr param1, IntPtr param2) =>
            (ButtonMessage) message switch
            {
                ButtonMessage.PushButtonPressed => OnPressed(),
                ButtonMessage.ButtonStateChanged => OnIsCheckedChanged(param2 != default),
                _ => base.HandleMessage(message, param1, param2)
            };

        /// <summary>
        /// Handles the <see cref="ButtonMessage.PushButtonPressed"/> message.
        /// </summary>
        /// <returns><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</returns>
        protected virtual bool OnPressed()
        {
            bool handled = false;
            Pressed?.Invoke(this, ref handled);
            return handled;
        }

        /// <summary>
        /// Handles the <see cref="ButtonMessage.ButtonStateChanged"/> message.
        /// </summary>
        /// <returns><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</returns>
        protected virtual bool OnIsCheckedChanged(bool isChecked)
        {
            bool handled = false;
            IsCheckedChanged?.Invoke(this, isChecked, ref handled);
            return handled;
        }

        /// <summary>
        /// Raised when a user completes a click and release in a button with <see cref="Widgets.ButtonBehavior.PushButton"/> button behavior.
        /// </summary>
        public event WidgetEventHandler<Button>? Pressed;

        /// <summary>
        /// Raised when a button that has <see cref="Widgets.ButtonBehavior.CheckBox"/> or <see cref="Widgets.ButtonBehavior.RadioButton"/> behavior
        /// is click and its value changes.
        /// </summary>
        /// <remarks>
        /// If the value changes by setting <see cref="IsChecked"/> property, this event won't be raised.
        /// </remarks>
        public event WidgetValueEventHandler<Button, bool>? IsCheckedChanged;
    }
}
