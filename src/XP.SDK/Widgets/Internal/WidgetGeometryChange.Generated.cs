using System;

namespace XP.SDK.Widgets.Internal
{
    
    /// <summary>
    /// <para>
    /// This structure contains the deltas for your widget's geometry when it
    /// changes.
    /// </para>
    /// </summary>
    public partial struct WidgetGeometryChange
    {
        public int dx;
        public int dy;
        public int dwidth;
        public int dheight;
    }
}