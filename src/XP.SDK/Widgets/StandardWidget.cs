#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using XP.SDK.Widgets.Internal;

namespace XP.SDK.Widgets
{
    public abstract class StandardWidget : Widget
    {
        private static readonly WidgetFuncCallback _standardWidgetCallback;

        static StandardWidget()
        {
            _standardWidgetCallback = StandardWidgetCallback;

            static int StandardWidgetCallback(WidgetMessage inmessage, WidgetID inwidget, IntPtr inparam1, IntPtr inparam2)
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


        protected StandardWidget(in Rect geometry, bool isVisible, string descriptor, bool isRoot, Widget? parent, WidgetClass @class) : base(isRoot, parent)
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
            WidgetsAPI.AddWidgetCallback(id, _standardWidgetCallback);
            Register(this);
        }

        protected virtual bool HandleMessage(WidgetMessage message, IntPtr param1, IntPtr param2)
        {
            return false;
        }
    }
}
