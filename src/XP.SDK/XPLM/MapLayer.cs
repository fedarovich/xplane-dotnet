using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using XP.SDK.XPLM.Interop;

namespace XP.SDK.XPLM
{
    public abstract class MapLayer : IDisposable
    {
        private GCHandle _handle;
        private MapLayerID _id;
        private int _disposed;

        #region Constructors and Disposal

        protected unsafe MapLayer(string map, MapLayerType layerType, string layerName, bool showToggle)
        {
            if (map == null) 
                throw new ArgumentNullException(nameof(map));
            if (layerName == null) 
                throw new ArgumentNullException(nameof(layerName));

            // TODO: Replace with Utf8String
            
            Span<byte> mapSpan = stackalloc byte[(map.Length << 1) | 1];
            byte* pMap = Utils.ToUtf8Unsafe(map, mapSpan);

            Span<byte> layerNameSpan = stackalloc byte[(layerName.Length << 1) | 1];
            byte* pLayerName = Utils.ToUtf8Unsafe(layerName, layerNameSpan);

            var handle = GCHandle.Alloc(this);

            var createStruct = new CreateMapLayer
            {
                structSize = Unsafe.SizeOf<CreateMapLayer>(),
                mapToCreateLayerIn = pMap,
                layerType = layerType,
                willBeDeletedCallback = &HandleWillBeDeletedCallback,
                prepCacheCallback = &HandlePrepareCacheCallback,
                drawCallback = &HandleMapDrawingCallback,
                iconCallback = &HandleMapIconDrawingCallback,
                labelCallback = &HandleMapLabelDrawingCallback,
                layerName = pLayerName,
                showUiToggle = showToggle.ToInt(),
                refcon = GCHandle.ToIntPtr(handle).ToPointer()
            };

            _id = MapAPI.CreateMapLayer(&createStruct);
            if (_id == default)
            {
                handle.Free();
                throw new MapLayerCreationFailedException();
            }

            _handle = handle;

            [UnmanagedCallersOnly]
            static void HandleWillBeDeletedCallback(MapLayerID inlayer, void* inrefcon) => Utils.TryGetObject<MapLayer>(inrefcon)?.HandleWillBeDeleted();

            [UnmanagedCallersOnly]
            static void HandlePrepareCacheCallback(MapLayerID inlayer, float* inTotalMapBoundsLeftTopRightBottom, MapProjectionID projection, void* inrefcon)
            {
                ref readonly RectF bounds = ref Unsafe.AsRef<RectF>((RectF*)inTotalMapBoundsLeftTopRightBottom);
                Utils.TryGetObject<MapLayer>(inrefcon)?.OnPreparingCache(bounds, projection);
            }

            [UnmanagedCallersOnly]
            static void HandleMapDrawingCallback(MapLayerID inlayer, float* inMapBoundsLeftTopRightBottom, float zoomRatio, float mapUnitsPerUserInterfaceUnit, MapStyle mapStyle, MapProjectionID projection, void* inrefcon)
            {
                ref readonly RectF bounds = ref Unsafe.AsRef<RectF>((RectF*)inMapBoundsLeftTopRightBottom);
                Utils.TryGetObject<MapLayer>(inrefcon)?.OnMapDrawing(bounds, zoomRatio, mapUnitsPerUserInterfaceUnit, mapStyle, projection);
            }

            [UnmanagedCallersOnly]
            static void HandleMapIconDrawingCallback(MapLayerID inlayer, float* inMapBoundsLeftTopRightBottom, float zoomRatio, float mapUnitsPerUserInterfaceUnit, MapStyle mapStyle, MapProjectionID projection, void* inrefcon)
            {
                ref readonly RectF bounds = ref Unsafe.AsRef<RectF>((RectF*)inMapBoundsLeftTopRightBottom);
                Utils.TryGetObject<MapLayer>(inrefcon)?.HandleIconDrawing(bounds, zoomRatio, mapUnitsPerUserInterfaceUnit, mapStyle, projection);
            }

            [UnmanagedCallersOnly]
            static void HandleMapLabelDrawingCallback(MapLayerID inlayer, float* inMapBoundsLeftTopRightBottom, float zoomRatio, float mapUnitsPerUserInterfaceUnit, MapStyle mapStyle, MapProjectionID projection, void* inrefcon)
            {
                ref readonly RectF bounds = ref Unsafe.AsRef<RectF>((RectF*)inMapBoundsLeftTopRightBottom);
                Utils.TryGetObject<MapLayer>(inrefcon)?.HandleLabelDrawing(bounds, zoomRatio, mapUnitsPerUserInterfaceUnit, mapStyle, projection);
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 0)
            {
                MapAPI.DestroyMapLayer(_id);
            }
        }

        #endregion

        #region Callbacks

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void HandleWillBeDeleted()
        {
            try
            {
                OnDeleting();
            }
            finally
            {
                if (_handle.IsAllocated)
                {
                    _handle.Free();
                    _id = default;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void HandleIconDrawing(in RectF bounds, float zoomRatio, float mapUnitsPerUserInterfaceUnit, MapStyle mapStyle, MapProjection projection)
        {
            OnIconDrawing(bounds, zoomRatio, mapUnitsPerUserInterfaceUnit, mapStyle, projection, new IconFunctions(_id));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void HandleLabelDrawing(in RectF bounds, float zoomRatio, float mapUnitsPerUserInterfaceUnit, MapStyle mapStyle, MapProjection projection)
        {
            OnLabelDrawing(bounds, zoomRatio, mapUnitsPerUserInterfaceUnit, mapStyle, projection, new LabelFunctions(_id));
        }

        protected virtual void OnDeleting()
        {
        }

        protected virtual void OnPreparingCache(in RectF bounds, MapProjection projection)
        {
        }

        protected virtual void OnMapDrawing(in RectF bounds, float zoomRatio, float mapUnitsPerUserInterfaceUnit, MapStyle mapStyle, MapProjection projection)
        {
        }

        protected virtual void OnIconDrawing(in RectF bounds, float zoomRatio, float mapUnitsPerUserInterfaceUnit, MapStyle mapStyle, MapProjection projection, IconFunctions iconFunctions)
        {
        }

        protected virtual void OnLabelDrawing(in RectF bounds, float zoomRatio, float mapUnitsPerUserInterfaceUnit, MapStyle mapStyle, MapProjection projection, LabelFunctions labelFunctions)
        {
        }

        #endregion

        #region Drawing

        protected readonly ref struct IconFunctions
        {
            private readonly MapLayerID _layerId;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal IconFunctions(MapLayerID layerId)
            {
                _layerId = layerId;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void DrawMapIconFromSheet(
                in ReadOnlySpan<char> pngPath, 
                int s, 
                int t, 
                int ds, 
                int dt, 
                float mapX, 
                float mapY,
                MapOrientation orientation, 
                float rotationDegrees, 
                float mapWidth)
            {
                MapAPI.DrawMapIconFromSheet(_layerId, pngPath, s, t, ds, dt, mapX, mapY, orientation, rotationDegrees, mapWidth);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public unsafe void DrawMapIconFromSheet(
                in ReadOnlySpan<byte> pngPathUtf8,
                int s,
                int t,
                int ds,
                int dt,
                float mapX,
                float mapY,
                MapOrientation orientation,
                float rotationDegrees,
                float mapWidth)
            {
                fixed (byte* path = pngPathUtf8)
                {
                    MapAPI.DrawMapIconFromSheet(_layerId, path, s, t, ds, dt, mapX, mapY, orientation, rotationDegrees, mapWidth);
                }
            }
        }

        protected readonly ref struct LabelFunctions
        {
            private readonly MapLayerID _layerId;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal LabelFunctions(MapLayerID layerId)
            {
                _layerId = layerId;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void DrawMapLabel(in ReadOnlySpan<char> text, float mapX, float mapY, MapOrientation orientation, float rotationDegrees)
            {
                MapAPI.DrawMapLabel(_layerId, text, mapX, mapY, orientation, rotationDegrees);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public unsafe void DrawMapLabel(in ReadOnlySpan<byte> textUtf8, float mapX, float mapY, MapOrientation orientation, float rotationDegrees)
            {
                fixed (byte* text = textUtf8)
                {
                    MapAPI.DrawMapLabel(_layerId, text, mapX, mapY, orientation, rotationDegrees);
                }
            }
        }

        #endregion
    }
}
