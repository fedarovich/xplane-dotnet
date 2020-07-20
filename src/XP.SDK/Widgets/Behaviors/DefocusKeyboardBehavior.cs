using System;
using XP.SDK.Widgets.Internal;

namespace XP.SDK.Widgets.Behaviors
{
    /// <summary>
    /// This causes a click in the widget to send keyboard focus back to X-Plane.
    /// This stops editing of any text fields, etc.
    /// </summary>
    public sealed class DefocusKeyboardBehavior : Behavior
    {
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public DefocusKeyboardBehavior()
        {
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="eatClicks">Specifies whether clicks in the background should be consumed by bringing the window to the foreground.</param>
        public DefocusKeyboardBehavior(bool eatClicks)
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
            return WidgetUtilsAPI.DefocusKeyboard(message, widgetId, param1, param2, EatClicks.ToInt());
        }

        /// <inheritdoc />
        protected override bool HandleMessage(WidgetMessage message, Widget widget, IntPtr param1, IntPtr param2)
        {
            throw new NotImplementedException();
        }
    }
}
