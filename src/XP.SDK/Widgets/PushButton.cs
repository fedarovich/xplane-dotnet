#nullable enable

namespace XP.SDK.Widgets
{
    /// <summary>
    /// Represent a push button.
    /// </summary>
    /// <remarks>
    /// <para>The <see cref="Type"/> property is forced to be <see cref="ButtonType.PushButton"/>.</para>
    /// <para>The <see cref="ButtonBehavior"/> property is forced to be <see cref="Widgets.ButtonBehavior.PushButton"/>.</para>
    /// </remarks>
    public class PushButton : Button
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PushButton"/> class.
        /// </summary>
        /// <param name="geometry">The widget geometry.</param>
        /// <param name="descriptor">The widget descriptor.</param>
        /// <param name="isVisible">The widget visibility.</param>
        /// <param name="parent">The parent widget.</param>
        /// <param name="isRoot">The value indicating whether this widget is a root one.</param>
        public PushButton(in Rect geometry, string descriptor = "", bool isVisible = true, Widget? parent = null, bool isRoot = false) : base(in geometry, descriptor, isVisible, parent, isRoot)
        {
            base.Type = ButtonType.PushButton;
            base.ButtonBehavior = ButtonBehavior.PushButton;
        }

        /// <inheritdoc />
        public sealed override ButtonType Type
        {
            get => ButtonType.PushButton;
            set { }
        }

        /// <inheritdoc />
        public sealed override ButtonBehavior ButtonBehavior
        {
            get => ButtonBehavior.PushButton;
            set { }
        }
    }
}
