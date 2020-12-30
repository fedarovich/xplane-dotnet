using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using InlineIL;

namespace XP.SDK.XPLM.Interop
{
    public static partial class MenusAPI
    {
        
        /// <summary>
        /// <para>
        /// This function returns the ID of the plug-ins menu, which is created for you
        /// at startup.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMFindPluginsMenu", ExactSpelling = true)]
        public static extern MenuID FindPluginsMenu();

        
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
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMFindAircraftMenu", ExactSpelling = true)]
        public static extern MenuID FindAircraftMenu();

        
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
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMCreateMenu", ExactSpelling = true)]
        public static extern unsafe MenuID CreateMenu(byte* inName, MenuID inParentMenu, int inParentItem, delegate* unmanaged[Cdecl]<void*, void*, void> inHandler, void* inMenuRef);

        
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
        [SkipLocalsInitAttribute]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe MenuID CreateMenu(in ReadOnlySpan<char> inName, MenuID inParentMenu, int inParentItem, delegate* unmanaged[Cdecl]<void*, void*, void> inHandler, void* inMenuRef)
        {
            Span<byte> inNameUtf8 = stackalloc byte[(inName.Length << 1) | 1];
            var inNamePtr = Utils.ToUtf8Unsafe(inName, inNameUtf8);
            return CreateMenu(inNamePtr, inParentMenu, inParentItem, inHandler, inMenuRef);
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
        public static unsafe MenuID CreateMenu(in XP.SDK.Utf8String inName, MenuID inParentMenu, int inParentItem, delegate* unmanaged[Cdecl]<void*, void*, void> inHandler, void* inMenuRef)
        {
            fixed (byte* inNamePtr = inName)
                return CreateMenu(inNamePtr, inParentMenu, inParentItem, inHandler, inMenuRef);
        }

        
        /// <summary>
        /// <para>
        /// This function destroys a menu that you have created.  Use this to remove a
        /// submenu if necessary.  (Normally this function will not be necessary.)
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMDestroyMenu", ExactSpelling = true)]
        public static extern void DestroyMenu(MenuID inMenuID);

        
        /// <summary>
        /// <para>
        /// This function removes all menu items from a menu, allowing you to rebuild
        /// it.  Use this function if you need to change the number of items on a menu.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMClearAllMenuItems", ExactSpelling = true)]
        public static extern void ClearAllMenuItems(MenuID inMenuID);

        
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
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMAppendMenuItem", ExactSpelling = true)]
        public static extern unsafe int AppendMenuItem(MenuID inMenu, byte* inItemName, void* inItemRef, int inDeprecatedAndIgnored);

        
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
        [SkipLocalsInitAttribute]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int AppendMenuItem(MenuID inMenu, in ReadOnlySpan<char> inItemName, void* inItemRef, int inDeprecatedAndIgnored)
        {
            Span<byte> inItemNameUtf8 = stackalloc byte[(inItemName.Length << 1) | 1];
            var inItemNamePtr = Utils.ToUtf8Unsafe(inItemName, inItemNameUtf8);
            return AppendMenuItem(inMenu, inItemNamePtr, inItemRef, inDeprecatedAndIgnored);
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
        public static unsafe int AppendMenuItem(MenuID inMenu, in XP.SDK.Utf8String inItemName, void* inItemRef, int inDeprecatedAndIgnored)
        {
            fixed (byte* inItemNamePtr = inItemName)
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
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMAppendMenuItemWithCommand", ExactSpelling = true)]
        public static extern unsafe int AppendMenuItemWithCommand(MenuID inMenu, byte* inItemName, CommandRef inCommandToExecute);

        
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
        [SkipLocalsInitAttribute]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int AppendMenuItemWithCommand(MenuID inMenu, in ReadOnlySpan<char> inItemName, CommandRef inCommandToExecute)
        {
            Span<byte> inItemNameUtf8 = stackalloc byte[(inItemName.Length << 1) | 1];
            var inItemNamePtr = Utils.ToUtf8Unsafe(inItemName, inItemNameUtf8);
            return AppendMenuItemWithCommand(inMenu, inItemNamePtr, inCommandToExecute);
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
        public static unsafe int AppendMenuItemWithCommand(MenuID inMenu, in XP.SDK.Utf8String inItemName, CommandRef inCommandToExecute)
        {
            fixed (byte* inItemNamePtr = inItemName)
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
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMAppendMenuSeparator", ExactSpelling = true)]
        public static extern void AppendMenuSeparator(MenuID inMenu);

        
        /// <summary>
        /// <para>
        /// This routine changes the name of an existing menu item.  Pass in the menu
        /// ID and the index of the menu item.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMSetMenuItemName", ExactSpelling = true)]
        public static extern unsafe void SetMenuItemName(MenuID inMenu, int inIndex, byte* inItemName, int inDeprecatedAndIgnored);

        
        /// <summary>
        /// <para>
        /// This routine changes the name of an existing menu item.  Pass in the menu
        /// ID and the index of the menu item.
        /// </para>
        /// </summary>
        [SkipLocalsInitAttribute]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetMenuItemName(MenuID inMenu, int inIndex, in ReadOnlySpan<char> inItemName, int inDeprecatedAndIgnored)
        {
            Span<byte> inItemNameUtf8 = stackalloc byte[(inItemName.Length << 1) | 1];
            var inItemNamePtr = Utils.ToUtf8Unsafe(inItemName, inItemNameUtf8);
            SetMenuItemName(inMenu, inIndex, inItemNamePtr, inDeprecatedAndIgnored);
        }

        
        /// <summary>
        /// <para>
        /// This routine changes the name of an existing menu item.  Pass in the menu
        /// ID and the index of the menu item.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetMenuItemName(MenuID inMenu, int inIndex, in XP.SDK.Utf8String inItemName, int inDeprecatedAndIgnored)
        {
            fixed (byte* inItemNamePtr = inItemName)
                SetMenuItemName(inMenu, inIndex, inItemNamePtr, inDeprecatedAndIgnored);
        }

        
        /// <summary>
        /// <para>
        /// Set whether a menu item is checked.  Pass in the menu ID and item index.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMCheckMenuItem", ExactSpelling = true)]
        public static extern void CheckMenuItem(MenuID inMenu, int index, MenuCheck inCheck);

        
        /// <summary>
        /// <para>
        /// This routine returns whether a menu item is checked or not. A menu item's
        /// check mark may be on or off, or a menu may not have an icon at all.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMCheckMenuItemState", ExactSpelling = true)]
        public static extern unsafe void CheckMenuItemState(MenuID inMenu, int index, MenuCheck* outCheck);

        
        /// <summary>
        /// <para>
        /// Sets whether this menu item is enabled.  Items start out enabled.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMEnableMenuItem", ExactSpelling = true)]
        public static extern void EnableMenuItem(MenuID inMenu, int index, int enabled);

        
        /// <summary>
        /// <para>
        /// Removes one item from a menu.  Note that all menu items below are moved up
        /// one; your plugin must track the change in index numbers.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMRemoveMenuItem", ExactSpelling = true)]
        public static extern void RemoveMenuItem(MenuID inMenu, int inIndex);
    }
}