#nullable enable
using System;
using System.Collections.Generic;
using System.Text;

namespace XP.SDK.Widgets
{
    public class ProgressBar : StandardWidget
    {
        public const int Class = 8;

        public ProgressBar(in Rect geometry, string descriptor = "", bool isVisible = true, Widget? parent = null, bool isRoot = false) 
            : base(in geometry, descriptor, isVisible, parent, isRoot, Class)
        {
        }

        public long Value
        {
            get => (long) GetProperty((int) ProgressBarProperty.Position);
            set => SetProperty((int) ProgressBarProperty.Position, new IntPtr(value));
        }

        public long MinValue
        {
            get => (long) GetProperty((int) ProgressBarProperty.Min);
            set => SetProperty((int) ProgressBarProperty.Min, new IntPtr(value));
        }

        public long MaxValue
        {
            get => (long) GetProperty((int) ProgressBarProperty.Max);
            set => SetProperty((int) ProgressBarProperty.Max, new IntPtr(value));
        }
    }
}
