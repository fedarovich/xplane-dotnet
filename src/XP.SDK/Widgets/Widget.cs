#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using XP.SDK.Widgets.Internal;
using XP.SDK.XPLM;

namespace XP.SDK.Widgets
{
    public abstract class Widget
    {
        private static readonly Dictionary<WidgetID, Widget> _widgetMap = new Dictionary<WidgetID, Widget>();
        private static readonly ConditionalWeakTable<Widget, object> _tags = new ConditionalWeakTable<Widget, object>();

        private WidgetCollection? _children;
        private bool _acquiredWindow;
        private WindowBase? _window;
        private List<WidgetFuncCallback>? _hooks;
        private BehaviorCollection? _behaviors;

        private Widget(WidgetID id)
        {
            Id = id;
            IsRoot = WidgetsAPI.GetParentWidget(id) == id;
        }

        private protected Widget(bool isRoot, Widget? parent)
        {
            if (isRoot && parent != null)
                throw new ArgumentException("The parent must be null for a root widget.");
        }

        /// <summary>
        /// Gets the value indicating whether the widget is the root one.
        /// </summary>
        public bool IsRoot { get; }

        /// <summary>
        /// Gets the widget ID.
        /// </summary>
        public WidgetID Id { get; private protected set; }

        /// <summary>
        /// Gets the widget behaviors.
        /// </summary>
        public BehaviorCollection Behaviors => _behaviors ??= new BehaviorCollection(this);

        /// <summary>
        /// Gets or sets the widget descriptor.
        /// </summary>
        public unsafe string Descriptor
        {
            get
            {
                int length = WidgetsAPI.GetWidgetDescriptor(Id, null, 0);
                byte* buffer = stackalloc byte[length + 1];
                return Marshal.PtrToStringUTF8(new IntPtr(buffer)) ?? string.Empty;
            }
            set => WidgetsAPI.SetWidgetDescriptor(Id, value);
        }

        /// <summary>
        /// Gets or sets the widget geometry.
        /// </summary>
        public unsafe Rect Geometry
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                int left, top, right, bottom;
                WidgetsAPI.GetWidgetGeometry(Id, &left, &top,&right, &bottom);
                return new Rect(left, top, right, bottom);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => WidgetsAPI.SetWidgetGeometry(Id, value.Left, value.Top, value.Right, value.Bottom);
        }

        /// <summary>
        /// Get the bounds of the area of a widget that is completely within its parent widgets.
        /// </summary>
        /// <remarks>
        /// Since a widget's bounding box can be outside its
        /// parent, part of its area will not be elligible for mouse clicks and should
        /// not draw. Use <see cref="Geometry"/> to find out what area defines your
        /// widget's shape, but use this routine to find out what area to actually draw
        /// into. Note that the widget library does not use OpenGL clipping to keep
        /// frame rates up, although you could use it internally.
        /// </remarks>
        public unsafe Rect ExposedGeometry
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                int left, top, right, bottom;
                WidgetsAPI.GetWidgetExposedGeometry(Id, &left, &top, &right, &bottom);
                return new Rect(left, top, right, bottom);
            }
        }

        /// <summary>
        /// Gets the value indicating whether this widget's hierarchy is the front most hierarchy.                 
        /// </summary>
        public bool IsInFront => WidgetsAPI.IsWidgetInFront(Id) != 0;

        /// <summary>
        /// Gets or sets the value indicating whether the widget is visible.
        /// </summary>
        /// <remarks>
        /// This property takes into consideration whether a parent is invisible.
        /// Use it to tell if the user can see the widget.
        /// </remarks>
        /// <seealso cref="Show"/>
        /// <seealso cref="Hide"/>
        public bool IsVisible
        {
            get => WidgetsAPI.IsWidgetVisible(Id) != 0;
            set
            {
                if (value)
                {
                    WidgetsAPI.ShowWidget(Id);
                }
                else
                {
                    WidgetsAPI.HideWidget(Id);
                }
            }
        }

        /// <summary>
        /// Gets the the window (<see cref="WindowBase"/>) that backs your widget window.
        /// </summary>
        public WindowBase? Window
        {
            get
            {
                if (!_acquiredWindow)
                {
                    _acquiredWindow = true;
                    var windowId = WidgetsAPI.GetWidgetUnderlyingWindow(Id);
                    _window = windowId != default ? WindowBase.FromId(windowId) : null;
                }

                return _window;
            }
        }

        /// <summary>
        /// Gets the child widgets.
        /// </summary>
        public IWidgetCollection Children => _children ??= new WidgetCollection(Id);

        /// <summary>
        /// Gets or sets the parent widget.
        /// </summary>
        public Widget? Parent
        {
            get
            {
                var parentId = WidgetsAPI.GetParentWidget(Id);
                return parentId == default ? null : GetOrCreate(parentId);
            }
            set => WidgetsAPI.PlaceWidgetWithin(Id, value?.Id ?? default);
        }

        /// <summary>
        /// Gets the root widget.
        /// </summary>
        public Widget? Root
        {
            get
            {
                var rootId = WidgetsAPI.FindRootWidget(Id);
                return rootId == default ? null : GetOrCreate(rootId);
            }
        }

        /// <summary>
        /// Gets or sets an arbitrary object that can be associated with the current widget.
        /// </summary>
        public object? Tag
        {
            get => _tags.TryGetValue(this, out var tag) ? tag : null;
            set
            {
                if (value != null)
                {
                    _tags.AddOrUpdate(this, value);
                }
                else
                {
                    _tags.Remove(this);
                }
            }
        }

        /// <summary>
        /// If <see langword="true"/> the widget package will use OpenGL to restrict drawing to the Widget's exposed rectangle.
        /// </summary>
        /// <seealso cref="ExposedGeometry"/>
        public bool Clip
        {
            get => GetProperty(WidgetPropertyID.Clip) != default;
            set => SetProperty(WidgetPropertyID.Clip, new IntPtr(value.ToInt()));
        }

        /// <summary>
        /// Gets the value indicating whether the widget is enabled.
        /// </summary>
        public bool IsEnabled
        {
            get => GetProperty(WidgetPropertyID.Enabled) != default;
            set => SetProperty(WidgetPropertyID.Enabled, new IntPtr(value.ToInt()));
        }

        /// <summary>
        /// This routine makes a widget visible if it is not already. Note that if a
        /// widget is not in a rooted widget hierarchy or one of its parents is not
        /// visible, it will still not be visible to the user.
        /// </summary>
        public void Show() => WidgetsAPI.ShowWidget(Id);

        /// <summary>
        /// Makes a widget invisible.
        /// </summary>
        public void Hide() => WidgetsAPI.HideWidget(Id);

        /// <summary>
        /// This routine makes the specified widget be in the front most widget
        /// hierarchy. If this widget is a root widget, its widget hierarchy comes to
        /// front, otherwise the widget's root is brought to the front. If this widget
        /// is not in an active widget hierarchy (e.g. there is no root widget at the
        /// top of the tree), this routine does nothing.
        /// </summary>
        public void BringRootToFront() => WidgetsAPI.BringRootWidgetToFront(Id);

        /// <summary>
        /// <para>
        /// Given a widget and a location, this routine returns the <see cref="Widget"/> of the
        /// child of that widget that owns that location. If <paramref name="recursive"/> is <see langword="true" /> then
        /// this will return a child of a child of a widget as it tries to find the
        /// deepest widget at that location. If <paramref name="onlyVisible"/> is true, then only
        /// visible widgets are considered, otherwise all widgets are considered. The
        /// current widget will be returned if the location is in
        /// that widget but not in a child widget. <see langword="null" /> is returned if the location is not
        /// in the container.
        /// </para>
        /// <para>
        /// NOTE: if a widget's geometry extends outside its parents geometry, it will
        /// not be returned by this call for mouse locations outside the parent
        /// geometry. The parent geometry limits the child's eligibility for mouse
        /// location.
        /// </para>
        /// </summary>
        public Widget? GetWidgetForLocation(int x, int y, bool recursive, bool onlyVisible)
        {
            var widgetId = WidgetsAPI.GetWidgetForLocation(Id, x, y, recursive.ToInt(), onlyVisible.ToInt());
            return widgetId == default ? null : GetOrCreate(widgetId);
        }

        #region Focus

        /// <summary>
        /// Makes the current widget to receive keyboard strokes.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Note that if the widget does not care about
        /// keystrokes, they will go to the parent widget, and if no widget cares about
        /// them, they go to X-Plane.
        /// </para>
        /// <para>
        /// Keyboard focus is not changed if this widget will not accept it.
        /// </para>
        /// </remarks>
        /// <returns>
        /// The widget that ended up with keyboard focus, or <see langword="null" /> for X-Plane.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Widget? Focus() => Focus(this);

        /// <summary>
        /// Sets the widget than will receive keyboard strokes.
        /// </summary>
        /// <param name="widget">The widget to focus, or <see langword="null" /> to make X-Plane receive the focus.</param>
        /// <remarks>
        /// <para>
        /// Note that if the widget does not care about
        /// keystrokes, they will go to the parent widget, and if no widget cares about
        /// them, they go to X-Plane.
        /// </para>
        /// <para>
        /// Keyboard focus is not changed if the new widget will not accept it. For
        /// setting to X-Plane, keyboard focus is always accepted.
        /// </para>
        /// </remarks>
        /// <returns>
        /// The widget that ended up with keyboard focus, or <see langword="null" /> for X-Plane.
        /// </returns>
        public static Widget? Focus(Widget? widget)
        {
            var focusedId = WidgetsAPI.SetKeyboardFocus(widget?.Id ?? default);
            return focusedId == default ? null : GetOrCreate(focusedId);
        }

        /// <summary>
        /// This causes the specified widget to lose focus; focus is passed to its
        /// parent, or the next parent that will accept it. This routine does nothing
        /// if this widget does not have focus.
        /// </summary>
        public void LoseFocus() => WidgetsAPI.LoseKeyboardFocus(Id);

        public static Widget? FocusedWidget
        {
            get
            {
                var focusedId = WidgetsAPI.GetWidgetWithFocus();
                return focusedId != default ? null : GetOrCreate(focusedId);
            }
        }

        #endregion

        /// <summary>
        /// Sends a message to this widget.
        /// </summary>
        /// <remarks>
        /// <para>
        /// You should probably not go around
        /// simulating the predefined messages that the widgets library defines for
        /// you. You may however define custom messages for your widgets and send them
        /// with this method.
        /// </para>
        /// <para>
        /// For each widget that receives the message (see the dispatching modes), each
        /// widget function from the most recently installed to the oldest one receives
        /// the message in order until it is handled.
        /// </para>
        /// </remarks>
        /// <param name="standardMessage">The message to send.</param>
        /// <param name="dispatchMode">The dispatch mode.</param>
        /// <param name="param1">The first message parameter.</param>
        /// <param name="param2">The second message parameter.</param>
        /// <returns><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SendMessage(WidgetMessage standardMessage, DispatchMode dispatchMode, IntPtr param1 = default, IntPtr param2 = default) => 
            WidgetsAPI.SendMessageToWidget(Id, standardMessage, dispatchMode, param1, param2) != 0;

        /// <summary>
        /// Sends a message to this widget.
        /// </summary>
        /// <remarks>
        /// <para>
        /// You should probably not go around
        /// simulating the predefined messages that the widgets library defines for
        /// you. You may however define custom messages for your widgets and send them
        /// with this method.
        /// </para>
        /// <para>
        /// For each widget that receives the message (see the dispatching modes), each
        /// widget function from the most recently installed to the oldest one receives
        /// the message in order until it is handled.
        /// </para>
        /// </remarks>
        /// <param name="customMessage">The message to send.</param>
        /// <param name="dispatchMode">The dispatch mode.</param>
        /// <param name="param1">The first message parameter.</param>
        /// <param name="param2">The second message parameter.</param>
        /// <returns><see langword="true"/> if the message was handled; <see langword="false"/> otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SendMessage(int customMessage, DispatchMode dispatchMode, IntPtr param1 = default, IntPtr param2 = default) =>
            WidgetsAPI.SendMessageToWidget(Id, (WidgetMessage) customMessage, dispatchMode, param1, param2) != 0;

        /// <summary>
        /// Tries to get the property value.
        /// </summary>
        /// <param name="standardProperty">The property ID.</param>
        /// <param name="value">The property value.</param>
        /// <returns><see langword="true"/> if the property exists; <see langword="false"/> otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe bool TryGetProperty(WidgetPropertyID standardProperty, out IntPtr value)
        {
            int exists = 0;
            var result = WidgetsAPI.GetWidgetProperty(Id, standardProperty, &exists);
            if (exists != 0)
            {
                value = result;
                return true;
            }
            else
            {
                value = IntPtr.Zero;
                return false;
            }
        }

        /// <summary>
        /// Tries to get the property value.
        /// </summary>
        /// <param name="customProperty">The property ID.</param>
        /// <param name="value">The property value.</param>
        /// <returns><see langword="true"/> if the property exists; <see langword="false"/> otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetProperty(int customProperty, out IntPtr value) => TryGetProperty((WidgetPropertyID) customProperty, out value);

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="standardProperty">The property ID.</param>
        /// <returns>The property value.</returns>
        /// <exception cref="KeyNotFoundException">The property with the specified ID does not exist.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IntPtr GetProperty(WidgetPropertyID standardProperty) =>
            TryGetProperty(standardProperty, out var value) ? value : throw new KeyNotFoundException($"Unable to find property '{standardProperty}'");

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="customProperty">The property ID.</param>
        /// <returns>The property value.</returns>
        /// <exception cref="KeyNotFoundException">The property with the specified ID does not exist.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IntPtr GetProperty(int customProperty) =>
            TryGetProperty(customProperty, out var value) ? value : throw new KeyNotFoundException($"Unable to find property '{customProperty}'");

        /// <summary>
        /// Sets the property value.
        /// </summary>
        /// <param name="standardProperty">The property ID.</param>
        /// <param name="value">The value to set.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetProperty(WidgetPropertyID standardProperty, IntPtr value) => WidgetsAPI.SetWidgetProperty(Id, standardProperty, value);

        /// <summary>
        /// Sets the property value.
        /// </summary>
        /// <param name="customProperty">The property ID.</param>
        /// <param name="value">The value to set.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetProperty(int customProperty, IntPtr value) => WidgetsAPI.SetWidgetProperty(Id, (WidgetPropertyID) customProperty, value);

        /// <summary>
        /// Adds a widget proc hook.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This hook supersedes any existing ones and will receive messages first;
        /// if it does not handle messages they will go on to be handled by pre-existing widgets.
        /// </para>
        /// <para>
        /// The widget function will remain on the widget for the life of the widget.
        /// The creation message will be sent to the new callback immediately with the
        /// widget ID, and the destruction message will be sent before the other widget
        /// function receives a destruction message.
        /// </para>
        /// <para>Consider using <see cref="Behaviors"/> which provide higher-lever abstraction for this functionality.</para>
        /// </remarks>
        /// <param name="hook">The hook.</param>
        public void AddHook(WidgetFuncCallback hook)
        {
            (_hooks ??= new List<WidgetFuncCallback>()).Add(hook);
            WidgetsAPI.AddWidgetCallback(Id, hook);
        }

        /// <summary>
        /// Simply moves a widget by an amount, +x = right, +y=up, without resizing the  widget.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void MoveBy(int deltaX, int deltaY) => WidgetUtilsAPI.MoveWidgetBy(Id, deltaX, deltaY);

        /// <summary>
        /// Given a widget class, this function returns the callbacks that power that widget class.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static WidgetFuncCallback GetWidgetClassFunction(WidgetClass widgetClass) => WidgetsAPI.GetWidgetClassFunc(widgetClass);

        /// <summary>
        /// Destroys the widget.
        /// </summary>
        /// <param name="destroyChildren">
        /// <para>If <see langword="true"/>, the widget's children will be destroyed first,
        /// then this widget will be destroyed. Furthermore, the widget's children
        /// will be destroyed with the inDestroyChildren flag set to 1, so the
        /// destruction will recurse down the widget tree.
        /// </para>
        /// <para>
        /// If <see langword="false"/>, the child widgets will simply end up with their parent set to <see langword="null" />. 
        /// </para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destroy(bool destroyChildren = true)
        {
            WidgetsAPI.DestroyWidget(Id, destroyChildren.ToInt());
        }

        private protected static void Register(Widget widget) => _widgetMap.Add(widget.Id, widget);

        private protected static void Unregister(WidgetID widgetId) => _widgetMap.Remove(widgetId);

        public static bool TryGetById(WidgetID widgetId, out Widget? widget) => _widgetMap.TryGetValue(widgetId, out widget);

        internal static Widget GetOrCreate(WidgetID widgetId)
        {
            if (_widgetMap.TryGetValue(widgetId, out var widget))
                return widget;

            widget = new UnknownWidget(widgetId);
            Register(widget);
            return widget;
        }

        private class UnknownWidget : Widget
        {
            public UnknownWidget(WidgetID id) : base(id)
            {
            }
        }

        private class WidgetCollection : IWidgetCollection
        {
            private readonly WidgetID _parentId;

            public WidgetCollection(WidgetID parentId) => _parentId = parentId;

            public IEnumerator<Widget> GetEnumerator()
            {
                var count = Count;
                for (int index = 0; index < count; index++)
                {
                    var id = WidgetsAPI.GetNthChildWidget(_parentId, index);
                    if (id == default)
                        throw new ArgumentException("Invalid index.", nameof(index));

                    yield return GetOrCreate(id);
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public int Count => WidgetsAPI.CountChildWidgets(_parentId);

            public Widget this[int index]
            {
                get
                {
                    if (index < 0 || index >= Count)
                        throw new ArgumentOutOfRangeException(nameof(index));

                    var id = WidgetsAPI.GetNthChildWidget(_parentId, index);
                    if (id == default)
                        throw new ArgumentException("Invalid index.", nameof(index));

                    return GetOrCreate(id);
                }
            }

            public void Add(Widget widget)
            {
                if (widget == null) 
                    throw new ArgumentNullException(nameof(widget));
                
                WidgetsAPI.PlaceWidgetWithin(widget.Id, _parentId);
            }

            public void Remove(Widget widget)
            {
                if (widget == null)
                    throw new ArgumentNullException(nameof(widget));

                WidgetsAPI.PlaceWidgetWithin(widget.Id, default);
            }
        }
    }
}
