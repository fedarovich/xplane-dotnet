using System;

namespace XP.SDK.Widgets
{
    public enum DispatchMode
    {
        Direct = 0,
        UpChain = 1,
        Recursive = 2,
        DirectAllCallbacks = 3,
        Once = 4
    }
}