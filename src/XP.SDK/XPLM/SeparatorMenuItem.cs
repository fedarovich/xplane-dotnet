#nullable enable
using System;

namespace XP.SDK.XPLM
{
    internal sealed class SeparatorMenuItem : MenuItem
    {
        public override string Name
        {
            get => string.Empty;
            set { }
        }

        public override bool IsEnabled
        {
            get => false;
            set { }
        }

        public override MenuCheck CheckState
        {
            get => MenuCheck.NoCheck;
            set { }
        }

        public override bool IsSeparator => true;

        public override Menu? SubMenu => null;
        
        public override Menu CreateSubMenu()
        {
            throw new InvalidOperationException("A separator cannot contain a sub-menu.");
        }

        public override Menu GetOrCreateSubMenu()
        {
            throw new InvalidOperationException("A separator cannot contain a sub-menu.");
        }

        public override void DestroySubMenu()
        {
        }

        public override event TypedEventHandler<MenuItem>? Click
        {
            add { }
            remove { }
        }
    }
}
