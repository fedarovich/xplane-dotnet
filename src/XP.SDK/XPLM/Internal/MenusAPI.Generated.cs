using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using InlineIL;

namespace XP.SDK.XPLM.Internal
{
    public static partial class MenusAPI
    {
        private static IntPtr FindPluginsMenuPtr;
        private static IntPtr FindAircraftMenuPtr;
        private static IntPtr CreateMenuPtr;
        private static IntPtr DestroyMenuPtr;
        private static IntPtr ClearAllMenuItemsPtr;
        private static IntPtr AppendMenuItemPtr;
        private static IntPtr AppendMenuItemWithCommandPtr;
        private static IntPtr AppendMenuSeparatorPtr;
        private static IntPtr SetMenuItemNamePtr;
        private static IntPtr CheckMenuItemPtr;
        private static IntPtr CheckMenuItemStatePtr;
        private static IntPtr EnableMenuItemPtr;
        private static IntPtr RemoveMenuItemPtr;

        static MenusAPI()
        {
            FindPluginsMenuPtr = Lib.GetExport("XPLMFindPluginsMenu");
            FindAircraftMenuPtr = Lib.GetExport("XPLMFindAircraftMenu");
            CreateMenuPtr = Lib.GetExport("XPLMCreateMenu");
            DestroyMenuPtr = Lib.GetExport("XPLMDestroyMenu");
            ClearAllMenuItemsPtr = Lib.GetExport("XPLMClearAllMenuItems");
            AppendMenuItemPtr = Lib.GetExport("XPLMAppendMenuItem");
            AppendMenuItemWithCommandPtr = Lib.GetExport("XPLMAppendMenuItemWithCommand");
            AppendMenuSeparatorPtr = Lib.GetExport("XPLMAppendMenuSeparator");
            SetMenuItemNamePtr = Lib.GetExport("XPLMSetMenuItemName");
            CheckMenuItemPtr = Lib.GetExport("XPLMCheckMenuItem");
            CheckMenuItemStatePtr = Lib.GetExport("XPLMCheckMenuItemState");
            EnableMenuItemPtr = Lib.GetExport("XPLMEnableMenuItem");
            RemoveMenuItemPtr = Lib.GetExport("XPLMRemoveMenuItem");
        }

        
        /// <summary>
        /// <para>
        /// This function returns the ID of the plug-ins menu, which is created for you
        /// at startup.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static MenuID FindPluginsMenu()
        {
            IL.DeclareLocals(false);
            Guard.NotNull(FindPluginsMenuPtr);
            MenuID result;
            IL.Push(FindPluginsMenuPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(MenuID)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This function returns the ID of the menu for the currently-loaded aircraft,
        /// used for showing aircraft-specific commands.
        /// </para>
        /// <para>
        /// The aircraft menu is created by X-Plane at startup, but it remains hidden
        /// until it is populated via XPLMAppendMenuItem() or
        /// XPLMAppendMenuItemWithCommand().
        /// </para>
        /// <para>
        /// Only plugins loaded with the user's current aircraft are allowed to access
        /// the aircraft menu. For all other plugins, this will return NULL, and any
        /// attempts to add menu items to it will fail.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static MenuID FindAircraftMenu()
        {
            IL.DeclareLocals(false);
            Guard.NotNull(FindAircraftMenuPtr);
            MenuID result;
            IL.Push(FindAircraftMenuPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(MenuID)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static unsafe MenuID CreateMenuPrivate(byte* inName, MenuID inParentMenu, int inParentItem, IntPtr inHandler, void* inMenuRef)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(CreateMenuPtr);
            MenuID result;
            IL.Push(inName);
            IL.Push(inParentMenu);
            IL.Push(inParentItem);
            IL.Push(inHandler);
            IL.Push(inMenuRef);
            IL.Push(CreateMenuPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(MenuID), typeof(byte*), typeof(MenuID), typeof(int), typeof(IntPtr), typeof(void*)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This function creates a new menu and returns its ID.  It returns NULL if
        /// the menu cannot be created.  Pass in a parent menu ID and an item index to
        /// create a submenu, or NULL for the parent menu to put the menu in the menu
        /// bar.  The menu's name is only used if the menu is in the menubar.  You also
        /// pass a handler function and a menu reference value. Pass NULL for the
        /// handler if you do not need callbacks from the menu (for example, if it only
        /// contains submenus).
        /// </para>
        /// <para>
        /// Important: you must pass a valid, non-empty menu title even if the menu is
        /// a submenu where the title is not visible.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe MenuID CreateMenu(byte* inName, MenuID inParentMenu, int inParentItem, MenuHandlerCallback inHandler, void* inMenuRef)
        {
            IL.DeclareLocals(false);
            IntPtr inHandlerPtr = inHandler != null ? Marshal.GetFunctionPointerForDelegate(inHandler) : default;
            MenuID result = CreateMenuPrivate(inName, inParentMenu, inParentItem, inHandlerPtr, inMenuRef);
            GC.KeepAlive(inHandler);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This function creates a new menu and returns its ID.  It returns NULL if
        /// the menu cannot be created.  Pass in a parent menu ID and an item index to
        /// create a submenu, or NULL for the parent menu to put the menu in the menu
        /// bar.  The menu's name is only used if the menu is in the menubar.  You also
        /// pass a handler function and a menu reference value. Pass NULL for the
        /// handler if you do not need callbacks from the menu (for example, if it only
        /// contains submenus).
        /// </para>
        /// <para>
        /// Important: you must pass a valid, non-empty menu title even if the menu is
        /// a submenu where the title is not visible.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe MenuID CreateMenu(in ReadOnlySpan<char> inName, MenuID inParentMenu, int inParentItem, MenuHandlerCallback inHandler, void* inMenuRef)
        {
            IL.DeclareLocals(false);
            Span<byte> inNameUtf8 = stackalloc byte[(inName.Length << 1) | 1];
            var inNamePtr = Utils.ToUtf8Unsafe(inName, inNameUtf8);
            return CreateMenu(inNamePtr, inParentMenu, inParentItem, inHandler, inMenuRef);
        }

        
        /// <summary>
        /// <para>
        /// This function destroys a menu that you have created.  Use this to remove a
        /// submenu if necessary.  (Normally this function will not be necessary.)
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void DestroyMenu(MenuID inMenuID)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(DestroyMenuPtr);
            IL.Push(inMenuID);
            IL.Push(DestroyMenuPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(MenuID)));
        }

        
        /// <summary>
        /// <para>
        /// This function removes all menu items from a menu, allowing you to rebuild
        /// it.  Use this function if you need to change the number of items on a menu.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void ClearAllMenuItems(MenuID inMenuID)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(ClearAllMenuItemsPtr);
            IL.Push(inMenuID);
            IL.Push(ClearAllMenuItemsPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(MenuID)));
        }

        
        /// <summary>
        /// <para>
        /// This routine appends a new menu item to the bottom of a menu and returns
        /// its index. Pass in the menu to add the item to, the items name, and a void
        /// * ref for this item.
        /// </para>
        /// <para>
        /// Returns a negative index if the append failed (due to an invalid parent
        /// menu argument).
        /// </para>
        /// <para>
        /// Note that all menu indices returned are relative to your plugin's menus
        /// only; if your plugin creates two sub-menus in the Plugins menu at different
        /// times, it doesn't matter how many other plugins also create sub-menus of
        /// Plugins in the intervening time: your sub-menus will be given menu indices
        /// 0 and 1. (The SDK does some work in the back-end to filter out menus that
        /// are irrelevant to your plugin in order to deliver this consistency for each
        /// plugin.)
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int AppendMenuItem(MenuID inMenu, byte* inItemName, void* inItemRef, int inDeprecatedAndIgnored)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(AppendMenuItemPtr);
            int result;
            IL.Push(inMenu);
            IL.Push(inItemName);
            IL.Push(inItemRef);
            IL.Push(inDeprecatedAndIgnored);
            IL.Push(AppendMenuItemPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(MenuID), typeof(byte*), typeof(void*), typeof(int)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine appends a new menu item to the bottom of a menu and returns
        /// its index. Pass in the menu to add the item to, the items name, and a void
        /// * ref for this item.
        /// </para>
        /// <para>
        /// Returns a negative index if the append failed (due to an invalid parent
        /// menu argument).
        /// </para>
        /// <para>
        /// Note that all menu indices returned are relative to your plugin's menus
        /// only; if your plugin creates two sub-menus in the Plugins menu at different
        /// times, it doesn't matter how many other plugins also create sub-menus of
        /// Plugins in the intervening time: your sub-menus will be given menu indices
        /// 0 and 1. (The SDK does some work in the back-end to filter out menus that
        /// are irrelevant to your plugin in order to deliver this consistency for each
        /// plugin.)
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int AppendMenuItem(MenuID inMenu, in ReadOnlySpan<char> inItemName, void* inItemRef, int inDeprecatedAndIgnored)
        {
            IL.DeclareLocals(false);
            Span<byte> inItemNameUtf8 = stackalloc byte[(inItemName.Length << 1) | 1];
            var inItemNamePtr = Utils.ToUtf8Unsafe(inItemName, inItemNameUtf8);
            return AppendMenuItem(inMenu, inItemNamePtr, inItemRef, inDeprecatedAndIgnored);
        }

        
        /// <summary>
        /// <para>
        /// Like XPLMAppendMenuItem(), but instead of the new menu item triggering the
        /// XPLMMenuHandler_f of the containiner menu, it will simply execute the
        /// command you pass in. Using a command for your menu item allows the user to
        /// bind a keyboard shortcut to the command and see that shortcut represented
        /// in the menu.
        /// </para>
        /// <para>
        /// Returns a negative index if the append failed (due to an invalid parent
        /// menu argument).
        /// </para>
        /// <para>
        /// Like XPLMAppendMenuItem(), all menu indices are relative to your plugin's
        /// menus only.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int AppendMenuItemWithCommand(MenuID inMenu, byte* inItemName, CommandRef inCommandToExecute)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(AppendMenuItemWithCommandPtr);
            int result;
            IL.Push(inMenu);
            IL.Push(inItemName);
            IL.Push(inCommandToExecute);
            IL.Push(AppendMenuItemWithCommandPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(MenuID), typeof(byte*), typeof(CommandRef)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// Like XPLMAppendMenuItem(), but instead of the new menu item triggering the
        /// XPLMMenuHandler_f of the containiner menu, it will simply execute the
        /// command you pass in. Using a command for your menu item allows the user to
        /// bind a keyboard shortcut to the command and see that shortcut represented
        /// in the menu.
        /// </para>
        /// <para>
        /// Returns a negative index if the append failed (due to an invalid parent
        /// menu argument).
        /// </para>
        /// <para>
        /// Like XPLMAppendMenuItem(), all menu indices are relative to your plugin's
        /// menus only.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int AppendMenuItemWithCommand(MenuID inMenu, in ReadOnlySpan<char> inItemName, CommandRef inCommandToExecute)
        {
            IL.DeclareLocals(false);
            Span<byte> inItemNameUtf8 = stackalloc byte[(inItemName.Length << 1) | 1];
            var inItemNamePtr = Utils.ToUtf8Unsafe(inItemName, inItemNameUtf8);
            return AppendMenuItemWithCommand(inMenu, inItemNamePtr, inCommandToExecute);
        }

        
        /// <summary>
        /// <para>
        /// This routine adds a separator to the end of a menu.
        /// </para>
        /// <para>
        /// Returns a negative index if the append failed (due to an invalid parent
        /// menu argument).
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void AppendMenuSeparator(MenuID inMenu)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(AppendMenuSeparatorPtr);
            IL.Push(inMenu);
            IL.Push(AppendMenuSeparatorPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(MenuID)));
        }

        
        /// <summary>
        /// <para>
        /// This routine changes the name of an existing menu item.  Pass in the menu
        /// ID and the index of the menu item.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetMenuItemName(MenuID inMenu, int inIndex, byte* inItemName, int inDeprecatedAndIgnored)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(SetMenuItemNamePtr);
            IL.Push(inMenu);
            IL.Push(inIndex);
            IL.Push(inItemName);
            IL.Push(inDeprecatedAndIgnored);
            IL.Push(SetMenuItemNamePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(MenuID), typeof(int), typeof(byte*), typeof(int)));
        }

        
        /// <summary>
        /// <para>
        /// This routine changes the name of an existing menu item.  Pass in the menu
        /// ID and the index of the menu item.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetMenuItemName(MenuID inMenu, int inIndex, in ReadOnlySpan<char> inItemName, int inDeprecatedAndIgnored)
        {
            IL.DeclareLocals(false);
            Span<byte> inItemNameUtf8 = stackalloc byte[(inItemName.Length << 1) | 1];
            var inItemNamePtr = Utils.ToUtf8Unsafe(inItemName, inItemNameUtf8);
            SetMenuItemName(inMenu, inIndex, inItemNamePtr, inDeprecatedAndIgnored);
        }

        
        /// <summary>
        /// <para>
        /// Set whether a menu item is checked.  Pass in the menu ID and item index.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void CheckMenuItem(MenuID inMenu, int index, MenuCheck inCheck)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(CheckMenuItemPtr);
            IL.Push(inMenu);
            IL.Push(index);
            IL.Push(inCheck);
            IL.Push(CheckMenuItemPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(MenuID), typeof(int), typeof(MenuCheck)));
        }

        
        /// <summary>
        /// <para>
        /// This routine returns whether a menu item is checked or not. A menu item's
        /// check mark may be on or off, or a menu may not have an icon at all.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void CheckMenuItemState(MenuID inMenu, int index, MenuCheck* outCheck)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(CheckMenuItemStatePtr);
            IL.Push(inMenu);
            IL.Push(index);
            IL.Push(outCheck);
            IL.Push(CheckMenuItemStatePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(MenuID), typeof(int), typeof(MenuCheck*)));
        }

        
        /// <summary>
        /// <para>
        /// Sets whether this menu item is enabled.  Items start out enabled.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void EnableMenuItem(MenuID inMenu, int index, int enabled)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(EnableMenuItemPtr);
            IL.Push(inMenu);
            IL.Push(index);
            IL.Push(enabled);
            IL.Push(EnableMenuItemPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(MenuID), typeof(int), typeof(int)));
        }

        
        /// <summary>
        /// <para>
        /// Removes one item from a menu.  Note that all menu items below are moved up
        /// one; your plugin must track the change in index numbers.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void RemoveMenuItem(MenuID inMenu, int inIndex)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(RemoveMenuItemPtr);
            IL.Push(inMenu);
            IL.Push(inIndex);
            IL.Push(RemoveMenuItemPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(MenuID), typeof(int)));
        }
    }
}