using System;

namespace XP.SDK.Widgets.Internal
{
    public unsafe partial struct WidgetCreate
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
        public int visible;
        public byte *descriptor;
        public int isRoot;
        public int containerIndex;
        public WidgetClass widgetClass;
    }
}