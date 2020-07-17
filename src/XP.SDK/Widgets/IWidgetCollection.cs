using System;
using System.Collections.Generic;
using System.Text;

namespace XP.SDK.Widgets
{
    public interface IWidgetCollection : IReadOnlyList<Widget>
    {
        void Add(Widget widget);

        void Remove(Widget widget);
    }
}
