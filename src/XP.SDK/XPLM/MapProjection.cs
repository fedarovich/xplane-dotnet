using System.Runtime.CompilerServices;
using XP.SDK.XPLM.Internal;

namespace XP.SDK.XPLM
{
    public readonly ref struct MapProjection
    {
        private readonly MapProjectionID _id;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public MapProjection(MapProjectionID id)
        {
            _id = id;
        }

        public MapProjectionID Id
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _id;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator MapProjection(MapProjectionID id) => new MapProjection(id);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe (float x, float y) Project(double latitude, double longitude)
        {
            (float x, float y) result = default;
            MapAPI.MapProject(_id, latitude, longitude, &result.x, &result.y);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe (double latitude, double longitude) Unproject(float mapX, float mapY)
        {
            (double lat, double lon) result = default;
            MapAPI.MapUnproject(_id, mapX, mapY, &result.lat, &result.lon);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float ScaleMeter(float mapX, float mapY) => MapAPI.MapScaleMeter(_id, mapX, mapY);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float GetNorthHeading(float mapX, float mapY) => MapAPI.MapGetNorthHeading(_id, mapX, mapY);
    }
}
