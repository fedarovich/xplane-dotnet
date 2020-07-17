#nullable enable
using System;
using System.Collections.Generic;
using System.Text;

namespace XP.SDK.Widgets
{
    public class PushButton : Button
    {
        public PushButton(in Rect geometry, string descriptor = "", bool isVisible = true, Widget? parent = null, bool isRoot = false) : base(in geometry, descriptor, isVisible, parent, isRoot)
        {
            base.Type = ButtonType.PushButton;
            base.ButtonBehavior = ButtonBehavior.PushButton;
        }

        public sealed override ButtonType Type
        {
            get => ButtonType.PushButton;
            set { }
        }

        public sealed override ButtonBehavior ButtonBehavior
        {
            get => ButtonBehavior.PushButton;
            set { }
        }
    }
}
