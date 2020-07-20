#nullable enable

namespace XP.SDK.Widgets
{
    /// <summary>
    /// Represent a radio button.
    /// </summary>
    /// <remarks>
    /// <para>The <see cref="Type"/> property is forced to be <see cref="ButtonType.RadioButton"/>.</para>
    /// <para>The <see cref="ButtonBehavior"/> property is forced to be <see cref="Widgets.ButtonBehavior.RadioButton"/>.</para>
    /// </remarks>
    public class RadioButton : Button
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RadioButton"/> class.
        /// </summary>
        /// <param name="geometry">The widget geometry.</param>
        /// <param name="descriptor">The widget descriptor.</param>
        /// <param name="isVisible">The widget visibility.</param>
        /// <param name="parent">The parent widget.</param>
        /// <param name="isRoot">The value indicating whether this widget is a root one.</param>
        public RadioButton(in Rect geometry, string descriptor = "", bool isVisible = true, Widget? parent = null, bool isRoot = false) 
            : base(in geometry, descriptor, isVisible, parent, isRoot)
        {
            base.Type = ButtonType.RadioButton;
            base.ButtonBehavior = ButtonBehavior.RadioButton;
        }

        /// <inheritdoc />
        public override ButtonType Type
        {
            get => ButtonType.RadioButton;
            set { }
        }

        /// <inheritdoc />
        public override ButtonBehavior ButtonBehavior
        {
            get => ButtonBehavior.RadioButton;
            set { }
        }
    }
}
