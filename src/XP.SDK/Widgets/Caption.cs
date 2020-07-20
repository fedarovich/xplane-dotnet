#nullable enable
using System;

namespace XP.SDK.Widgets
{
    /// <summary>
    /// A caption is a simple widget that shows its descriptor as a string, useful
    /// for labeling parts of a window. It always shows its descriptor as its
    /// string and is otherwise transparent.
    /// </summary>
    public class Caption : StandardWidget
    {
        /// <summary>
        /// The caption class.
        /// </summary>
        public const int Class = 6;

        /// <summary>
        /// Initializes a new instance of the <see cref="Caption"/> class.
        /// </summary>
        /// <param name="geometry">The widget geometry.</param>
        /// <param name="descriptor">The widget descriptor.</param>
        /// <param name="isVisible">The widget visibility.</param>
        /// <param name="parent">The parent widget.</param>
        /// <param name="isRoot">The value indicating whether this widget is a root one.</param>
        public Caption(in Rect geometry, string descriptor = "", bool isVisible = true, Widget? parent = null, bool isRoot = false) 
            : base(in geometry, descriptor, isVisible, parent, isRoot, Class)
        {
        }

        /// <summary>
        /// Gets or sets the value indicating whether the caption is lit.
        /// </summary>
        /// <remarks>Use lit captions against screens.</remarks>
        public bool IsLit
        {
            get => GetProperty((int) CaptionProperty.CaptionLit) != default;
            set => SetProperty((int) CaptionProperty.CaptionLit, new IntPtr(value.ToInt()));
        }
    }
}
