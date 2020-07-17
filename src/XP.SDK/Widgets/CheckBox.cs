#nullable enable
using System;
using System.Collections.Generic;
using System.Text;

namespace XP.SDK.Widgets
{
    public class CheckBox : Button
    {
        public CheckBox(in Rect geometry, string descriptor = "", bool isVisible = true, Widget? parent = null, bool isRoot = false)
            : base(in geometry, descriptor, isVisible, parent, isRoot)
        {
            base.Type = ButtonType.RadioButton;
            base.ButtonBehavior = ButtonBehavior.CheckBox;
        }

        public sealed override ButtonType Type
        {
            get => ButtonType.RadioButton;
            set { }
        }

        public sealed override ButtonBehavior ButtonBehavior
        {
            get => ButtonBehavior.CheckBox;
            set { }
        }
    }
}
