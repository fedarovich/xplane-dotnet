using InlineIL;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Internal
{
    public static partial class Menus
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
        static Menus()
        {
            const string libraryName = "XPLM";
            FindPluginsMenuPtr = FunctionResolver.Resolve(libraryName, "XPLMFindPluginsMenu");
            FindAircraftMenuPtr = FunctionResolver.Resolve(libraryName, "XPLMFindAircraftMenu");
            CreateMenuPtr = FunctionResolver.Resolve(libraryName, "XPLMCreateMenu");
            DestroyMenuPtr = FunctionResolver.Resolve(libraryName, "XPLMDestroyMenu");
            ClearAllMenuItemsPtr = FunctionResolver.Resolve(libraryName, "XPLMClearAllMenuItems");
            AppendMenuItemPtr = FunctionResolver.Resolve(libraryName, "XPLMAppendMenuItem");
            AppendMenuItemWithCommandPtr = FunctionResolver.Resolve(libraryName, "XPLMAppendMenuItemWithCommand");
            AppendMenuSeparatorPtr = FunctionResolver.Resolve(libraryName, "XPLMAppendMenuSeparator");
            SetMenuItemNamePtr = FunctionResolver.Resolve(libraryName, "XPLMSetMenuItemName");
            CheckMenuItemPtr = FunctionResolver.Resolve(libraryName, "XPLMCheckMenuItem");
            CheckMenuItemStatePtr = FunctionResolver.Resolve(libraryName, "XPLMCheckMenuItemState");
            EnableMenuItemPtr = FunctionResolver.Resolve(libraryName, "XPLMEnableMenuItem");
            RemoveMenuItemPtr = FunctionResolver.Resolve(libraryName, "XPLMRemoveMenuItem");
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static MenuID FindPluginsMenu()
        {
            IL.DeclareLocals(false);
            MenuID result;
            IL.Push(FindPluginsMenuPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(MenuID)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static MenuID FindAircraftMenu()
        {
            IL.DeclareLocals(false);
            MenuID result;
            IL.Push(FindAircraftMenuPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(MenuID)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe MenuID CreateMenu(byte *inName, MenuID inParentMenu, int inParentItem, MenuHandlerCallback inHandler, void *inMenuRef)
        {
            IL.DeclareLocals(false);
            MenuID result;
            IntPtr inHandlerPtr = Marshal.GetFunctionPointerForDelegate(inHandler);
            IL.Push(inName);
            IL.Push(inParentMenu);
            IL.Push(inParentItem);
            IL.Push(inHandlerPtr);
            IL.Push(inMenuRef);
            IL.Push(CreateMenuPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(MenuID), typeof(byte *), typeof(MenuID), typeof(int), typeof(IntPtr), typeof(void *)));
            IL.Pop(out result);
            GC.KeepAlive(inHandler);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void DestroyMenu(MenuID inMenuID)
        {
            IL.DeclareLocals(false);
            IL.Push(inMenuID);
            IL.Push(DestroyMenuPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(MenuID)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void ClearAllMenuItems(MenuID inMenuID)
        {
            IL.DeclareLocals(false);
            IL.Push(inMenuID);
            IL.Push(ClearAllMenuItemsPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(MenuID)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int AppendMenuItem(MenuID inMenu, byte *inItemName, void *inItemRef, int inDeprecatedAndIgnored)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inMenu);
            IL.Push(inItemName);
            IL.Push(inItemRef);
            IL.Push(inDeprecatedAndIgnored);
            IL.Push(AppendMenuItemPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(MenuID), typeof(byte *), typeof(void *), typeof(int)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int AppendMenuItemWithCommand(MenuID inMenu, byte *inItemName, CommandRef inCommandToExecute)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inMenu);
            IL.Push(inItemName);
            IL.Push(inCommandToExecute);
            IL.Push(AppendMenuItemWithCommandPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(MenuID), typeof(byte *), typeof(CommandRef)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void AppendMenuSeparator(MenuID inMenu)
        {
            IL.DeclareLocals(false);
            IL.Push(inMenu);
            IL.Push(AppendMenuSeparatorPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(MenuID)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetMenuItemName(MenuID inMenu, int inIndex, byte *inItemName, int inForceEnglish)
        {
            IL.DeclareLocals(false);
            IL.Push(inMenu);
            IL.Push(inIndex);
            IL.Push(inItemName);
            IL.Push(inForceEnglish);
            IL.Push(SetMenuItemNamePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(MenuID), typeof(int), typeof(byte *), typeof(int)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void CheckMenuItem(MenuID inMenu, int index, MenuCheck inCheck)
        {
            IL.DeclareLocals(false);
            IL.Push(inMenu);
            IL.Push(index);
            IL.Push(inCheck);
            IL.Push(CheckMenuItemPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(MenuID), typeof(int), typeof(MenuCheck)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void CheckMenuItemState(MenuID inMenu, int index, MenuCheck*outCheck)
        {
            IL.DeclareLocals(false);
            IL.Push(inMenu);
            IL.Push(index);
            IL.Push(outCheck);
            IL.Push(CheckMenuItemStatePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(MenuID), typeof(int), typeof(MenuCheck*)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void EnableMenuItem(MenuID inMenu, int index, int enabled)
        {
            IL.DeclareLocals(false);
            IL.Push(inMenu);
            IL.Push(index);
            IL.Push(enabled);
            IL.Push(EnableMenuItemPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(MenuID), typeof(int), typeof(int)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void RemoveMenuItem(MenuID inMenu, int inIndex)
        {
            IL.DeclareLocals(false);
            IL.Push(inMenu);
            IL.Push(inIndex);
            IL.Push(RemoveMenuItemPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(MenuID), typeof(int)));
        }
    }
}