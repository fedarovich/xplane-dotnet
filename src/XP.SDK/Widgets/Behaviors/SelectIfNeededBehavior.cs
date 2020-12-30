using System;
using XP.SDK.Widgets.Interop;

namespace XP.SDK.Widgets.Behaviors
{
    /// <summary>
    /// Causes the widget to bring its window to the foreground if it is not already.
    /// <see cref="EatClicks"/> specifies whether clicks in the background should be consumed by bringing the window to the foreground.
    /// </summary>
    public sealed class SelectIfNeededBehavior : Behavior
    {
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public SelectIfNeededBehavior()
        {
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="eatClicks">Specifies whether clicks in the background should be consumed by bringing the window to the foreground.</param>
        public SelectIfNeededBehavior(bool eatClicks)
        {
            EatClicks = eatClicks;
        }

        /// <summary>
        /// Gets or sets the value indicating whether clicks in the background should be consumed by bringing the window to the foreground.
        /// </summary>
        public bool EatClicks { get; set; }

        /// <inheritdoc />
        protected override int HandleMessageCore(WidgetMessage message, WidgetID widgetId, IntPtr param1, IntPtr param2)
        {
            return WidgetUtilsAPI.SelectIfNeeded(message, widgetId, param1, param2, EatClicks.ToInt());
        }

        /// <inheritdoc />
        protected override bool HandleMessage(WidgetMessage message, Widget widget, IntPtr param1, IntPtr param2)
        {
            throw new NotImplementedException();
        }
    }
}
