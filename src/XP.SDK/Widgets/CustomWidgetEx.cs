#nullable enable
using System;
using System.Runtime.CompilerServices;
using XP.SDK.Widgets.Interop;
using XP.SDK.XPLM;

namespace XP.SDK.Widgets
{
    /// <summary>
    /// A base class for creating custom widgets.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The inheritors should override the virtual methods for the message they actually need.
    /// </para>
    /// <para>
    /// You can also create custom widgets by inheriting from <see cref="CustomWidget"/>
    /// which provides lower-level interface but has better performance.
    /// </para>
    /// </remarks>
    /// <seealso cref="CustomWidget"/>
    public abstract class CustomWidgetEx : CustomWidget
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomWidgetEx"/> class.
        /// </summary>
        /// <param name="geometry">The widget geometry.</param>
        /// <param name="descriptor">The widget descriptor.</param>
        /// <param name="isVisible">The widget visibility.</param>
        /// <param name="parent">The parent widget.</param>
        /// <param name="isRoot">The value indicating whether this widget is a root one.</param>
        protected CustomWidgetEx(in Rect geometry, string descriptor, bool isVisible, Widget? parent, bool isRoot) : base(in geometry, descriptor, isVisible, parent, isRoot)
        {
        }

        /// <inheritdoc />
        protected sealed override bool HandleMessage(WidgetMessage message, IntPtr param1, IntPtr param2) =>
            message switch
            {
                WidgetMessage.None => false,
                WidgetMessage.Create => OnCreated(param1 != default),
                WidgetMessage.Destroy => OnDestroyed(param1 != default),
                WidgetMessage.Paint => OnPaint(),
                WidgetMessage.Draw => OnDraw(),
                WidgetMessage.KeyPress => OnKeyPress(ref AsRef<KeyState>(param1)),
                WidgetMessage.KeyTakeFocus => OnTakingFocus(param1 != default),
                WidgetMessage.KeyLoseFocus => OnLostFocus(param1 != default),
                WidgetMessage.MouseDown => OnMouseDown(ref AsRef<MouseState>(param1)),
                WidgetMessage.MouseDrag => OnMouseDrag(ref AsRef<MouseState>(param1)),
                WidgetMessage.MouseUp => OnMouseUp(ref AsRef<MouseState>(param1)),
                WidgetMessage.Reshape => OnReshape(GetOrCreate(new WidgetID(param1)), ref AsRef<WidgetGeometryChange>(param2)),
                WidgetMessage.ExposedChanged => OnExposedChanged(),
                WidgetMessage.AcceptChild => OnChildAdded(GetOrCreate(new WidgetID(param1))),
                WidgetMessage.LoseChild => OnChildRemoved(GetOrCreate(new WidgetID(param1))),
                WidgetMessage.AcceptParent => OnParentChanged(param1 != default ? GetOrCreate(new WidgetID(param1)) : null),
                WidgetMessage.Shown => OnShown(GetOrCreate(new WidgetID(param1))),
                WidgetMessage.Hidden => OnHidden(GetOrCreate(new WidgetID(param1))),
                WidgetMessage.DescriptorChanged => OnDescriptorChanged(),
                WidgetMessage.PropertyChanged => OnPropertyChanged(param1.ToInt32(), param2),
                WidgetMessage.MouseWheel => OnMouseWheel(ref AsRef<MouseState>(param1)), 
                WidgetMessage.CursorAdjust => OnCursorAdjust(ref AsRef<MouseState>(param1), ref AsRef<CursorStatus>(param2)),
                _ => OnCustomMessage((int) message, param1, param2)
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe ref T AsRef<T>(IntPtr ptr) where T : unmanaged => ref Unsafe.AsRef<T>(ptr.ToPointer());

        /// <summary>
        /// Handles the <see cref="WidgetMessage.Create"/> message.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The create message is sent once per widget that is created with your widget function and once
        /// for any widget that has your widget function attached.
        /// </para>
        /// <para>Dispatching: <see cref="DispatchMode.Direct"/></para>
        /// </remarks>
        /// <param name="isSubclass"><see langword="true"/> if you are being added as a subclass, <see langword="false"/> if the widget is first being created.</param>
        /// <returns><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</returns>
        protected virtual bool OnCreated(bool isSubclass) => false;

        /// <summary>
        /// Handles the <see cref="WidgetMessage.Destroy"/> message.
        /// </summary>
        /// <remarks>
        /// <para>The destroy message is sent once for each message that is destroyed that has your widget function.</para>
        /// <para>Dispatching: <see cref="DispatchMode.DirectAllCallbacks"/></para>
        /// </remarks>
        /// <param name="recursive"><see langword="true"/> if being deleted by a recursive delete to the parent, <see langword="false"/> for explicit deletion.</param>
        /// <returns><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</returns>
        protected virtual bool OnDestroyed(bool recursive) => false;


        /// <summary>
        /// Handles the <see cref="WidgetMessage.Paint"/> message.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The paint message is sent to your widget to draw itself. The paint message is the bare-bones
        /// message; in response you must draw yourself, draw your children, set up clipping and culling,
        /// check for visibility, etc. If you don't want to do all of this, ignore the paint message and
        /// a draw message (see below) will be sent to you.
        /// </para>
        /// <para>Dispatching: <see cref="DispatchMode.Direct"/></para>
        /// </remarks>
        /// <returns><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</returns>
        protected virtual bool OnPaint() => false;

        /// <summary>
        /// Handles the <see cref="WidgetMessage.Draw"/> message.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The draw message is sent to your widget when it is time to draw yourself. OpenGL will be set up
        /// to draw in 2-d global screen coordinates, but you should use the XPLM to set up OpenGL state.
        /// </para>
        /// <para>Dispatching: <see cref="DispatchMode.Direct"/></para>
        /// </remarks>
        /// <returns><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</returns>
        protected virtual bool OnDraw() => false;

        /// <summary>
        /// Handles the <see cref="WidgetMessage.KeyPress"/> message.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The key press message is sent once per key that is pressed. The first parameter is the type of key
        /// code (integer or char) and the second is the code itself. By handling this event, you consume the
        /// key stroke.
        /// </para>
        /// <para>Handling this message 'consumes' the keystroke; not handling it passes it to your parent widget.</para>
        /// <para>Dispatching: <see cref="DispatchMode.UpChain"/></para>
        /// </remarks>
        /// <returns><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</returns>
        protected virtual bool OnKeyPress(ref KeyState keyState) => false;


        /// <summary>
        /// Handles the <see cref="WidgetMessage.KeyTakeFocus"/> message.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Keyboard focus is being given to you. By handling this message you accept keyboard focus. 
        /// </para>
        /// <para>Handling this message accepts focus; not handling refuses focus.</para>
        /// <para>Dispatching: <see cref="DispatchMode.Direct"/></para>
        /// </remarks>
        /// <param name="fromChild"><see langword="true"/> if a child of yours gave up focus to you; <see langword="false"/> if someone set focus on you explicitly.</param>
        /// <returns><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</returns>
        protected virtual bool OnTakingFocus(bool fromChild) => false;

        /// <summary>
        /// Handles the <see cref="WidgetMessage.KeyLoseFocus"/> message.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Keyboard focus is being taken away from you.
        /// </para>
        /// <para>Dispatching: <see cref="DispatchMode.Direct"/></para>
        /// </remarks>
        /// <param name="takenByOtherWidget"><see langword="true"/> if focus is being taken by another widget; <see langword="false"/> if code requested to remove focus.</param>
        /// <returns><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</returns>
        protected virtual bool OnLostFocus(bool takenByOtherWidget) => false;

        /// <summary>
        /// Handles the <see cref="WidgetMessage.MouseDown"/> message.
        /// </summary>
        /// <remarks>
        /// <para>
        /// You receive one mousedown event per click.
        /// </para>
        /// <para>
        /// By accepting this you eat the click, otherwise your parent gets it. You will not receive drag and
        /// mouse up messages if you do not accept the down message.
        /// </para>
        /// <para>
        /// Handling this message consumes the mouse click, not handling it passes it to the next widget.
        /// You can act 'transparent' as a window by never handling mouse clicks to certain areas.
        /// </para>
        /// <para>
        /// Dispatching: <see cref="DispatchMode.UpChain"/>. NOTE: Technically this is direct dispatched, but the widgets library will shop
        /// it to each widget until one consumes the click, making it effectively "up chain".
        /// </para>
        /// </remarks>
        /// <returns><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</returns>
        protected virtual bool OnMouseDown(ref MouseState mouseState) => false;

        /// <summary>
        /// Handles the <see cref="WidgetMessage.MouseDrag"/> message.
        /// </summary>
        /// <remarks>
        /// <para>
        /// You receive a series of mouse drag messages (typically one per frame in the sim) as the mouse is
        /// moved once you have accepted a <see cref="WidgetMessage.MouseDown"/> message.
        /// You will continue to receive these until the mouse button is released.
        /// You may receive multiple mouse state messages with the same mouse position. You will receive mouse
        /// drag events even if the mouse is dragged out of your current or original bounds at the time of the
        /// mouse down.
        /// </para>
        /// <para>Dispatching: <see cref="DispatchMode.Direct"/>.</para>
        /// </remarks>
        /// <returns><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</returns>
        protected virtual bool OnMouseDrag(ref MouseState mouseState) => false;

        /// <summary>
        /// Handles the <see cref="WidgetMessage.MouseUp"/> message.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The mouseup event is sent once when the mouse button is released after a drag or click. You only
        /// receive this message if you accept the <see cref="WidgetMessage.MouseDown"/> message.
        /// </para>
        /// <para>
        /// Dispatching: <see cref="DispatchMode.Direct"/>.
        /// </para>
        /// </remarks>
        /// <returns><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</returns>
        protected virtual bool OnMouseUp(ref MouseState mouseState) => false;

        /// <summary>
        /// Handles the <see cref="WidgetMessage.Reshape"/> message.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Your geometry or a child's geometry is being changed.
        /// </para>
        /// <para>
        /// Dispatching: <see cref="DispatchMode.UpChain"/>.
        /// </para>
        /// </remarks>
        /// <param name="originalWidget">The reshaped widget.</param>
        /// <param name="change">The change.</param>
        /// <returns><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</returns>
        protected virtual bool OnReshape(Widget originalWidget, ref WidgetGeometryChange change) => false;

        /// <summary>
        /// Handles the <see cref="WidgetMessage.ExposedChanged"/> message.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Your exposed area has changed.
        /// </para>
        /// <para>
        /// Dispatching: <see cref="DispatchMode.Direct"/>.
        /// </para>
        /// </remarks>
        /// <returns><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</returns>
        protected virtual bool OnExposedChanged() => false;

        /// <summary>
        /// Handles the <see cref="WidgetMessage.AcceptChild"/> message.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A child has been added to you.
        /// </para>
        /// <para>
        /// Dispatching: <see cref="DispatchMode.Direct"/>.
        /// </para>
        /// </remarks>
        /// <returns><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</returns>
        protected virtual bool OnChildAdded(Widget child) => false;

        /// <summary>
        /// Handles the <see cref="WidgetMessage.LoseChild"/> message.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A child has been removed from to you.
        /// </para>
        /// <para>
        /// Dispatching: <see cref="DispatchMode.Direct"/>.
        /// </para>
        /// </remarks>
        /// <returns><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</returns>
        protected virtual bool OnChildRemoved(Widget child) => false;

        /// <summary>
        /// Handles the <see cref="WidgetMessage.AcceptParent"/> message.
        /// </summary>
        /// <remarks>
        /// <para>
        /// You now have a new parent, or have no parent.
        /// </para>
        /// <para>
        /// Dispatching: <see cref="DispatchMode.Direct"/>.
        /// </para>
        /// </remarks>
        /// <returns><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</returns>
        protected virtual bool OnParentChanged(Widget? parent) => false;

        /// <summary>
        /// Handles the <see cref="WidgetMessage.Shown"/> message.
        /// </summary>
        /// <remarks>
        /// <para>
        /// You or a child has been shown. 
        /// </para>
        /// <para>
        /// Note that this does not include you being shown because your parent
        /// was shown, you were put in a new parent, your root was shown, etc.
        /// </para>
        /// <para>
        /// Dispatching: <see cref="DispatchMode.UpChain"/>.
        /// </para>
        /// </remarks>
        /// <returns><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</returns>
        protected virtual bool OnShown(Widget shownWidget) => false;

        /// <summary>
        /// Handles the <see cref="WidgetMessage.Hidden"/> message.
        /// </summary>
        /// <remarks>
        /// <para>
        /// You have been hidden.
        /// </para>
        /// <para>
        /// Note that this does not include you being hidden because your parent
        /// was hidden, you were put in a new parent, your root was hidden, etc.
        /// </para>
        /// <para>
        /// Dispatching: <see cref="DispatchMode.UpChain"/>.
        /// </para>
        /// </remarks>
        /// <returns><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</returns>
        protected virtual bool OnHidden(Widget hiddenWidget) => false;

        /// <summary>
        /// Handles the <see cref="WidgetMessage.DescriptorChanged"/> message.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Your descriptor has changed.
        /// </para>
        /// <para>
        /// Dispatching: <see cref="DispatchMode.Direct"/>.
        /// </para>
        /// </remarks>
        /// <returns><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</returns>
        protected virtual bool OnDescriptorChanged() => false;

        /// <summary>
        /// Handles the <see cref="WidgetMessage.PropertyChanged"/> message.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A property has changed.
        /// </para>
        /// <para>
        /// Dispatching: <see cref="DispatchMode.Direct"/>.
        /// </para>
        /// </remarks>
        /// <returns><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</returns>
        protected virtual bool OnPropertyChanged(int propertyId, IntPtr value) => false;

        /// <summary>
        /// Handles the <see cref="WidgetMessage.MouseWheel"/> message.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The mouse wheel has moved.
        /// </para>
        /// <para>
        /// Return <see langword="true"/> to consume the mouse wheel move, or <see langword="false"/> to pass the message to a parent.
        /// </para>
        /// <para>
        /// Dispatching: <see cref="DispatchMode.UpChain"/>.
        /// </para>
        /// </remarks>
        /// <returns><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</returns>
        protected virtual bool OnMouseWheel(ref MouseState mouseState) => false;

        /// <summary>
        /// Handles the <see cref="WidgetMessage.CursorAdjust"/> message.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The cursor is over your widget. If you consume this message, change the <paramref name="cursorStatus"/>
        /// value to indicate the desired result.
        /// </para>
        /// <para>
        /// Return <see langword="true"/> to consume this message, or <see langword="false"/> to pass the message to a parent.
        /// </para>
        /// <para>
        /// Dispatching: <see cref="DispatchMode.UpChain"/>.
        /// </para>
        /// </remarks>
        /// <returns><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</returns>
        protected virtual bool OnCursorAdjust(ref MouseState mouseState, ref CursorStatus cursorStatus) => false;

        /// <summary>
        /// Handles custom messages.
        /// </summary>
        /// <param name="message">The message ID.</param>
        /// <param name="param1">The first message parameter.</param>
        /// <param name="param2">The second message parameter.</param>
        /// <returns><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</returns>
        protected virtual bool OnCustomMessage(int message, IntPtr param1, IntPtr param2) => false;
    }
}
