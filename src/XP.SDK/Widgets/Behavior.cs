using System;

namespace XP.SDK.Widgets
{
    /// <summary>
    /// Provides a standard way to attach additional behavior to the widgets.
    /// </summary>
    public abstract class Behavior
    {
        /// <summary>
        /// Gets or sets the value indicating whether this behavior is enabled. If the behavior is disabled, it won't handle any messages from the widget.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        internal int WidgetFuncCallback(WidgetMessage inMessage, WidgetID inWidget, IntPtr inParam1, IntPtr inParam2)
        {
            if (!IsEnabled)
                return 0;

            return HandleMessageCore(inMessage, inWidget, inParam1, inParam2);
        }

        /// <summary>
        /// Low-level message handling routine.
        /// </summary>
        /// <remarks>
        /// You should generally prefer overriding <see cref="HandleMessage"/> as it provides higher-level abstraction.
        /// </remarks>
        /// <param name="message">The message.</param>
        /// <param name="widgetId">The widget ID.</param>
        /// <param name="param1">The first message parameter.</param>
        /// <param name="param2">The second message parameter.</param>
        /// <returns>1 if the message was handled; 0 otherwise.</returns>
        /// <seealso cref="HandleMessage"/>
        protected virtual int HandleMessageCore(WidgetMessage message, WidgetID widgetId, IntPtr param1, IntPtr param2)
        {
            var widget = Widget.GetOrCreate(widgetId);
            return HandleMessage(message, widget, param1, param2).ToInt();
        }

        /// <summary>
        /// High-level message handling routine.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="widget">The widget.</param>
        /// <param name="param1">The first message parameter.</param>
        /// <param name="param2">The second message parameter.</param>
        /// <returns><see langword="true" /> if the message was handled; <see langword="false" /> otherwise.</returns>
        /// <seealso cref="HandleMessageCore"/>
        protected abstract bool HandleMessage(WidgetMessage message, Widget widget, IntPtr param1, IntPtr param2);
    }
}
