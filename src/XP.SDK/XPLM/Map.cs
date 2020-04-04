using System;
using System.Runtime.InteropServices;
using System.Text.Unicode;
using System.Threading;
using XP.SDK.XPLM.Internal;

namespace XP.SDK.XPLM
{
    public static class Map
    {
        public static readonly string UserInterface = "XPLM_MAP_USER_INTERFACE";

        public static readonly string InstructorOperatorStation = "XPLM_MAP_IOS";

        private static readonly ReadOnlyMemory<byte> UserInterfaceUtf8;

        private static readonly ReadOnlyMemory<byte> InstructorOperatorStationUtf8;

        private static readonly MapCreatedCallback MapCreatedCallback;

        static unsafe Map()
        {
            var userInterfaceUtf8 = new byte[UserInterface.Length + 1];
            Utf8.FromUtf16(UserInterface, userInterfaceUtf8, out _, out _);
            UserInterfaceUtf8 = userInterfaceUtf8;

            var instructorOperatorStationUtf8 = new byte[InstructorOperatorStationUtf8.Length + 1];
            Utf8.FromUtf16(InstructorOperatorStation, instructorOperatorStationUtf8, out _, out _);
            InstructorOperatorStationUtf8 = instructorOperatorStationUtf8;

            MapCreatedCallback = OnMapCreated;

            static void OnMapCreated(byte* mapidentifier, void* refcon) => 
                Utils.TryGetObject<MapCreationHook>(refcon)?.Invoke(mapidentifier);
        }

        public static unsafe void RegisterMapCreationHook(Action<string> callback)
        {
            var hook = new MapCreationHook(callback);
            PluginBase.RegisterObject(hook);
            MapAPI.RegisterMapCreationHook(
                MapCreatedCallback,
                GCHandle.ToIntPtr(hook.Handle).ToPointer());
        }

        private sealed class MapCreationHook : DelegateWrapper<Action<string>>
        {
            public MapCreationHook(Action<string> @delegate) : base(@delegate)
            {
            }

            public unsafe void Invoke(byte* mapIdentifier)
            {
                var len = Utils.StrLen(mapIdentifier);
                var span = new Span<byte>(mapIdentifier, len + 1);
                if (span.SequenceEqual(UserInterfaceUtf8.Span))
                {
                    Delegate.Invoke(UserInterface);
                }
                else if (span.SequenceEqual(InstructorOperatorStationUtf8.Span))
                {
                    Delegate.Invoke(InstructorOperatorStation);
                }
                else
                {
                    Delegate.Invoke(Marshal.PtrToStringUTF8(new IntPtr(mapIdentifier)));
                }
            }
        }
    }
}
