#nullable enable
using System;

namespace XP.SDK.Widgets
{
    /// <summary>
    /// The general graphics widget can show one of many icons available from X-Plane.
    /// </summary>
    public class GeneralGraphics : StandardWidget
    {
        /// <summary>
        /// The general graphics class.
        /// </summary>
        public const int Class = 7;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralGraphics"/> class.
        /// </summary>
        /// <param name="geometry">The widget geometry.</param>
        /// <param name="descriptor">The widget descriptor.</param>
        /// <param name="isVisible">The widget visibility.</param>
        /// <param name="parent">The parent widget.</param>
        /// <param name="isRoot">The value indicating whether this widget is a root one.</param>
        public GeneralGraphics(in Rect geometry, string descriptor = "", bool isVisible = true, Widget? parent = null, bool isRoot = false) : 
            base(in geometry, descriptor, isVisible, parent, isRoot, Class)
        {
        }

        /// <summary>
        /// Gets or sets the general graphics type.
        /// </summary>
        public GeneralGraphicsType Type
        {
            get => (GeneralGraphicsType) GetProperty((int) GeneralGraphicsProperty.GeneralGraphicsType);
            set => SetProperty((int) GeneralGraphicsProperty.GeneralGraphicsType, (IntPtr) value);
        }
    }
}
