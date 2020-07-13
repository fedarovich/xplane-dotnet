#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using XP.SDK.XPLM.Internal;

namespace XP.SDK.XPLM
{
    internal sealed class NormalMenuItem : MenuItem
    {
        private string _name;
        private bool _isEnabled = true;
        private Menu? _subMenu;

        internal NormalMenuItem(Menu parentMenu, string name)
        {
            ParentMenu = parentMenu ?? throw new ArgumentNullException(nameof(parentMenu));
            _name = name ?? throw new ArgumentNullException(nameof(name));
        }

        internal NormalMenuItem(Menu parentMenu, string name, TypedEventHandler<MenuItem>? onClick) 
            : this(parentMenu, name)
        {
            Click += onClick;
        }

        internal Menu ParentMenu { get; }

        public override string Name
        {
            get => _name;
            set
            {
                var index = MenuItemList.GetIndex(this);
                if (index >= 0)
                {
                    _name = value;
                    MenusAPI.SetMenuItemName(ParentMenu.Id, index.Value, value, 0);
                }
            }
        }

        public override bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                var index = MenuItemList.GetIndex(this);
                if (index >= 0)
                {
                    _isEnabled = value;
                    MenusAPI.EnableMenuItem(ParentMenu.Id, index.Value, value.ToInt());
                }
            }
        }

        public override MenuCheck CheckState
        {
            get
            {
                unsafe
                {
                    var index = MenuItemList.GetIndex(this);
                    if (index >= 0)
                    {
                        MenuCheck state;
                        MenusAPI.CheckMenuItemState(ParentMenu.Id, index.Value, &state);
                        return state;
                    }
                    return MenuCheck.NoCheck;
                }
            }
            set
            {
                var index = MenuItemList.GetIndex(this);
                if (index >= 0)
                {
                    MenusAPI.CheckMenuItem(ParentMenu.Id, index.Value, value);
                }
            } 
        }

        public override bool IsSeparator => false;

        public override Menu? SubMenu => _subMenu;
        
        public override Menu CreateSubMenu()
        {
            if (_subMenu != null)
                throw new InvalidOperationException("The sub-menu already exists.");

            _subMenu = new Menu(this);
            return _subMenu;
        }

        public override Menu GetOrCreateSubMenu()
        {
            _subMenu ??= new Menu(this);
            return _subMenu;
        }

        public override void DestroySubMenu()
        {
            if (_subMenu != null)
            {
                _subMenu.Dispose();
                _subMenu = null;
            }
        }

        public event TypedEventHandler<MenuItem>? Click; 

        internal override void OnClick()
        {
            Click?.Invoke(this, EventArgs.Empty);
        }
    }
}
