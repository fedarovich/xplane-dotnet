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
        private static readonly Dictionary<WidgetID, Widget> WidgetMap = new Dictionary<WidgetID, Widget>();

        private WidgetCollection? _children;
        private bool _acquiredWindow;
        private WindowBase? _window;
        private List<WidgetFuncCallback>? _callbacks;

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

        public bool IsRoot { get; }

        public WidgetID Id { get; private protected set; }

        public unsafe string Descriptor
        {
            get
            {
                int length = WidgetsAPI.GetWidgetDescriptor(Id, null, 0);
                byte* buffer = stackalloc byte[length + 1];
                return Marshal.PtrToStringUTF8(new IntPtr(buffer));
            }
            set => WidgetsAPI.SetWidgetDescriptor(Id, value);
        }

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

        public bool IsInFront => WidgetsAPI.IsWidgetInFront(Id) != 0;

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

        public IWidgetCollection Children => _children ??= new WidgetCollection(Id);

        public Widget? Parent
        {
            get
            {
                var parentId = WidgetsAPI.GetParentWidget(Id);
                return parentId == default ? null : GetOrCreate(parentId);
            }
            set => WidgetsAPI.PlaceWidgetWithin(Id, value?.Id ?? default);
        }

        public Widget? Root
        {
            get
            {
                var rootId = WidgetsAPI.FindRootWidget(Id);
                return rootId == default ? null : GetOrCreate(rootId);
            }
        }

        public void BringRootToFront() => WidgetsAPI.BringRootWidgetToFront(Id);

        public Widget? GetWidgetForLocation(int x, int y, bool recursive, bool onlyVisible)
        {
            var widgetId = WidgetsAPI.GetWidgetForLocation(Id, x, y, recursive.ToInt(), onlyVisible.ToInt());
            return widgetId == default ? null : GetOrCreate(widgetId);
        }

        #region Focus

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Widget? Focus() => Focus(this);

        public static Widget? Focus(Widget? widget)
        {
            var focusedId = WidgetsAPI.SetKeyboardFocus(widget?.Id ?? default);
            return focusedId == default ? null : GetOrCreate(focusedId);
        }

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SendMessage(WidgetMessage standardMessage, DispatchMode dispatchMode, IntPtr param1, IntPtr param2) => 
            WidgetsAPI.SendMessageToWidget(Id, standardMessage, dispatchMode, param1, param2) != 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SendMessage(int customMessage, DispatchMode dispatchMode, IntPtr param1, IntPtr param2) =>
            WidgetsAPI.SendMessageToWidget(Id, (WidgetMessage) customMessage, dispatchMode, param1, param2) != 0;

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetProperty(int customProperty, out IntPtr value) => TryGetProperty((WidgetPropertyID) customProperty, out value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IntPtr GetProperty(WidgetPropertyID standardProperty) =>
            TryGetProperty(standardProperty, out var value) ? value : throw new KeyNotFoundException($"Unable to find property '{standardProperty}'");

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IntPtr GetProperty(int customProperty) =>
            TryGetProperty(customProperty, out var value) ? value : throw new KeyNotFoundException($"Unable to find property '{customProperty}'");

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetProperty(WidgetPropertyID standardProperty, IntPtr value) => WidgetsAPI.SetWidgetProperty(Id, standardProperty, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetProperty(int customProperty, IntPtr value) => WidgetsAPI.SetWidgetProperty(Id, (WidgetPropertyID) customProperty, value);

        public void AddHook(WidgetFuncCallback hook)
        {
            (_callbacks ??= new List<WidgetFuncCallback>()).Add(hook);
            WidgetsAPI.AddWidgetCallback(Id, hook);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static WidgetFuncCallback GetWidgetClassFunction(WidgetClass widgetClass) => WidgetsAPI.GetWidgetClassFunc(widgetClass);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destroy(bool destroyChildren = true)
        {
            WidgetsAPI.DestroyWidget(Id, destroyChildren.ToInt());
        }

        private protected static void Register(Widget widget) => WidgetMap.Add(widget.Id, widget);

        private protected static void Unregister(WidgetID widgetId) => WidgetMap.Remove(widgetId);

        public static bool TryGetById(WidgetID widgetId, out Widget? widget) => WidgetMap.TryGetValue(widgetId, out widget);

        private protected static Widget GetOrCreate(WidgetID widgetId)
        {
            if (WidgetMap.TryGetValue(widgetId, out var widget))
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
