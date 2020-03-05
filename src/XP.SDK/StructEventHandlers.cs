using System;
using System.Collections.Generic;
using System.Text;

namespace XP.SDK
{
    public delegate void InStructEventHandler<T>(object sender, in T args) where T : struct;

    public delegate void RefStructEventHandler<T>(object sender, ref T args) where T : struct;
}
