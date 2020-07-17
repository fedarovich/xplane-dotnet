#nullable enable
using System;
using System.Collections.Generic;
using System.Text;

namespace XP.SDK.Widgets
{
    public class RadioButton : Button
    {
        public RadioButton(in Rect geometry, string descriptor = "", bool isVisible = true, Widget? parent = null, bool isRoot = false) 
            : base(in geometry, descriptor, isVisible, parent, isRoot)
        {
            base.Type = ButtonType.RadioButton;
            base.ButtonBehavior = ButtonBehavior.RadioButton;
        }

        public override ButtonType Type
        {
            get => ButtonType.RadioButton;
            set { }
        }

        public override ButtonBehavior ButtonBehavior
        {
            get => ButtonBehavior.RadioButton;
            set { }
        }
    }
}
