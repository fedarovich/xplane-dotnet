using System.Collections.Generic;

namespace XP.SDK.Widgets
{
    /// <summary>
    /// The interface for a collection of child <see cref="Widget"/>s.
    /// </summary>
    public interface IWidgetCollection : IReadOnlyList<Widget>
    {
        /// <summary>
        /// Adds a widget.
        /// </summary>
        /// <param name="widget">The widget to add.</param>
        void Add(Widget widget);

        /// <summary>
        /// Removes the widget.
        /// </summary>
        /// <param name="widget">The widget to remove.</param>
        void Remove(Widget widget);
    }
}
