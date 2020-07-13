using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using XP.SDK.XPLM.Internal;

namespace XP.SDK.XPLM
{
    internal class MenuItemList : IReadOnlyList<MenuItem>, IDisposable
    {
        private static readonly ConditionalWeakTable<MenuItem, MenuItemList> _lists = new ConditionalWeakTable<MenuItem, MenuItemList>();

        private readonly MenuID _menuId;
        private readonly List<MenuItem> _itemList = new List<MenuItem>(8);
        private bool _disposed;

        public MenuItemList(MenuID menuId)
        {
            _menuId = menuId;
        }

        public IEnumerator<MenuItem> GetEnumerator() => _itemList.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => _itemList.Count;

        public MenuItem this[int index] => _itemList[index];

        internal unsafe void Add(MenuItem item)
        {
            if (_disposed) 
                throw new ObjectDisposedException(nameof(Menu));

            _itemList.Add(item);
            MenusAPI.AppendMenuItem(_menuId, item.Name, (void*) item.UniqueId, 0);
            _lists.Add(item, this);
        }

        internal void RemoveAt(int index)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Menu));

            var item = _itemList[index];
            item.DestroySubMenu();
            _lists.Remove(item);
            MenusAPI.RemoveMenuItem(_menuId, index);
            _itemList.RemoveAt(index);
        }

        internal bool Remove(MenuItem item)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Menu));

            var index = _itemList.IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }

            return false;
        }

        internal void Clear()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Menu));

            foreach (var item in _itemList)
            {
                item.DestroySubMenu();
                _lists.Remove(item);
            }
            _itemList.Clear();
            MenusAPI.ClearAllMenuItems(_menuId);
        }

        internal static int? GetIndex(MenuItem menuItem)
        {
            if (!_lists.TryGetValue(menuItem, out var list))
                return null;

            var index = list._itemList.IndexOf(menuItem);
            return index >= 0 ? index : (int?) null;
        }

        internal MenuItem FindByUniqueId(long uniqueId)
        {
            return _itemList.Find(i => i.UniqueId == uniqueId);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                Clear();
                _disposed = true;
            }
        }
    }
}
