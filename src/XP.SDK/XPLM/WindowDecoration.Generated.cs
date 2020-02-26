using System;

namespace XP.SDK.XPLM
{
    
    /// <summary>
    /// <para>
    /// XPLMWindowDecoration describes how "modern" windows will be displayed. This
    /// impacts both how X-Plane draws your window as well as certain mouse
    /// handlers.
    /// </para>
    /// <para>
    /// Your window's decoration can only be specified when you create the window
    /// (in the XPLMCreateWindow_t you pass to XPLMCreateWindowEx()).
    /// </para>
    /// </summary>
    public enum WindowDecoration
    {
        None = 0,
        RoundRectangle = 1,
        SelfDecorated = 2,
        SelfDecoratedResizable = 3
    }
}