#nullable enable
using System;
using System.Collections.Generic;
using System.Text;

namespace XP.SDK.Widgets
{
    public class Caption : StandardWidget
    {
        public const int Class = 6;

        public Caption(in Rect geometry, string descriptor = "", bool isVisible = true, Widget? parent = null, bool isRoot = false) 
            : base(in geometry, descriptor, isVisible, parent, isRoot, Class)
        {
        }

        public bool IsLit
        {
            get => GetProperty((int) CaptionProperty.CaptionLit) != default;
            set => SetProperty((int) CaptionProperty.CaptionLit, new IntPtr(value.ToInt()));
        }
    }
}
