using System;
using System.Runtime.CompilerServices;
using XP.SDK.Widgets.Internal;

namespace XP.SDK.Widgets.Behaviors
{
    /// <summary>
    /// Drags the widget in response to mouse clicks.
    /// </summary>
    public sealed class DragWidgetBehavior : Behavior
    {
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public DragWidgetBehavior()
        {
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="dragRegion">The global coordinates of the drag region, which might be a sub-region of your widget (for example, a title bar).</param>
        public DragWidgetBehavior(in Rect dragRegion)
        {
            DragRegion = dragRegion;
        }

        /// <summary>
        /// Gets or sets the global coordinates of the drag region, which might be a sub-region of your widget (for example, a title bar).
        /// </summary>
        public Rect DragRegion { get; set; }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        protected override int HandleMessageCore(WidgetMessage message, WidgetID widgetId, IntPtr param1, IntPtr param2)
        {
            var (left, top, right, bottom) = DragRegion;
            return WidgetUtilsAPI.DragWidget(message, widgetId, param1, param2, left, top, right, bottom);
        }

        /// <inheritdoc />
        protected override bool HandleMessage(WidgetMessage message, Widget widget, IntPtr param1, IntPtr param2)
        {
            throw new NotImplementedException();
        }
    }
}
