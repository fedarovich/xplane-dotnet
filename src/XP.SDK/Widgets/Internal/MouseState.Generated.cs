using System;

namespace XP.SDK.Widgets.Internal
{
    
    /// <summary>
    /// <para>
    /// When the mouse is clicked or dragged, a pointer to this structure is passed
    /// to your widget function.
    /// </para>
    /// </summary>
    public partial struct MouseState
    {
        public int x;
        public int y;
        public int button;
        public int delta;
    }
}