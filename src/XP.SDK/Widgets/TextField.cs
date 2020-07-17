#nullable enable
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using XP.SDK.Widgets.Internal;
using XP.SDK.XPLM;

namespace XP.SDK.Widgets
{
    public class TextField : StandardWidget
    {
        public const int Class = 4;

        public TextField(in Rect geometry, string descriptor = "", bool isVisible = true, Widget? parent = null, bool isRoot = false) 
            : base(in geometry, descriptor, isVisible, parent, isRoot, Class)
        {
        }

        public TextFieldType Type
        {
            get => (TextFieldType) GetProperty((int) TextFieldProperty.TextFieldType);
            set => SetProperty((int) TextFieldProperty.TextFieldType, (IntPtr) value);
        }

        public int SelectionStart
        {
            get => (int) GetProperty((int) TextFieldProperty.EditFieldSelStart);
            set => SetProperty((int) TextFieldProperty.EditFieldSelStart, new IntPtr(value));
        }

        public int SelectionEnd
        {
            get => (int) GetProperty((int) TextFieldProperty.EditFieldSelEnd);
            set => SetProperty((int) TextFieldProperty.EditFieldSelEnd, new IntPtr(value));
        }

        public int SelectionDragStart => (int) GetProperty((int) TextFieldProperty.EditFieldSelDragStart);

        public bool IsPasswordMode
        {
            get => GetProperty((int) TextFieldProperty.PasswordMode) != default;
            set => SetProperty((int) TextFieldProperty.PasswordMode, new IntPtr(value.ToInt()));
        }

        public int MaxLength
        {
            get => (int) GetProperty((int) TextFieldProperty.MaxCharacters);
            set => SetProperty((int) TextFieldProperty.MaxCharacters, new IntPtr(value));
        }

        public int ScrollPosition
        {
            get => (int) GetProperty((int) TextFieldProperty.ScrollPosition);
            set => SetProperty((int) TextFieldProperty.ScrollPosition, new IntPtr(value));
        }

        public FontID Font
        {
            get => (FontID) GetProperty((int) TextFieldProperty.Font);
            set => SetProperty((int) TextFieldProperty.Font, (IntPtr) value);
        }

        protected override unsafe bool HandleMessage(WidgetMessage message, IntPtr param1, IntPtr param2) =>
            message switch
            {
                (WidgetMessage) TextFieldMessage.TextFieldChanged => OnTextChanged(),
                WidgetMessage.KeyPress => OnKeyPressed(ref Unsafe.AsRef<KeyState>(param1.ToPointer())),
                _ => base.HandleMessage(message, param1, param2)
            };
        

        protected virtual bool OnTextChanged()
        {
            bool handled = false;
            TextChanged?.Invoke(this, ref handled);
            return handled;
        }

        protected virtual bool OnKeyPressed(ref KeyState keyState)
        {
            bool handled = false;
            KeyPressed?.Invoke(this, ref handled, ref keyState);
            return handled;
        }

        public event WidgetEventHandler<TextField>? TextChanged;

        public event WidgetRefEventHandler<TextField, KeyState>? KeyPressed;
    }
}
