using System;

namespace XP.SDK.Widgets
{
    
    /// <summary>
    /// <para>
    /// A track is a UI element that displays a value vertically or horizontally.
    /// X-Plane has three kinds of tracks: scroll bars, sliders, and progress bars.
    /// Tracks can be displayed either horizontally or vertically; tracks will
    /// choose their own layout based on the larger dimension of their dimensions
    /// (e.g. they know if they are tall or wide). Sliders may be lit or unlit
    /// (showing the user manipulating them).
    /// </para>
    /// <para>
    /// ScrollBar - this is a standard scroll bar with arrows and a thumb to drag.
    /// Slider - this is a simple track with a ball in the middle that can be slid.
    /// Progress - this is a progress indicator showing how a long task is going.
    /// </para>
    /// </summary>
    public enum TrackStyle
    {
        ScrollBar = 0,
        Slider = 1,
        Progress = 2
    }
}