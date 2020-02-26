using System;

namespace XP.SDK.Widgets
{
    
    /// <summary>
    /// <para>
    /// The dispatching modes describe how the widgets library sends out messages.
    /// Currently there are three modes:
    /// </para>
    /// </summary>
    public enum DispatchMode
    {
        Direct = 0,
        UpChain = 1,
        Recursive = 2,
        DirectAllCallbacks = 3,
        Once = 4
    }
}