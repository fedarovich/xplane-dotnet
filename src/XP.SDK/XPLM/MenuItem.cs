using System;
using System.Collections.Generic;
using System.Threading;

#nullable enable

namespace XP.SDK.XPLM
{
    /// <summary>
    /// The menu item.
    /// </summary>
    public abstract class MenuItem
    {
        private static long _counter;

        private protected MenuItem()
        {
            UniqueId = ++_counter;
        }

        /// <summary>
        /// Gets or sets the menu item name.
        /// </summary>
        public abstract string Name { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether the menu item is enabled.
        /// </summary>
        public abstract bool IsEnabled { get; set; } 

        /// <summary>
        /// Gets or sets the check state of the menu.
        /// </summary>
        public abstract MenuCheck CheckState { get; set; }

        /// <summary>
        /// Gets the value indicating whether the menu item is a separator.
        /// </summary>
        public abstract bool IsSeparator { get; }

        /// <summary>
        /// Gets the item's sub-menu.
        /// </summary>
        public abstract Menu? SubMenu { get; }

        /// <summary>
        /// Creates an item's sub-menu.
        /// </summary>
        /// <returns>The new item's sub-menu.</returns>
        /// <exception cref="InvalidOperationException">The item's sub-menu already exists.</exception>
        /// <exception cref="InvalidOperationException">The item is a separator.</exception>
        public abstract Menu CreateSubMenu();

        /// <summary>
        /// Gets or creates an item's sub-menu.
        /// </summary>
        /// <returns>The item's sub-menu.</returns>
        /// <exception cref="InvalidOperationException">The item is a separator.</exception>
        public abstract Menu GetOrCreateSubMenu();

        /// <summary>
        /// Destroys the item's sub-menu.
        /// </summary>
        public abstract void DestroySubMenu();

        /// <summary>
        /// Gets the item's sub-menu.
        /// </summary>
        public long UniqueId { get; }

        internal virtual void OnClick()
        {
        }
    }
}
