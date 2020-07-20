#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using XP.SDK.Widgets.Internal;

namespace XP.SDK.Widgets
{
    /// <summary>
    /// A base class for creating custom widgets.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The inheritors must override <see cref="HandleMessage"/> method.
    /// </para>
    /// <para>
    /// You can also create custom widgets by inheriting from <see cref="CustomWidgetEx"/>
    /// which provides higher-level interface but has worse performance.
    /// </para>
    /// </remarks>
    /// <seealso cref="CustomWidgetEx"/>
    public abstract class CustomWidget : Widget
    {
        private static readonly WidgetFuncCallback _customWidgetCallback;

        static CustomWidget()
        {
            _customWidgetCallback = CustomWidgetCallback;

            static int CustomWidgetCallback(WidgetMessage inmessage, WidgetID inwidget, IntPtr inparam1, IntPtr inparam2)
            {
                try
                {
                    if (TryGetById(inwidget, out var widget) && widget is CustomWidget customWidget)
                    {
                        return customWidget.HandleMessage(inmessage, inparam1, inparam2).ToInt();
                    }
                }
                finally
                {
                    if (inmessage == WidgetMessage.Destroy)
                    {
                        Unregister(inwidget);
                    }
                }

                return 0;
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="CustomWidget"/> class.
        /// </summary>
        /// <param name="geometry">The widget geometry.</param>
        /// <param name="descriptor">The widget descriptor.</param>
        /// <param name="isVisible">The widget visibility.</param>
        /// <param name="parent">The parent widget.</param>
        /// <param name="isRoot">The value indicating whether this widget is a root one.</param>
        protected CustomWidget(in Rect geometry, string descriptor, bool isVisible, Widget? parent, bool isRoot) : base(isRoot, parent)
        {
            var id = WidgetsAPI.CreateCustomWidget(
                geometry.Left,
                geometry.Top,
                geometry.Right,
                geometry.Bottom,
                isVisible.ToInt(),
                descriptor,
                isRoot.ToInt(),
                parent?.Id ?? default,
                _customWidgetCallback);

            if (id == default)
                throw new InvalidOperationException("Failed to create widget.");

            Id = id;
            Register(this);
        }

        /// <summary>
        /// Handles widget messages.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="param1">The first message parameter.</param>
        /// <param name="param2">The second message parameter.</param>
        /// <returns><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</returns>
        protected abstract bool HandleMessage(WidgetMessage message, IntPtr param1, IntPtr param2);
    }
}
