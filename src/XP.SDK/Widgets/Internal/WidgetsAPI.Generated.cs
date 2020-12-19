using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using InlineIL;
using XP.SDK.XPLM;

namespace XP.SDK.Widgets.Internal
{
    public static partial class WidgetsAPI
    {
        
        /// <summary>
        /// <para>
        /// This function creates a new widget and returns the new widget's ID to you.
        /// If the widget creation fails for some reason, it returns NULL. Widget
        /// creation will fail either if you pass a bad class ID or if there is not
        /// adequate memory.
        /// </para>
        /// <para>
        /// Input Parameters:
        /// </para>
        /// <para>
        /// - Top, left, bottom, and right in global screen coordinates defining the
        /// widget's location on the screen.
        /// - inVisible is 1 if the widget should be drawn, 0 to start the widget as
        /// hidden.
        /// - inDescriptor is a null terminated string that will become the widget's
        /// descriptor.
        /// - inIsRoot is 1 if this is going to be a root widget, 0 if it will not be.
        /// - inContainer is the ID of this widget's container. It must be 0 for a root
        /// widget. for a non-root widget, pass the widget ID of the widget to place
        /// this widget within. If this widget is not going to start inside another
        /// widget, pass 0; this new widget will then just be floating off in space
        /// (and will not be drawn until it is placed in a widget.
        /// - inClass is the class of the widget to draw. Use one of the predefined
        /// class-IDs to create a standard widget.
        /// </para>
        /// <para>
        /// A note on widget embedding: a widget is only called (and will be drawn,
        /// etc.) if it is placed within a widget that will be called. Root widgets are
        /// always called. So it is possible to have whole chains of widgets that are
        /// simply not called. You can preconstruct widget trees and then place them
        /// into root widgets later to activate them if you wish.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPCreateWidget", ExactSpelling = true)]
        public static extern unsafe WidgetID CreateWidget(int inLeft, int inTop, int inRight, int inBottom, int inVisible, byte* inDescriptor, int inIsRoot, WidgetID inContainer, WidgetClass inClass);

        
        /// <summary>
        /// <para>
        /// This function creates a new widget and returns the new widget's ID to you.
        /// If the widget creation fails for some reason, it returns NULL. Widget
        /// creation will fail either if you pass a bad class ID or if there is not
        /// adequate memory.
        /// </para>
        /// <para>
        /// Input Parameters:
        /// </para>
        /// <para>
        /// - Top, left, bottom, and right in global screen coordinates defining the
        /// widget's location on the screen.
        /// - inVisible is 1 if the widget should be drawn, 0 to start the widget as
        /// hidden.
        /// - inDescriptor is a null terminated string that will become the widget's
        /// descriptor.
        /// - inIsRoot is 1 if this is going to be a root widget, 0 if it will not be.
        /// - inContainer is the ID of this widget's container. It must be 0 for a root
        /// widget. for a non-root widget, pass the widget ID of the widget to place
        /// this widget within. If this widget is not going to start inside another
        /// widget, pass 0; this new widget will then just be floating off in space
        /// (and will not be drawn until it is placed in a widget.
        /// - inClass is the class of the widget to draw. Use one of the predefined
        /// class-IDs to create a standard widget.
        /// </para>
        /// <para>
        /// A note on widget embedding: a widget is only called (and will be drawn,
        /// etc.) if it is placed within a widget that will be called. Root widgets are
        /// always called. So it is possible to have whole chains of widgets that are
        /// simply not called. You can preconstruct widget trees and then place them
        /// into root widgets later to activate them if you wish.
        /// </para>
        /// </summary>
        [SkipLocalsInitAttribute]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe WidgetID CreateWidget(int inLeft, int inTop, int inRight, int inBottom, int inVisible, in ReadOnlySpan<char> inDescriptor, int inIsRoot, WidgetID inContainer, WidgetClass inClass)
        {
            Span<byte> inDescriptorUtf8 = stackalloc byte[(inDescriptor.Length << 1) | 1];
            var inDescriptorPtr = Utils.ToUtf8Unsafe(inDescriptor, inDescriptorUtf8);
            return CreateWidget(inLeft, inTop, inRight, inBottom, inVisible, inDescriptorPtr, inIsRoot, inContainer, inClass);
        }

        
        /// <summary>
        /// <para>
        /// This function is the same as XPCreateWidget except that instead of passing
        /// a class ID, you pass your widget callback function pointer defining the
        /// widget. Use this function to define a custom widget. All parameters are the
        /// same as XPCreateWidget, except that the widget class has been replaced with
        /// the widget function.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPCreateCustomWidget", ExactSpelling = true)]
        public static extern unsafe WidgetID CreateCustomWidget(int inLeft, int inTop, int inRight, int inBottom, int inVisible, byte* inDescriptor, int inIsRoot, WidgetID inContainer, delegate* unmanaged[Cdecl]<WidgetMessage, WidgetID, nint, nint, int> inCallback);

        
        /// <summary>
        /// <para>
        /// This function is the same as XPCreateWidget except that instead of passing
        /// a class ID, you pass your widget callback function pointer defining the
        /// widget. Use this function to define a custom widget. All parameters are the
        /// same as XPCreateWidget, except that the widget class has been replaced with
        /// the widget function.
        /// </para>
        /// </summary>
        [SkipLocalsInitAttribute]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe WidgetID CreateCustomWidget(int inLeft, int inTop, int inRight, int inBottom, int inVisible, in ReadOnlySpan<char> inDescriptor, int inIsRoot, WidgetID inContainer, delegate* unmanaged[Cdecl]<WidgetMessage, WidgetID, nint, nint, int> inCallback)
        {
            Span<byte> inDescriptorUtf8 = stackalloc byte[(inDescriptor.Length << 1) | 1];
            var inDescriptorPtr = Utils.ToUtf8Unsafe(inDescriptor, inDescriptorUtf8);
            return CreateCustomWidget(inLeft, inTop, inRight, inBottom, inVisible, inDescriptorPtr, inIsRoot, inContainer, inCallback);
        }

        
        /// <summary>
        /// <para>
        /// This class destroys a widget. Pass in the ID of the widget to kill. If you
        /// pass 1 for inDestroyChilren, the widget's children will be destroyed first,
        /// then this widget will be destroyed. (Furthermore, the widget's children
        /// will be destroyed with the inDestroyChildren flag set to 1, so the
        /// destruction will recurse down the widget tree.) If you pass 0 for this
        /// flag, the child widgets will simply end up with their parent set to 0.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPDestroyWidget", ExactSpelling = true)]
        public static extern void DestroyWidget(WidgetID inWidget, int inDestroyChildren);

        
        /// <summary>
        /// <para>
        /// This sends any message to a widget. You should probably not go around
        /// simulating the predefined messages that the widgets library defines for
        /// you. You may however define custom messages for your widgets and send them
        /// with this method.
        /// </para>
        /// <para>
        /// This method supports several dispatching patterns; see XPDispatchMode for
        /// more info. The function returns 1 if the message was handled, 0 if it was
        /// not.
        /// </para>
        /// <para>
        /// For each widget that receives the message (see the dispatching modes), each
        /// widget function from the most recently installed to the oldest one receives
        /// the message in order until it is handled.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPSendMessageToWidget", ExactSpelling = true)]
        public static extern int SendMessageToWidget(WidgetID inWidget, WidgetMessage inMessage, DispatchMode inMode, nint inParam1, nint inParam2);

        
        /// <summary>
        /// <para>
        /// This function changes which container a widget resides in. You may NOT use
        /// this function on a root widget! inSubWidget is the widget that will be
        /// moved. Pass a widget ID in inContainer to make inSubWidget be a child of
        /// inContainer. It will become the last/closest widget in the container. Pass
        /// 0 to remove the widget from any container. Any call to this other than
        /// passing the widget ID of the old parent of the affected widget will cause
        /// the widget to be removed from its old parent. Placing a widget within its
        /// own parent simply makes it the last widget.
        /// </para>
        /// <para>
        /// NOTE: this routine does not reposition the sub widget in global
        /// coordinates. If the container has layout management code, it will
        /// reposition the subwidget for you, otherwise you must do it with
        /// SetWidgetGeometry.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPPlaceWidgetWithin", ExactSpelling = true)]
        public static extern void PlaceWidgetWithin(WidgetID inSubWidget, WidgetID inContainer);

        
        /// <summary>
        /// <para>
        /// This routine returns the number of widgets another widget contains.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPCountChildWidgets", ExactSpelling = true)]
        public static extern int CountChildWidgets(WidgetID inWidget);

        
        /// <summary>
        /// <para>
        /// This routine returns the widget ID of a child widget by index. Indexes are
        /// 0 based, from 0 to one minus the number of widgets in the parent,
        /// inclusive. If the index is invalid, 0 is returned.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPGetNthChildWidget", ExactSpelling = true)]
        public static extern WidgetID GetNthChildWidget(WidgetID inWidget, int inIndex);

        
        /// <summary>
        /// <para>
        /// Returns the parent of a widget, or 0 if the widget has no parent. Root
        /// widgets never have parents and therefore always return 0.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPGetParentWidget", ExactSpelling = true)]
        public static extern WidgetID GetParentWidget(WidgetID inWidget);

        
        /// <summary>
        /// <para>
        /// This routine makes a widget visible if it is not already. Note that if a
        /// widget is not in a rooted widget hierarchy or one of its parents is not
        /// visible, it will still not be visible to the user.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPShowWidget", ExactSpelling = true)]
        public static extern void ShowWidget(WidgetID inWidget);

        
        /// <summary>
        /// <para>
        /// Makes a widget invisible. See XPShowWidget for considerations of when a
        /// widget might not be visible despite its own visibility state.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPHideWidget", ExactSpelling = true)]
        public static extern void HideWidget(WidgetID inWidget);

        
        /// <summary>
        /// <para>
        /// This returns 1 if a widget is visible, 0 if it is not. Note that this
        /// routine takes into consideration whether a parent is invisible. Use this
        /// routine to tell if the user can see the widget.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPIsWidgetVisible", ExactSpelling = true)]
        public static extern int IsWidgetVisible(WidgetID inWidget);

        
        /// <summary>
        /// <para>
        /// Returns the Widget ID of the root widget that contains the passed in widget
        /// or NULL if the passed in widget is not in a rooted hierarchy.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPFindRootWidget", ExactSpelling = true)]
        public static extern WidgetID FindRootWidget(WidgetID inWidget);

        
        /// <summary>
        /// <para>
        /// This routine makes the specified widget be in the front most widget
        /// hierarchy. If this widget is a root widget, its widget hierarchy comes to
        /// front, otherwise the widget's root is brought to the front. If this widget
        /// is not in an active widget hiearchy (e.g. there is no root widget at the
        /// top of the tree), this routine does nothing.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPBringRootWidgetToFront", ExactSpelling = true)]
        public static extern void BringRootWidgetToFront(WidgetID inWidget);

        
        /// <summary>
        /// <para>
        /// This routine returns true if this widget's hierarchy is the front most
        /// hierarchy. It returns false if the widget's hierarchy is not in front, or
        /// if the widget is not in a rooted hierarchy.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPIsWidgetInFront", ExactSpelling = true)]
        public static extern int IsWidgetInFront(WidgetID inWidget);

        
        /// <summary>
        /// <para>
        /// This routine returns the bounding box of a widget in global coordinates.
        /// Pass NULL for any parameter you are not interested in.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPGetWidgetGeometry", ExactSpelling = true)]
        public static extern unsafe void GetWidgetGeometry(WidgetID inWidget, int* outLeft, int* outTop, int* outRight, int* outBottom);

        
        /// <summary>
        /// <para>
        /// This function changes the bounding box of a widget.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPSetWidgetGeometry", ExactSpelling = true)]
        public static extern void SetWidgetGeometry(WidgetID inWidget, int inLeft, int inTop, int inRight, int inBottom);

        
        /// <summary>
        /// <para>
        /// Given a widget and a location, this routine returns the widget ID of the
        /// child of that widget that owns that location. If inRecursive is true then
        /// this will return a child of a child of a widget as it tries to find the
        /// deepest widget at that location. If inVisibleOnly is true, then only
        /// visible widgets are considered, otherwise all widgets are considered. The
        /// widget ID passed for inContainer will be returned if the location is in
        /// that widget but not in a child widget. 0 is returned if the location is not
        /// in the container.
        /// </para>
        /// <para>
        /// NOTE: if a widget's geometry extends outside its parents geometry, it will
        /// not be returned by this call for mouse locations outside the parent
        /// geometry. The parent geometry limits the child's eligibility for mouse
        /// location.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPGetWidgetForLocation", ExactSpelling = true)]
        public static extern WidgetID GetWidgetForLocation(WidgetID inContainer, int inXOffset, int inYOffset, int inRecursive, int inVisibleOnly);

        
        /// <summary>
        /// <para>
        /// This routine returns the bounds of the area of a widget that is completely
        /// within its parent widgets. Since a widget's bounding box can be outside its
        /// parent, part of its area will not be elligible for mouse clicks and should
        /// not draw. Use XPGetWidgetGeometry to find out what area defines your
        /// widget's shape, but use this routine to find out what area to actually draw
        /// into. Note that the widget library does not use OpenGL clipping to keep
        /// frame rates up, although you could use it internally.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPGetWidgetExposedGeometry", ExactSpelling = true)]
        public static extern unsafe void GetWidgetExposedGeometry(WidgetID inWidgetID, int* outLeft, int* outTop, int* outRight, int* outBottom);

        
        /// <summary>
        /// <para>
        /// Every widget has a descriptor, which is a text string. What the text string
        /// is used for varies from widget to widget; for example, a push button's text
        /// is its descriptor, a caption shows its descriptor, and a text field's
        /// descriptor is the text being edited. In other words, the usage for the text
        /// varies from widget to widget, but this API provides a universal and
        /// convenient way to get at it. While not all UI widgets need their
        /// descriptor, many do.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPSetWidgetDescriptor", ExactSpelling = true)]
        public static extern unsafe void SetWidgetDescriptor(WidgetID inWidget, byte* inDescriptor);

        
        /// <summary>
        /// <para>
        /// Every widget has a descriptor, which is a text string. What the text string
        /// is used for varies from widget to widget; for example, a push button's text
        /// is its descriptor, a caption shows its descriptor, and a text field's
        /// descriptor is the text being edited. In other words, the usage for the text
        /// varies from widget to widget, but this API provides a universal and
        /// convenient way to get at it. While not all UI widgets need their
        /// descriptor, many do.
        /// </para>
        /// </summary>
        [SkipLocalsInitAttribute]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetWidgetDescriptor(WidgetID inWidget, in ReadOnlySpan<char> inDescriptor)
        {
            Span<byte> inDescriptorUtf8 = stackalloc byte[(inDescriptor.Length << 1) | 1];
            var inDescriptorPtr = Utils.ToUtf8Unsafe(inDescriptor, inDescriptorUtf8);
            SetWidgetDescriptor(inWidget, inDescriptorPtr);
        }

        
        /// <summary>
        /// <para>
        /// This routine returns the widget's descriptor. Pass in the length of the
        /// buffer you are going to receive the descriptor in. The descriptor will be
        /// null terminated for you. This routine returns the length of the actual
        /// descriptor; if you pass NULL for outDescriptor, you can get the
        /// descriptor's length without getting its text. If the length of the
        /// descriptor exceeds your buffer length, the buffer will not be null
        /// terminated (this routine has 'strncpy' semantics).
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPGetWidgetDescriptor", ExactSpelling = true)]
        public static extern unsafe int GetWidgetDescriptor(WidgetID inWidget, byte* outDescriptor, int inMaxDescLength);

        
        /// <summary>
        /// <para>
        /// Returns the window (from the XPLMDisplay API) that backs your widget
        /// window. If you have opted in to modern windows, via a call to
        /// XPLMEnableFeature("XPLM_USE_NATIVE_WIDGET_WINDOWS", 1), you can use the
        /// returned window ID for display APIs like XPLMSetWindowPositioningMode(),
        /// allowing you to pop the widget window out into a real OS window, or move it
        /// into VR.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPGetWidgetUnderlyingWindow", ExactSpelling = true)]
        public static extern WindowID GetWidgetUnderlyingWindow(WidgetID inWidget);

        
        /// <summary>
        /// <para>
        /// This function sets a widget's property. Properties are arbitrary values
        /// associated by a widget by ID.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPSetWidgetProperty", ExactSpelling = true)]
        public static extern void SetWidgetProperty(WidgetID inWidget, WidgetPropertyID inProperty, nint inValue);

        
        /// <summary>
        /// <para>
        /// This routine returns the value of a widget's property, or 0 if the property
        /// is not defined. If you need to know whether the property is defined, pass a
        /// pointer to an int for inExists; the existence of that property will be
        /// returned in the int. Pass NULL for inExists if you do not need this
        /// information.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPGetWidgetProperty", ExactSpelling = true)]
        public static extern unsafe nint GetWidgetProperty(WidgetID inWidget, WidgetPropertyID inProperty, int* inExists);

        
        /// <summary>
        /// <para>
        /// Controls which widget will receive keystrokes. Pass the widget ID of the
        /// widget to get the keys. Note that if the widget does not care about
        /// keystrokes, they will go to the parent widget, and if no widget cares about
        /// them, they go to X-Plane.
        /// </para>
        /// <para>
        /// If you set the keyboard focus to widget ID 0, X-Plane gets keyboard focus.
        /// </para>
        /// <para>
        /// This routine returns the widget ID that ended up with keyboard focus, or 0
        /// for X-Plane.
        /// </para>
        /// <para>
        /// Keyboard focus is not changed if the new widget will not accept it. For
        /// setting to X-Plane, keyboard focus is always accepted.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPSetKeyboardFocus", ExactSpelling = true)]
        public static extern WidgetID SetKeyboardFocus(WidgetID inWidget);

        
        /// <summary>
        /// <para>
        /// This causes the specified widget to lose focus; focus is passed to its
        /// parent, or the next parent that will accept it. This routine does nothing
        /// if this widget does not have focus.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLoseKeyboardFocus", ExactSpelling = true)]
        public static extern void LoseKeyboardFocus(WidgetID inWidget);

        
        /// <summary>
        /// <para>
        /// This routine returns the widget that has keyboard focus, or 0 if X-Plane
        /// has keyboard focus or some other plugin window that does not have widgets
        /// has focus.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPGetWidgetWithFocus", ExactSpelling = true)]
        public static extern WidgetID GetWidgetWithFocus();

        
        /// <summary>
        /// <para>
        /// This function adds a new widget callback to a widget. This widget callback
        /// supercedes any existing ones and will receive messages first; if it does
        /// not handle messages they will go on to be handled by pre-existing widgets.
        /// </para>
        /// <para>
        /// The widget function will remain on the widget for the life of the widget.
        /// The creation message will be sent to the new callback immediately with the
        /// widget ID, and the destruction message will be sent before the other widget
        /// function receives a destruction message.
        /// </para>
        /// <para>
        /// This provides a way to 'subclass' an existing widget. By providing a second
        /// hook that only handles certain widget messages, you can customize or extend
        /// widget behavior.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPAddWidgetCallback", ExactSpelling = true)]
        public static extern unsafe void AddWidgetCallback(WidgetID inWidget, delegate* unmanaged[Cdecl]<WidgetMessage, WidgetID, nint, nint, int> inNewCallback);

        
        /// <summary>
        /// <para>
        /// Given a widget class, this function returns the callbacks that power that
        /// widget class.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPGetWidgetClassFunc", ExactSpelling = true)]
        public static extern WidgetFuncCallback GetWidgetClassFunc(WidgetClass inWidgetClass);
    }
}