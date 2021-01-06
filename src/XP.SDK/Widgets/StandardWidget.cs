#nullable enable
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using XP.SDK.Widgets.Interop;

namespace XP.SDK.Widgets
{
    /// <summary>
    /// The base class for standard widgets.
    /// </summary>
    public abstract class StandardWidget : Widget
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardWidget"/> class.
        /// </summary>
        /// <param name="geometry">The widget geometry.</param>
        /// <param name="descriptor">The widget descriptor.</param>
        /// <param name="isVisible">The widget visibility.</param>
        /// <param name="parent">The parent widget.</param>
        /// <param name="isRoot">The value indicating whether this widget is a root one.</param>
        /// <param name="class">The standard widget class.</param>
        protected unsafe StandardWidget(in Rect geometry, string descriptor, bool isVisible, Widget? parent, bool isRoot, WidgetClass @class) 
            : base(isRoot, parent)
        {
            var id = WidgetsAPI.CreateWidget(
                geometry.Left,
                geometry.Top,
                geometry.Right,
                geometry.Bottom,
                isVisible.ToInt(),
                descriptor,
                isRoot.ToInt(),
                parent?.Id ?? default,
                @class);

            if (id == default)
                throw new InvalidOperationException("Failed to create widget.");

            Id = id;
            WidgetsAPI.AddWidgetCallback(id, &StandardWidgetCallback);
            Register(this);
        }

        /// <summary>
        /// Handles widget messages.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="param1">The first message parameter.</param>
        /// <param name="param2">The second message parameter.</param>
        /// <returns><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</returns>
        protected virtual bool HandleMessage(WidgetMessage message, IntPtr param1, IntPtr param2) => false;

        [UnmanagedCallersOnly]
        private static int StandardWidgetCallback(WidgetMessage inmessage, WidgetID inwidget, IntPtr inparam1, IntPtr inparam2)
        {
            try
            {
                if (TryGetById(inwidget, out var widget) && widget is StandardWidget standardWidget)
                {
                    return standardWidget.HandleMessage(inmessage, inparam1, inparam2).ToInt();
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
}
