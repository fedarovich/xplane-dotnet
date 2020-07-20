using System;
using XP.SDK.Widgets.Internal;

namespace XP.SDK.Widgets.Behaviors
{
    /// <summary>
    /// Causes the widget to maintain its children in fixed position relative to itself as it is resized. Use this on the top level 'window' widget for your window.
    /// </summary>
    public sealed class FixedLayoutBehavior : Behavior
    {
        private static readonly Lazy<FixedLayoutBehavior> _shared = new Lazy<FixedLayoutBehavior>(false);

        /// <summary>
        /// Gets a shared instance of <see cref="FixedLayoutBehavior"/>.
        /// </summary>
        public static FixedLayoutBehavior Shared => _shared.Value;

        /// <inheritdoc />
        protected override int HandleMessageCore(WidgetMessage message, WidgetID widgetId, IntPtr param1, IntPtr param2)
        {
            return WidgetUtilsAPI.FixedLayout(message, widgetId, param1, param2);
        }

        /// <inheritdoc />
        protected override bool HandleMessage(WidgetMessage message, Widget widget, IntPtr param1, IntPtr param2)
        {
            throw new NotImplementedException();
        }
    }
}
