using System;
using System.Runtime.CompilerServices;

namespace XP.SDK.XPLM.Internal
{
    public unsafe partial struct CreateMapLayer
    {
        public int structSize;
        public byte *mapToCreateLayerIn;
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
        public byte *layerName;
        public void *refcon;
    }
}