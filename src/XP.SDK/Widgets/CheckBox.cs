#nullable enable

namespace XP.SDK.Widgets
{
    /// <summary>
    /// Represent a check box.
    /// </summary>
    /// <remarks>
    /// <para>The <see cref="Type"/> property is forced to be <see cref="ButtonType.RadioButton"/>.</para>
    /// <para>The <see cref="ButtonBehavior"/> property is forced to be <see cref="Widgets.ButtonBehavior.CheckBox"/>.</para>
    /// </remarks>
    public class CheckBox : Button
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckBox"/> class.
        /// </summary>
        /// <param name="geometry">The widget geometry.</param>
        /// <param name="descriptor">The widget descriptor.</param>
        /// <param name="isVisible">The widget visibility.</param>
        /// <param name="parent">The parent widget.</param>
        /// <param name="isRoot">The value indicating whether this widget is a root one.</param>
        public CheckBox(in Rect geometry, string descriptor = "", bool isVisible = true, Widget? parent = null, bool isRoot = false)
            : base(in geometry, descriptor, isVisible, parent, isRoot)
        {
            base.Type = ButtonType.RadioButton;
            base.ButtonBehavior = ButtonBehavior.CheckBox;
        }

        /// <inheritdoc />
        public sealed override ButtonType Type
        {
            get => ButtonType.RadioButton;
            set { }
        }

        /// <inheritdoc />
        public sealed override ButtonBehavior ButtonBehavior
        {
            get => ButtonBehavior.CheckBox;
            set { }
        }
    }
}
