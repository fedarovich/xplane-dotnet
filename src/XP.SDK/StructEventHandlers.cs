using System;
using System.Collections.Generic;
using System.Text;

namespace XP.SDK
{
    public delegate void TypedEventHandler<in TSender>(TSender sender, EventArgs args);

    public delegate void TypedEventHandler<in TSender, in TArgs>(TSender sender, TArgs args);

    public delegate void InStructEventHandler<TArgs>(object sender, in TArgs args) where TArgs : struct;

    public delegate void InStructEventHandler<in TSender, TArgs>(TSender sender, in TArgs args) where TArgs : struct;

    public delegate void RefStructEventHandler<TArgs>(object sender, ref TArgs args) where TArgs : struct;

    public delegate void RefStructEventHandler<in TSender, TArgs>(TSender sender, ref TArgs args) where TArgs : struct;
}
