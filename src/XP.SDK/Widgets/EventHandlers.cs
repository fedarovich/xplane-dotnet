namespace XP.SDK.Widgets
{
    public delegate void WidgetEventHandler<in TWidget>(TWidget sender, ref bool handled) 
        where TWidget : Widget;

    public delegate void WidgetValueEventHandler<in TWidget, in TArgs>(TWidget sender, ref bool handled, TArgs args)
        where TWidget : Widget;

    public delegate void WidgetInEventHandler<in TWidget, TArgs>(TWidget sender, ref bool handled, in TArgs args)
        where TWidget : Widget
        where TArgs : struct;

    public delegate void WidgetRefEventHandler<in TWidget, TArgs>(TWidget sender, ref bool handled, ref TArgs args)
        where TWidget : Widget
        where TArgs : struct;
}