using System;
using System.Runtime.InteropServices;
using XP.SDK.XPLM.Interop;

namespace XP.SDK.XPLM
{
    public static class Map
    {
        /// <summary>
        /// Globally unique identifier for X-Plane’s Map window.
        /// </summary>
        /// <returns>
        /// The UTF-8 literal <c>XPLM_MAP_USER_INTERFACE</c>.
        /// </returns>
        public static Utf8String UserInterface => new (new byte[] { 88, 80, 76, 77, 95, 77, 65, 80, 95, 85, 83, 69, 82, 95, 73, 78, 84, 69, 82, 70, 65, 67, 69, 0 }, 23);

        /// <summary>
        /// Globally unique identifier for X-Plane’s Instructor Operator Station window.
        /// </summary>
        /// <returns>
        /// The UTF-8 literal <c>XPLM_MAP_IOS</c>.
        /// </returns>
        public static Utf8String InstructorOperatorStation => new (new byte[] { 88, 80, 76, 77, 95, 77, 65, 80, 95, 73, 79, 83, 0 }, 12);

        /// <summary>
        /// <para>
        /// Registers your callback to receive a notification each time a new map is
        /// constructed in X-Plane. This callback is the best time to add your custom
        /// <see cref="MapLayer"/>.
        /// </para>
        /// <para>
        /// Note that you will not be notified about any maps that already exist.
        /// You  can use <see cref="Exists"/> to check for maps that were created previously.
        /// </para>
        /// </summary>
        public static unsafe void RegisterMapCreationHook(CreationHook callback)
        {
            var hook = new MapCreationHook(callback);
            PluginBase.RegisterObject(hook);
            MapAPI.RegisterMapCreationHook(
                &OnMapCreated,
                GCHandle.ToIntPtr(hook.Handle).ToPointer());

            [UnmanagedCallersOnly]
            static void OnMapCreated(byte* mapidentifier, void* refcon) =>
                Utils.TryGetObject<MapCreationHook>(refcon)?.Invoke(mapidentifier);
        }

        /// <summary>
        /// Returns <see langword="true"/> if the map with the specified identifier already exists in
        /// X-Plane. In that case, you can safely create <see cref="MapLayer"/> specifying
        /// that your layer should be added to that map.
        /// </summary>
        public static bool Exists(in Utf8String mapIdentifier)
        {
            return MapAPI.MapExists(mapIdentifier) != 0;
        }

        /// <summary>
        /// Returns <see langword="true"/> if the map with the specified identifier already exists in
        /// X-Plane. In that case, you can safely create <see cref="MapLayer"/> specifying
        /// that your layer should be added to that map.
        /// </summary>
        public static bool Exists(in ReadOnlySpan<char> mapIdentifier)
        {
            return MapAPI.MapExists(mapIdentifier) != 0;
        }

        /// <summary>
        /// Map creation hook.
        /// </summary>
        public delegate void CreationHook(in Utf8String mapIdentifier);

        private sealed class MapCreationHook : DelegateWrapper<CreationHook>
        {
            public MapCreationHook(CreationHook @delegate) : base(@delegate)
            {
            }

            public unsafe void Invoke(byte* mapIdentifier)
            {
                Delegate.Invoke(new Utf8String(mapIdentifier));
            }
        }
    }
}
