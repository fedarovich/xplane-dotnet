using System;

namespace XP.SDK.Widgets
{
    
    /// <summary>
    /// <para>
    /// There are a few built-in window styles in X-Plane that you can use.
    /// </para>
    /// <para>
    /// Note that X-Plane 6 does not offer real shadow-compositing; you must make
    /// sure to put a window on top of another window of the right style to the
    /// shadows work, etc. This applies to elements with insets and shadows. The
    /// rules are:
    /// </para>
    /// <para>
    /// Sub windows must go on top of main windows, and screens and list views on
    /// top of subwindows. Only help and main windows can be over the main screen.
    /// </para>
    /// <para>
    /// With X-Plane 7 any window or element may be placed over any other element.
    /// </para>
    /// <para>
    /// Some windows are scaled by stretching, some by repeating. The drawing
    /// routines know which scaling method to use. The list view cannot be rescaled
    /// in X-Plane 6 because it has both a repeating pattern and a gradient in one
    /// element. All other elements can be rescaled.
    /// </para>
    /// </summary>
    public enum WindowStyle
    {
        Help = 0,
        MainWindow = 1,
        SubWindow = 2,
        Screen = 4,
        ListView = 5
    }
}