#nullable enable
using System;
using System.Collections.Generic;
using System.Text;

namespace XP.SDK.Widgets
{
    public class SubWindow : StandardWidget
    {
        public const int Class = 2;

        public SubWindow(in Rect geometry, string descriptor = "", bool isVisible = true, Widget? parent = null, bool isRoot = false) : 
            base(in geometry, descriptor, isVisible, parent, isRoot, Class)
        {
        }

        public SubWindowType Type
        {
            get => (SubWindowType) GetProperty((int) SubWindowProperty.SubWindowType);
            set => SetProperty((int) SubWindowProperty.SubWindowType, (IntPtr) value);
        }
    }
}
