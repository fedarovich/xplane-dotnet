using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using XP.SDK.XPLM.Interop;

namespace XP.SDK.XPLM
{
    public sealed class Menu : IDisposable
    {
        private static readonly MenuHandlerCallback _menuCallback;
        private static Menu _pluginsMenu;
        private static Menu _aircraftMenu;

        private readonly MenuItemList _items;
        private MenuID _id;
        private GCHandle _handle;
        private int _disposed;

        static unsafe Menu()
        {
            _menuCallback = MenuCallback;

            static void MenuCallback(void* inMenuRef, void* inItemRef)
            {
                var menu = Utils.TryGetObject<Menu>(inMenuRef);
                var uniqueId = (long) inItemRef;
                var item = menu._items.FindByUniqueId(uniqueId);
                item?.OnClick();
                menu.Click?.Invoke(menu, item);
            }
        }

        /// <summary>
        /// Initializes a new top-level menu.
        /// </summary>
        public unsafe Menu()
        {
            _handle = GCHandle.Alloc(this);
            _id = MenusAPI.CreateMenu(
                Guid.NewGuid().ToString("N"),
                default,
                default,
                &OnMenuCallback,
                GCHandle.ToIntPtr(_handle).ToPointer());
            _items = new MenuItemList(_id);
        }

        internal unsafe Menu(NormalMenuItem parentItem)
        {
            var index = MenuItemList.GetIndex(parentItem);
            if (index == null)
                throw new InvalidOperationException("The menu item is not attached to any menu.");

            _handle = GCHandle.Alloc(this);
            _id = MenusAPI.CreateMenu(
                Guid.NewGuid().ToString("N"),
                parentItem.ParentMenu.Id,
                index.Value,
                &OnMenuCallback,
                GCHandle.ToIntPtr(_handle).ToPointer());
            _items = new MenuItemList(_id);
        }

        private Menu(MenuID id)
        {
            _id = id;
            _items = new MenuItemList(id);
        }

        [UnmanagedCallersOnly]
        static unsafe void OnMenuCallback(void* inMenuRef, void* inItemRef)
        {
            var menu = Utils.TryGetObject<Menu>(inMenuRef);
            var uniqueId = (long)inItemRef;
            var item = menu._items.FindByUniqueId(uniqueId);
            item?.OnClick();
            menu.Click?.Invoke(menu, item);
        }

        /// <summary>
        /// Gets the menu ID.
        /// </summary>
        public MenuID Id
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _id;
        }

        /// <summary>
        /// Gets the list of menu items.
        /// </summary>
        public IReadOnlyList<MenuItem> Items => _items;

        /// <summary>
        /// Gets the plug-in's menu, which is created for you at startup.
        /// </summary>
        public static Menu PluginsMenu
        {
            get
            {
                if (_pluginsMenu == null)
                {
                    var id = MenusAPI.FindPluginsMenu();
                    if (id != default)
                    {
                        _pluginsMenu = new Menu(id);
                    }
                }

                return _pluginsMenu;
            }
        }

        /// <summary>
        /// Gets the aircraft menu for the currently-loaded aircraft,
        /// used for showing aircraft-specific commands.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The aircraft menu is created by X-Plane at startup,
        /// but it remains hidden until it is populated via
        /// <see cref="AddItem(string, out MenuItem, TypedEventHandler{MenuItem})"/>,
        /// <see cref="AddItem(string, Action{MenuItem})"/>,
        /// <see cref="AddItem(string, CommandRef, out MenuItem)"/>,
        /// <see cref="AddItem(string, CommandRef, Action{MenuItem})"/>,
        /// <see cref="AddItem(string, Command, out MenuItem)"/> or
        /// <see cref="AddItem(string, Command, Action{MenuItem})"/>.
        /// </para>
        /// <para>
        /// Only plugins loaded with the user's current aircraft are allowed to access
        /// the aircraft menu. For all other plugins, this will return <see langword="null"/>, and any
        /// attempts to add menu items to it will fail.
        /// </para>
        /// </remarks>
        public static Menu AircraftMenu
        {
            get
            {
                if (_aircraftMenu == null)
                {
                    var id = MenusAPI.FindAircraftMenu();
                    if (id != default)
                    {
                        _aircraftMenu = new Menu(id);
                    }
                }

                return _aircraftMenu;
            }
        }

        /// <summary>
        /// Adds a new menu item to the menu.
        /// </summary>
        /// <param name="name">The menu item display name.</param>
        /// <param name="item">The created menu item.</param>
        /// <param name="onClick">An optional Click event handler.</param>
        /// <returns>The current menu instance.</returns>
        public Menu AddItem(string name, out MenuItem item, TypedEventHandler<MenuItem> onClick = null)
        {
            item = _items.Add(new NormalMenuItem(this, name, onClick));
            return this;
        }

        /// <summary>
        /// Adds a new menu item to the menu.
        /// </summary>
        /// <param name="name">The menu item display name.</param>
        /// <param name="config">The delegate that can be used to configure the created menu item.</param>
        /// <returns>The current menu instance.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Menu AddItem(string name, Action<MenuItem> config = null)
        {
            AddItem(name, out var item);
            config?.Invoke(item);
            return this;
        }

        /// <summary>
        /// Adds a new menu item to the menu.
        /// </summary>
        /// <param name="name">The menu item display name.</param>
        /// <param name="commandRef">The command to execute on item click.</param>
        /// <param name="item">The created menu item.</param>
        /// <returns>The current menu instance.</returns>
        public Menu AddItem(string name, CommandRef commandRef, out MenuItem item)
        {
            item = _items.Add(new NormalMenuItem(this, name), commandRef);
            return this;
        }

        /// <summary>
        /// Adds a new menu item to the menu.
        /// </summary>
        /// <param name="name">The menu item display name.</param>
        /// <param name="commandRef">The command to execute on item click.</param>
        /// <param name="config">The delegate that can be used to configure the created menu item.</param>
        /// <returns>The current menu instance.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Menu AddItem(string name, CommandRef commandRef, Action<MenuItem> config = null)
        {
            AddItem(name, commandRef, out var item);
            config?.Invoke(item);
            return this;
        }

        /// <summary>
        /// Adds a new menu item to the menu.
        /// </summary>
        /// <param name="name">The menu item display name.</param>
        /// <param name="command">The command to execute on item click.</param>
        /// <param name="item">The created menu item.</param>
        /// <returns>The current menu instance.</returns>
        public Menu AddItem(string name, Command command, out MenuItem item)
        {
            return AddItem(name, command.CommandRef, out item);
        }

        /// <summary>
        /// Adds a new menu item to the menu.
        /// </summary>
        /// <param name="name">The menu item display name.</param>
        /// <param name="command">The command to execute on item click.</param>
        /// <param name="config">The delegate that can be used to configure the created menu item.</param>
        /// <returns>The current menu instance.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Menu AddItem(string name, Command command, Action<MenuItem> config = null)
        {
            return AddItem(name, command.CommandRef, config);
        }

        /// <summary>
        /// Adds a new menu item separator.
        /// </summary>
        /// <param name="item">The created separator.</param>
        /// <returns>The current menu instance.</returns>
        public Menu AddSeparator(out MenuItem item)
        {
            item = _items.Add(new SeparatorMenuItem());
            return this;
        }

        /// <summary>
        /// Adds a new menu item separator.
        /// </summary>
        /// <returns>The current menu instance.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Menu AddSeparator() => AddSeparator(out _);

        /// <summary>
        /// Removes the menu item by index.
        /// </summary>
        /// <param name="index">The index of the menu item to remove.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveItem(int index) => _items.RemoveAt(index);

        /// <summary>
        /// Removes the menu item.
        /// </summary>
        /// <param name="item">The menu item to remove.</param>
        /// <returns><see langword="true" /> if the item existed in the menu and was removed; <see langword="false" /> otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool RemoveItem(MenuItem item) => _items.Remove(item);

        /// <summary>
        /// Removes all menu items.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearItems() => _items.Clear();

        /// <summary>
        /// Raised when the user clicks a menu item.
        /// </summary>
        public event TypedEventHandler<Menu, MenuItem> Click; 

        /// <inheritdoc />
        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 0)
            {
                _items.Dispose();
                Click = null;

                if (_handle.IsAllocated)
                {
                    MenusAPI.DestroyMenu(_id);
                    _handle.Free();
                }
                else if (this == _pluginsMenu)
                {
                    _pluginsMenu = null;
                }
                else if (this == _aircraftMenu)
                {
                    _aircraftMenu = null;
                }

                _id = default;
            }
        }
    }
}
