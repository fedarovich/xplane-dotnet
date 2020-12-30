#nullable enable
using System;
using System.Runtime.CompilerServices;
using XP.SDK.Widgets.Interop;
using XP.SDK.XPLM;

namespace XP.SDK.Widgets
{
    /// <summary>
    /// The text field widget provides an editable text field including mouse
    /// selection and keyboard navigation.The contents of the text field are its
    /// <see cref="Widget.Descriptor"/>.
    /// </summary>
    /// <remarks>
    /// <para>The descriptor changes as the user types.</para>
    /// <para>
    /// The text field can have a number of types, that effect the visual layout of
    /// the text field.The text field sends messages to itself so you may control
    /// its behavior.
    /// </para>
    /// <para>
    /// If you need to filter keystrokes, subscribe to <see cref="KeyPressed"/>.
    /// Since key presses are passed by reference, you can modify the
    /// keystroke and pass it through to the text field widget.
    /// </para>
    /// </remarks>
    public class TextField : StandardWidget
    {
        /// <summary>
        /// The text field class.
        /// </summary>
        public const int Class = 4;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextField"/> class.
        /// </summary>
        /// <param name="geometry">The widget geometry.</param>
        /// <param name="descriptor">The widget descriptor.</param>
        /// <param name="isVisible">The widget visibility.</param>
        /// <param name="parent">The parent widget.</param>
        /// <param name="isRoot">The value indicating whether this widget is a root one.</param>
        public TextField(in Rect geometry, string descriptor = "", bool isVisible = true, Widget? parent = null, bool isRoot = false) 
            : base(in geometry, descriptor, isVisible, parent, isRoot, Class)
        {
        }

        /// <summary>
        /// Gets or sets the text field type.
        /// </summary>
        public TextFieldType Type
        {
            get => (TextFieldType) GetProperty((int) TextFieldProperty.TextFieldType);
            set => SetProperty((int) TextFieldProperty.TextFieldType, (IntPtr) value);
        }

        /// <summary>
        /// Gets or sets the character position the selection starts at, zero based.
        /// </summary>
        /// <remarks>
        /// If it is the same as the end insertion point, the insertion point is not a selection.
        /// </remarks>
        /// <seealso cref="SelectionEnd"/>
        public int SelectionStart
        {
            get => (int) GetProperty((int) TextFieldProperty.EditFieldSelStart);
            set => SetProperty((int) TextFieldProperty.EditFieldSelStart, new IntPtr(value));
        }

        /// <summary>
        /// Gets or sets the character position of the end of the selection.
        /// </summary>
        /// <seealso cref="SelectionStart"/>
        public int SelectionEnd
        {
            get => (int) GetProperty((int) TextFieldProperty.EditFieldSelEnd);
            set => SetProperty((int) TextFieldProperty.EditFieldSelEnd, new IntPtr(value));
        }

        /// <summary>
        /// Gets the character position a drag was started at if the user is dragging to select text, or -1 if a drag is not in progress.
        /// </summary>
        public int SelectionDragStart => (int) GetProperty((int) TextFieldProperty.EditFieldSelDragStart);

        /// <summary>
        /// Gets the value indicating whether this text field is in password mode.
        /// </summary>
        /// <remarks>
        /// In the password mode the characters will be drawn as *s even though the descriptor will contain plain-text.
        /// </remarks>
        public bool IsPasswordMode
        {
            get => GetProperty((int) TextFieldProperty.PasswordMode) != default;
            set => SetProperty((int) TextFieldProperty.PasswordMode, new IntPtr(value.ToInt()));
        }

        /// <summary>
        /// Gets or sets the max number of characters you can enter, if limited. Zero means unlimited.
        /// </summary>
        public int MaxLength
        {
            get => (int) GetProperty((int) TextFieldProperty.MaxCharacters);
            set => SetProperty((int) TextFieldProperty.MaxCharacters, new IntPtr(value));
        }

        /// <summary>
        /// Gets or sets the first visible character on the left. This effectively scrolls the text field.
        /// </summary>
        public int ScrollPosition
        {
            get => (int) GetProperty((int) TextFieldProperty.ScrollPosition);
            set => SetProperty((int) TextFieldProperty.ScrollPosition, new IntPtr(value));
        }

        /// <summary>
        /// Gets or sets the font to draw the field's text with.
        /// </summary>
        public FontID Font
        {
            get => (FontID) GetProperty((int) TextFieldProperty.Font);
            set => SetProperty((int) TextFieldProperty.Font, (IntPtr) value);
        }

        /// <inheritdoc />
        protected override unsafe bool HandleMessage(WidgetMessage message, IntPtr param1, IntPtr param2) =>
            message switch
            {
                (WidgetMessage) TextFieldMessage.TextFieldChanged => OnTextChanged(),
                WidgetMessage.KeyPress => OnKeyPressed(ref Unsafe.AsRef<KeyState>(param1.ToPointer())),
                _ => base.HandleMessage(message, param1, param2)
            };


        /// <summary>
        /// Handles the <see cref="TextFieldMessage.TextFieldChanged"/> message.
        /// </summary>
        /// <returns><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</returns>
        protected virtual bool OnTextChanged()
        {
            bool handled = false;
            TextChanged?.Invoke(this, ref handled);
            return handled;
        }

        /// <summary>
        /// Handles the <see cref="WidgetMessage.KeyPress"/> message.
        /// </summary>
        /// <returns><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</returns>
        protected virtual bool OnKeyPressed(ref KeyState keyState)
        {
            bool handled = false;
            KeyPressed?.Invoke(this, ref keyState, ref handled);
            return handled;
        }

        /// <summary>
        /// Raised when the text is changed.
        /// </summary>
        public event WidgetEventHandler<TextField>? TextChanged;

        /// <summary>
        /// Raised when a key is pressed on the keyboard.
        /// </summary>
        public event WidgetRefEventHandler<TextField, KeyState>? KeyPressed;
    }
}
