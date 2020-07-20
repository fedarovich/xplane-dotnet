#nullable enable
using System;

namespace XP.SDK.Widgets
{
    /// <summary>
    /// This widget implements a progress indicator as seen when X-Plane starts up.
    /// </summary>
    public class ProgressBar : StandardWidget
    {
        /// <summary>
        /// The progress bar class.
        /// </summary>
        public const int Class = 8;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressBar"/> class.
        /// </summary>
        /// <param name="geometry">The widget geometry.</param>
        /// <param name="descriptor">The widget descriptor.</param>
        /// <param name="isVisible">The widget visibility.</param>
        /// <param name="parent">The parent widget.</param>
        /// <param name="isRoot">The value indicating whether this widget is a root one.</param>
        public ProgressBar(in Rect geometry, string descriptor = "", bool isVisible = true, Widget? parent = null, bool isRoot = false) 
            : base(in geometry, descriptor, isVisible, parent, isRoot, Class)
        {
        }

        /// <summary>
        /// Gets or sets the current value.
        /// </summary>
        public long Value
        {
            get => (long) GetProperty((int) ProgressBarProperty.Position);
            set => SetProperty((int) ProgressBarProperty.Position, new IntPtr(value));
        }

        /// <summary>
        /// Gets or sets the minimal value.
        /// </summary>
        public long MinValue
        {
            get => (long) GetProperty((int) ProgressBarProperty.Min);
            set => SetProperty((int) ProgressBarProperty.Min, new IntPtr(value));
        }

        /// <summary>
        /// Gets or sets the maximal value.
        /// </summary>
        public long MaxValue
        {
            get => (long) GetProperty((int) ProgressBarProperty.Max);
            set => SetProperty((int) ProgressBarProperty.Max, new IntPtr(value));
        }
    }
}
