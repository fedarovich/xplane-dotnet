#nullable enable
using System;

namespace XP.SDK.Widgets
{
    /// <summary>
    /// X-Plane dialogs are divided into separate areas; the sub-window widgets
    /// allow you to make these areas. Create one main window and place several
    /// sub-windows inside it.Then place your controls inside the sub- windows.
    /// </summary>
    public class SubWindow : StandardWidget
    {
        /// <summary>
        /// The sub-window c
        /// </summary>
        public const int Class = 2;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubWindow"/> class.
        /// </summary>
        /// <param name="geometry">The widget geometry.</param>
        /// <param name="descriptor">The widget descriptor.</param>
        /// <param name="isVisible">The widget visibility.</param>
        /// <param name="parent">The parent widget.</param>
        /// <param name="isRoot">The value indicating whether this widget is a root one.</param>
        public SubWindow(in Rect geometry, string descriptor = "", bool isVisible = true, Widget? parent = null, bool isRoot = false) : 
            base(in geometry, descriptor, isVisible, parent, isRoot, Class)
        {
        }

        /// <summary>
        /// Gets or sets the type of the sub-window.
        /// </summary>
        public SubWindowType Type
        {
            get => (SubWindowType) GetProperty((int) SubWindowProperty.SubWindowType);
            set => SetProperty((int) SubWindowProperty.SubWindowType, (IntPtr) value);
        }
    }
}
