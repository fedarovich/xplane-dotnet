#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using XP.SDK.Widgets.Internal;

namespace XP.SDK.Widgets
{
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


        protected CustomWidget(in Rect geometry, bool isVisible, string descriptor, bool isRoot, Widget? parent) : base(isRoot, parent)
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

        protected abstract bool HandleMessage(WidgetMessage message, IntPtr param1, IntPtr param2);
    }
}
