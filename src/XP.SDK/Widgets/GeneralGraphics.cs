#nullable enable
using System;
using System.Collections.Generic;
using System.Text;

namespace XP.SDK.Widgets
{
    public class GeneralGraphics : StandardWidget
    {
        public const int Class = 7;

        public GeneralGraphics(in Rect geometry, string descriptor = "", bool isVisible = true, Widget? parent = null, bool isRoot = false) : 
            base(in geometry, descriptor, isVisible, parent, isRoot, Class)
        {
        }

        public GeneralGraphicsType Type
        {
            get => (GeneralGraphicsType) GetProperty((int) GeneralGraphicsProperty.GeneralGraphicsType);
            set => SetProperty((int) GeneralGraphicsProperty.GeneralGraphicsType, (IntPtr) value);
        }
    }
}
