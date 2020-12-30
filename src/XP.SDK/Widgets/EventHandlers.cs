using System;
using XP.SDK.Widgets.Interop;
using XP.SDK.XPLM;

namespace XP.SDK.Widgets
{
    /// <summary>
    /// Generic event handler for widget message with no parameters.
    /// </summary>
    /// <typeparam name="TWidget">The widget type.</typeparam>
    /// <param name="sender">The sender.</param>
    /// <param name="handled"><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</param>
    public delegate void WidgetEventHandler<in TWidget>(TWidget sender, ref bool handled) 
        where TWidget : Widget;

    /// <summary>
    /// Generic event handler for widget message with one argument passed by value.
    /// </summary>
    /// <typeparam name="TWidget">The widget type.</typeparam>
    /// <typeparam name="TArgs">The argument type.</typeparam>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The argument.</param>
    /// <param name="handled"><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</param>
    public delegate void WidgetValueEventHandler<in TWidget, in TArgs>(TWidget sender, TArgs args, ref bool handled)
        where TWidget : Widget;

    /// <summary>
    /// Generic event handler for widget message with one argument passed by read-only reference.
    /// </summary>
    /// <typeparam name="TWidget">The widget type.</typeparam>
    /// <typeparam name="TArgs">The argument type.</typeparam>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The argument.</param>
    /// <param name="handled"><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</param>
    public delegate void WidgetInEventHandler<in TWidget, TArgs>(TWidget sender, in TArgs args, ref bool handled)
        where TWidget : Widget
        where TArgs : struct;

    /// <summary>
    /// Generic event handler for widget message with one argument passed by reference.
    /// </summary>
    /// <typeparam name="TWidget">The widget type.</typeparam>
    /// <typeparam name="TArgs">The argument type.</typeparam>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The argument.</param>
    /// <param name="handled"><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</param>
    public delegate void WidgetRefEventHandler<in TWidget, TArgs>(TWidget sender, ref TArgs args, ref bool handled)
        where TWidget : Widget
        where TArgs : struct;


    /// <summary>
    /// Event handler for <see cref="WidgetMessage.Create"/> message.
    /// </summary>
    /// <typeparam name="TWidget">The widget type.</typeparam>
    /// <param name="sender">The sender.</param>
    /// <param name="isSubclass"><see langword="true"/> if you are being added as a subclass, <see langword="false"/> if the widget is first being created.</param>
    /// <param name="handled"><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</param>
    public delegate void WidgetCreatedEventHandler<in TWidget>(TWidget sender, bool isSubclass, ref bool handled)
        where TWidget : Widget;

    /// <summary>
    /// Event handler for <see cref="WidgetMessage.Destroy"/> message.
    /// </summary>
    /// <typeparam name="TWidget">The widget type.</typeparam>
    /// <param name="sender">The sender.</param>
    /// <param name="isRecursive"><see langword="true"/> if being deleted by a recursive delete to the parent, <see langword="false"/> for explicit deletion.</param>
    /// <param name="handled"><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</param>
    public delegate void WidgetDestroyedEventHandler<in TWidget>(TWidget sender, bool isRecursive, ref bool handled)
        where TWidget : Widget;

    /// <summary>
    /// Event handler for <see cref="WidgetMessage.KeyTakeFocus"/> message.
    /// </summary>
    /// <typeparam name="TWidget">The widget type.</typeparam>
    /// <param name="sender">The sender.</param>
    /// <param name="fromChild"><see langword="true"/> if a child of yours gave up focus to you; <see langword="false"/> if someone set focus on you explicitly.</param>
    /// <param name="handled"><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</param>
    public delegate void WidgetTakingFocusEventHandler<in TWidget>(TWidget sender, bool fromChild, ref bool handled)
        where TWidget : Widget;

    /// <summary>
    /// Event handler for <see cref="WidgetMessage.KeyLoseFocus"/> message.
    /// </summary>
    /// <typeparam name="TWidget">The widget type.</typeparam>
    /// <param name="sender">The sender.</param>
    /// <param name="takenByOtherWidget"><see langword="true"/> if focus is being taken by another widget; <see langword="false"/> if code requested to remove focus.</param>
    /// <param name="handled"><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</param>
    public delegate void WidgetLostFocusEventHandler<in TWidget>(TWidget sender, bool takenByOtherWidget, ref bool handled)
        where TWidget : Widget;

    /// <summary>
    /// Event handler for <see cref="WidgetMessage.Reshape"/> message.
    /// </summary>
    /// <typeparam name="TWidget">The widget type.</typeparam>
    /// <param name="sender">The sender.</param>
    /// <param name="originalWidget">The reshaped widget.</param>
    /// <param name="change">The change.</param>
    /// <param name="handled"><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</param>
    public delegate void WidgetReshapeEventHandler<in TWidget>(TWidget sender, Widget originalWidget, ref WidgetGeometryChange change, ref bool handled)
        where TWidget : Widget;

    /// <summary>
    /// Event handler for <see cref="WidgetMessage.PropertyChanged"/> message.
    /// </summary>
    /// <typeparam name="TWidget">The widget type.</typeparam>
    /// <param name="sender">The sender.</param>
    /// <param name="propertyId">The property ID.</param>
    /// <param name="newValue">The new property value.</param>
    /// <param name="handled"><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</param>
    public delegate void WidgetPropertyChangeEventHandler<in TWidget>(TWidget sender, int propertyId, IntPtr newValue, ref bool handled)
        where TWidget : Widget;

    /// <summary>
    /// Event handler for <see cref="WidgetMessage.PropertyChanged"/> message.
    /// </summary>
    /// <typeparam name="TWidget">The widget type.</typeparam>
    /// <param name="sender">The sender.</param>
    /// <param name="mouseState">The mouse state.</param>
    /// <param name="cursorStatus">The cursor.</param>
    /// <param name="handled"><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</param>
    public delegate void WidgetCursorAdjustEventHandler<in TWidget>(TWidget sender, ref MouseState mouseState, ref CursorStatus cursorStatus, ref bool handled)
        where TWidget : Widget;
}