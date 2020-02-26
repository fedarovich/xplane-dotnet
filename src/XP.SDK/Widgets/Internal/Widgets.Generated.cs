using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using InlineIL;
using XP.SDK.XPLM.Internal;

namespace XP.SDK.Widgets.Internal
{
    public static partial class Widgets
    {
        private static IntPtr CreateWidgetPtr;
        private static IntPtr CreateCustomWidgetPtr;
        private static IntPtr DestroyWidgetPtr;
        private static IntPtr SendMessageToWidgetPtr;
        private static IntPtr PlaceWidgetWithinPtr;
        private static IntPtr CountChildWidgetsPtr;
        private static IntPtr GetNthChildWidgetPtr;
        private static IntPtr GetParentWidgetPtr;
        private static IntPtr ShowWidgetPtr;
        private static IntPtr HideWidgetPtr;
        private static IntPtr IsWidgetVisiblePtr;
        private static IntPtr FindRootWidgetPtr;
        private static IntPtr BringRootWidgetToFrontPtr;
        private static IntPtr IsWidgetInFrontPtr;
        private static IntPtr GetWidgetGeometryPtr;
        private static IntPtr SetWidgetGeometryPtr;
        private static IntPtr GetWidgetForLocationPtr;
        private static IntPtr GetWidgetExposedGeometryPtr;
        private static IntPtr SetWidgetDescriptorPtr;
        private static IntPtr GetWidgetDescriptorPtr;
        private static IntPtr GetWidgetUnderlyingWindowPtr;
        private static IntPtr SetWidgetPropertyPtr;
        private static IntPtr GetWidgetPropertyPtr;
        private static IntPtr SetKeyboardFocusPtr;
        private static IntPtr LoseKeyboardFocusPtr;
        private static IntPtr GetWidgetWithFocusPtr;
        private static IntPtr AddWidgetCallbackPtr;
        private static IntPtr GetWidgetClassFuncPtr;

        static Widgets()
        {
            const string libraryName = "Widgets";
            CreateWidgetPtr = FunctionResolver.Resolve(libraryName, "XPCreateWidget");
            CreateCustomWidgetPtr = FunctionResolver.Resolve(libraryName, "XPCreateCustomWidget");
            DestroyWidgetPtr = FunctionResolver.Resolve(libraryName, "XPDestroyWidget");
            SendMessageToWidgetPtr = FunctionResolver.Resolve(libraryName, "XPSendMessageToWidget");
            PlaceWidgetWithinPtr = FunctionResolver.Resolve(libraryName, "XPPlaceWidgetWithin");
            CountChildWidgetsPtr = FunctionResolver.Resolve(libraryName, "XPCountChildWidgets");
            GetNthChildWidgetPtr = FunctionResolver.Resolve(libraryName, "XPGetNthChildWidget");
            GetParentWidgetPtr = FunctionResolver.Resolve(libraryName, "XPGetParentWidget");
            ShowWidgetPtr = FunctionResolver.Resolve(libraryName, "XPShowWidget");
            HideWidgetPtr = FunctionResolver.Resolve(libraryName, "XPHideWidget");
            IsWidgetVisiblePtr = FunctionResolver.Resolve(libraryName, "XPIsWidgetVisible");
            FindRootWidgetPtr = FunctionResolver.Resolve(libraryName, "XPFindRootWidget");
            BringRootWidgetToFrontPtr = FunctionResolver.Resolve(libraryName, "XPBringRootWidgetToFront");
            IsWidgetInFrontPtr = FunctionResolver.Resolve(libraryName, "XPIsWidgetInFront");
            GetWidgetGeometryPtr = FunctionResolver.Resolve(libraryName, "XPGetWidgetGeometry");
            SetWidgetGeometryPtr = FunctionResolver.Resolve(libraryName, "XPSetWidgetGeometry");
            GetWidgetForLocationPtr = FunctionResolver.Resolve(libraryName, "XPGetWidgetForLocation");
            GetWidgetExposedGeometryPtr = FunctionResolver.Resolve(libraryName, "XPGetWidgetExposedGeometry");
            SetWidgetDescriptorPtr = FunctionResolver.Resolve(libraryName, "XPSetWidgetDescriptor");
            GetWidgetDescriptorPtr = FunctionResolver.Resolve(libraryName, "XPGetWidgetDescriptor");
            GetWidgetUnderlyingWindowPtr = FunctionResolver.Resolve(libraryName, "XPGetWidgetUnderlyingWindow");
            SetWidgetPropertyPtr = FunctionResolver.Resolve(libraryName, "XPSetWidgetProperty");
            GetWidgetPropertyPtr = FunctionResolver.Resolve(libraryName, "XPGetWidgetProperty");
            SetKeyboardFocusPtr = FunctionResolver.Resolve(libraryName, "XPSetKeyboardFocus");
            LoseKeyboardFocusPtr = FunctionResolver.Resolve(libraryName, "XPLoseKeyboardFocus");
            GetWidgetWithFocusPtr = FunctionResolver.Resolve(libraryName, "XPGetWidgetWithFocus");
            AddWidgetCallbackPtr = FunctionResolver.Resolve(libraryName, "XPAddWidgetCallback");
            GetWidgetClassFuncPtr = FunctionResolver.Resolve(libraryName, "XPGetWidgetClassFunc");
        }

        
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
        /// </para>
        /// <para>
        /// - inVisible is 1 if the widget should be drawn, 0 to start the widget as
        /// hidden.
        /// </para>
        /// <para>
        /// - inDescriptor is a null terminated string that will become the widget's
        /// descriptor.
        /// </para>
        /// <para>
        /// - inIsRoot is 1 if this is going to be a root widget, 0 if it will not be.
        /// </para>
        /// <para>
        /// - inContainer is the ID of this widget's container. It must be 0 for a root
        /// widget. for a non-root widget, pass the widget ID of the widget to place
        /// this widget within. If this widget is not going to start inside another
        /// widget, pass 0; this new widget will then just be floating off in space
        /// (and will not be drawn until it is placed in a widget.
        /// </para>
        /// <para>
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe WidgetID CreateWidget(int inLeft, int inTop, int inRight, int inBottom, int inVisible, byte* inDescriptor, int inIsRoot, WidgetID inContainer, WidgetClass inClass)
        {
            IL.DeclareLocals(false);
            WidgetID result;
            IL.Push(inLeft);
            IL.Push(inTop);
            IL.Push(inRight);
            IL.Push(inBottom);
            IL.Push(inVisible);
            IL.Push(inDescriptor);
            IL.Push(inIsRoot);
            IL.Push(inContainer);
            IL.Push(inClass);
            IL.Push(CreateWidgetPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(WidgetID), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(byte*), typeof(int), typeof(WidgetID), typeof(WidgetClass)));
            IL.Pop(out result);
            return result;
        }

        
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
        /// </para>
        /// <para>
        /// - inVisible is 1 if the widget should be drawn, 0 to start the widget as
        /// hidden.
        /// </para>
        /// <para>
        /// - inDescriptor is a null terminated string that will become the widget's
        /// descriptor.
        /// </para>
        /// <para>
        /// - inIsRoot is 1 if this is going to be a root widget, 0 if it will not be.
        /// </para>
        /// <para>
        /// - inContainer is the ID of this widget's container. It must be 0 for a root
        /// widget. for a non-root widget, pass the widget ID of the widget to place
        /// this widget within. If this widget is not going to start inside another
        /// widget, pass 0; this new widget will then just be floating off in space
        /// (and will not be drawn until it is placed in a widget.
        /// </para>
        /// <para>
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe WidgetID CreateWidget(int inLeft, int inTop, int inRight, int inBottom, int inVisible, in ReadOnlySpan<char> inDescriptor, int inIsRoot, WidgetID inContainer, WidgetClass inClass)
        {
            IL.DeclareLocals(false);
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe WidgetID CreateCustomWidget(int inLeft, int inTop, int inRight, int inBottom, int inVisible, byte* inDescriptor, int inIsRoot, WidgetID inContainer, WidgetFuncCallback inCallback)
        {
            IL.DeclareLocals(false);
            WidgetID result;
            IntPtr inCallbackPtr = Marshal.GetFunctionPointerForDelegate(inCallback);
            IL.Push(inLeft);
            IL.Push(inTop);
            IL.Push(inRight);
            IL.Push(inBottom);
            IL.Push(inVisible);
            IL.Push(inDescriptor);
            IL.Push(inIsRoot);
            IL.Push(inContainer);
            IL.Push(inCallbackPtr);
            IL.Push(CreateCustomWidgetPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(WidgetID), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(byte*), typeof(int), typeof(WidgetID), typeof(IntPtr)));
            IL.Pop(out result);
            GC.KeepAlive(inCallback);
            return result;
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe WidgetID CreateCustomWidget(int inLeft, int inTop, int inRight, int inBottom, int inVisible, in ReadOnlySpan<char> inDescriptor, int inIsRoot, WidgetID inContainer, WidgetFuncCallback inCallback)
        {
            IL.DeclareLocals(false);
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void DestroyWidget(WidgetID inWidget, int inDestroyChildren)
        {
            IL.DeclareLocals(false);
            IL.Push(inWidget);
            IL.Push(inDestroyChildren);
            IL.Push(DestroyWidgetPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WidgetID), typeof(int)));
        }

        
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int SendMessageToWidget(WidgetID inWidget, WidgetMessage inMessage, DispatchMode inMode, IntPtr inParam1, IntPtr inParam2)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inWidget);
            IL.Push(inMessage);
            IL.Push(inMode);
            IL.Push(inParam1);
            IL.Push(inParam2);
            IL.Push(SendMessageToWidgetPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(WidgetID), typeof(WidgetMessage), typeof(DispatchMode), typeof(IntPtr), typeof(IntPtr)));
            IL.Pop(out result);
            return result;
        }

        
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void PlaceWidgetWithin(WidgetID inSubWidget, WidgetID inContainer)
        {
            IL.DeclareLocals(false);
            IL.Push(inSubWidget);
            IL.Push(inContainer);
            IL.Push(PlaceWidgetWithinPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WidgetID), typeof(WidgetID)));
        }

        
        /// <summary>
        /// <para>
        /// This routine returns the number of widgets another widget contains.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int CountChildWidgets(WidgetID inWidget)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inWidget);
            IL.Push(CountChildWidgetsPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(WidgetID)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine returns the widget ID of a child widget by index. Indexes are
        /// 0 based, from 0 to one minus the number of widgets in the parent,
        /// inclusive. If the index is invalid, 0 is returned.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static WidgetID GetNthChildWidget(WidgetID inWidget, int inIndex)
        {
            IL.DeclareLocals(false);
            WidgetID result;
            IL.Push(inWidget);
            IL.Push(inIndex);
            IL.Push(GetNthChildWidgetPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(WidgetID), typeof(WidgetID), typeof(int)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine returns the parent of a widget, or 0 if the widget has no
        /// parent. Root widgets never have parents and therefore always return 0.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static WidgetID GetParentWidget(WidgetID inWidget)
        {
            IL.DeclareLocals(false);
            WidgetID result;
            IL.Push(inWidget);
            IL.Push(GetParentWidgetPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(WidgetID), typeof(WidgetID)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine makes a widget visible if it is not already. Note that if a
        /// widget is not in a rooted widget hierarchy or one of its parents is not
        /// visible, it will still not be visible to the user.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void ShowWidget(WidgetID inWidget)
        {
            IL.DeclareLocals(false);
            IL.Push(inWidget);
            IL.Push(ShowWidgetPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WidgetID)));
        }

        
        /// <summary>
        /// <para>
        /// Makes a widget invisible. See XPShowWidget for considerations of when a
        /// widget might not be visible despite its own visibility state.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void HideWidget(WidgetID inWidget)
        {
            IL.DeclareLocals(false);
            IL.Push(inWidget);
            IL.Push(HideWidgetPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WidgetID)));
        }

        
        /// <summary>
        /// <para>
        /// This returns 1 if a widget is visible, 0 if it is not. Note that this
        /// routine takes into consideration whether a parent is invisible. Use this
        /// routine to tell if the user can see the widget.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int IsWidgetVisible(WidgetID inWidget)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inWidget);
            IL.Push(IsWidgetVisiblePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(WidgetID)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// XPFindRootWidget returns the Widget ID of the root widget that contains the
        /// passed in widget or NULL if the passed in widget is not in a rooted
        /// hierarchy.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static WidgetID FindRootWidget(WidgetID inWidget)
        {
            IL.DeclareLocals(false);
            WidgetID result;
            IL.Push(inWidget);
            IL.Push(FindRootWidgetPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(WidgetID), typeof(WidgetID)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine makes the specified widget be in the front most widget
        /// hierarchy. If this widget is a root widget, its widget hierarchy comes to
        /// front, otherwise the widget's root is brought to the front. If this widget
        /// is not in an active widget hiearchy (e.g. there is no root widget at the
        /// top of the tree), this routine does nothing.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void BringRootWidgetToFront(WidgetID inWidget)
        {
            IL.DeclareLocals(false);
            IL.Push(inWidget);
            IL.Push(BringRootWidgetToFrontPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WidgetID)));
        }

        
        /// <summary>
        /// <para>
        /// This routine returns true if this widget's hierarchy is the front most
        /// hierarchy. It returns false if the widget's hierarchy is not in front, or
        /// if the widget is not in a rooted hierarchy.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int IsWidgetInFront(WidgetID inWidget)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inWidget);
            IL.Push(IsWidgetInFrontPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(WidgetID)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine returns the bounding box of a widget in global coordinates.
        /// Pass NULL for any parameter you are not interested in.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetWidgetGeometry(WidgetID inWidget, int* outLeft, int* outTop, int* outRight, int* outBottom)
        {
            IL.DeclareLocals(false);
            IL.Push(inWidget);
            IL.Push(outLeft);
            IL.Push(outTop);
            IL.Push(outRight);
            IL.Push(outBottom);
            IL.Push(GetWidgetGeometryPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WidgetID), typeof(int*), typeof(int*), typeof(int*), typeof(int*)));
        }

        
        /// <summary>
        /// <para>
        /// This function changes the bounding box of a widget.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetWidgetGeometry(WidgetID inWidget, int inLeft, int inTop, int inRight, int inBottom)
        {
            IL.DeclareLocals(false);
            IL.Push(inWidget);
            IL.Push(inLeft);
            IL.Push(inTop);
            IL.Push(inRight);
            IL.Push(inBottom);
            IL.Push(SetWidgetGeometryPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WidgetID), typeof(int), typeof(int), typeof(int), typeof(int)));
        }

        
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static WidgetID GetWidgetForLocation(WidgetID inContainer, int inXOffset, int inYOffset, int inRecursive, int inVisibleOnly)
        {
            IL.DeclareLocals(false);
            WidgetID result;
            IL.Push(inContainer);
            IL.Push(inXOffset);
            IL.Push(inYOffset);
            IL.Push(inRecursive);
            IL.Push(inVisibleOnly);
            IL.Push(GetWidgetForLocationPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(WidgetID), typeof(WidgetID), typeof(int), typeof(int), typeof(int), typeof(int)));
            IL.Pop(out result);
            return result;
        }

        
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetWidgetExposedGeometry(WidgetID inWidgetID, int* outLeft, int* outTop, int* outRight, int* outBottom)
        {
            IL.DeclareLocals(false);
            IL.Push(inWidgetID);
            IL.Push(outLeft);
            IL.Push(outTop);
            IL.Push(outRight);
            IL.Push(outBottom);
            IL.Push(GetWidgetExposedGeometryPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WidgetID), typeof(int*), typeof(int*), typeof(int*), typeof(int*)));
        }

        
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetWidgetDescriptor(WidgetID inWidget, byte* inDescriptor)
        {
            IL.DeclareLocals(false);
            IL.Push(inWidget);
            IL.Push(inDescriptor);
            IL.Push(SetWidgetDescriptorPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WidgetID), typeof(byte*)));
        }

        
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetWidgetDescriptor(WidgetID inWidget, in ReadOnlySpan<char> inDescriptor)
        {
            IL.DeclareLocals(false);
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int GetWidgetDescriptor(WidgetID inWidget, byte* outDescriptor, int inMaxDescLength)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inWidget);
            IL.Push(outDescriptor);
            IL.Push(inMaxDescLength);
            IL.Push(GetWidgetDescriptorPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(WidgetID), typeof(byte*), typeof(int)));
            IL.Pop(out result);
            return result;
        }

        
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static WindowID GetWidgetUnderlyingWindow(WidgetID inWidget)
        {
            IL.DeclareLocals(false);
            WindowID result;
            IL.Push(inWidget);
            IL.Push(GetWidgetUnderlyingWindowPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(WindowID), typeof(WidgetID)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This function sets a widget's property. Properties are arbitrary values
        /// associated by a widget by ID.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetWidgetProperty(WidgetID inWidget, WidgetPropertyID inProperty, IntPtr inValue)
        {
            IL.DeclareLocals(false);
            IL.Push(inWidget);
            IL.Push(inProperty);
            IL.Push(inValue);
            IL.Push(SetWidgetPropertyPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WidgetID), typeof(WidgetPropertyID), typeof(IntPtr)));
        }

        
        /// <summary>
        /// <para>
        /// This routine returns the value of a widget's property, or 0 if the property
        /// is not defined. If you need to know whether the property is defined, pass a
        /// pointer to an int for inExists; the existence of that property will be
        /// returned in the int. Pass NULL for inExists if you do not need this
        /// information.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe IntPtr GetWidgetProperty(WidgetID inWidget, WidgetPropertyID inProperty, int* inExists)
        {
            IL.DeclareLocals(false);
            IntPtr result;
            IL.Push(inWidget);
            IL.Push(inProperty);
            IL.Push(inExists);
            IL.Push(GetWidgetPropertyPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(IntPtr), typeof(WidgetID), typeof(WidgetPropertyID), typeof(int*)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// XPSetKeyboardFocus controls which widget will receive keystrokes. Pass the
        /// Widget ID of the widget to get the keys. Note that if the widget does not
        /// care about keystrokes, they will go to the parent widget, and if no widget
        /// cares about them, they go to X-Plane.
        /// </para>
        /// <para>
        /// If you set the keyboard focus to Widget ID 0, X-Plane gets keyboard focus.
        /// </para>
        /// <para>
        /// This routine returns the widget ID that ended up with keyboard focus, or 0
        /// for X-Plane.
        /// </para>
        /// <para>
        /// Keyboard focus is not changed if the new widget will not accept it. For
        /// setting to X-Plane, keyboard focus is always accepted.
        /// </para>
        /// <para>
        /// *
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static WidgetID SetKeyboardFocus(WidgetID inWidget)
        {
            IL.DeclareLocals(false);
            WidgetID result;
            IL.Push(inWidget);
            IL.Push(SetKeyboardFocusPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(WidgetID), typeof(WidgetID)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This causes the specified widget to lose focus; focus is passed to its
        /// parent, or the next parent that will accept it. This routine does nothing
        /// if this widget does not have focus.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void LoseKeyboardFocus(WidgetID inWidget)
        {
            IL.DeclareLocals(false);
            IL.Push(inWidget);
            IL.Push(LoseKeyboardFocusPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WidgetID)));
        }

        
        /// <summary>
        /// <para>
        /// This routine returns the widget that has keyboard focus, or 0 if X-Plane
        /// has keyboard focus or some other plugin window that does not have widgets
        /// has focus.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static WidgetID GetWidgetWithFocus()
        {
            IL.DeclareLocals(false);
            WidgetID result;
            IL.Push(GetWidgetWithFocusPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(WidgetID)));
            IL.Pop(out result);
            return result;
        }

        
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void AddWidgetCallback(WidgetID inWidget, WidgetFuncCallback inNewCallback)
        {
            IL.DeclareLocals(false);
            IntPtr inNewCallbackPtr = Marshal.GetFunctionPointerForDelegate(inNewCallback);
            IL.Push(inWidget);
            IL.Push(inNewCallbackPtr);
            IL.Push(AddWidgetCallbackPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WidgetID), typeof(IntPtr)));
            GC.KeepAlive(inNewCallback);
        }

        
        /// <summary>
        /// <para>
        /// Given a widget class, this function returns the callbacks that power that
        /// widget class.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static WidgetFuncCallback GetWidgetClassFunc(WidgetClass inWidgetClass)
        {
            IL.DeclareLocals(false);
            WidgetFuncCallback result;
            IL.Push(inWidgetClass);
            IL.Push(GetWidgetClassFuncPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(WidgetFuncCallback), typeof(WidgetClass)));
            IL.Pop(out result);
            return result;
        }
    }
}