using System;
using System.Runtime.CompilerServices;

namespace XP.SDK.XPLM.Internal
{
    
    /// <summary>
    /// <para>
    /// This structure defines all of the parameters used to create a map layer
    /// using XPLMCreateMapLayer. The structure will be expanded in future SDK APIs
    /// to include more features.  Always set the structSize member to the size of
    /// your struct in bytes!
    /// </para>
    /// <para>
    /// Each layer must be associated with exactly one map instance in X-Plane.
    /// That map, and that map alone, will call your callbacks. Likewise, when that
    /// map is deleted, your layer will be as well.
    /// </para>
    /// </summary>
    public unsafe partial struct CreateMapLayer
    {
        public int structSize;
        public byte* mapToCreateLayerIn;
        public MapLayerType layerType;
        [ManagedTypeAttribute(typeof(MapWillBeDeletedCallback))]
        public IntPtr willBeDeletedCallback;
        [ManagedTypeAttribute(typeof(MapPrepareCacheCallback))]
        public IntPtr prepCacheCallback;
        [ManagedTypeAttribute(typeof(MapDrawingCallback))]
        public IntPtr drawCallback;
        [ManagedTypeAttribute(typeof(MapIconDrawingCallback))]
        public IntPtr iconCallback;
        [ManagedTypeAttribute(typeof(MapLabelDrawingCallback))]
        public IntPtr labelCallback;
        public int showUiToggle;
        public byte* layerName;
        public void* refcon;
    }
}