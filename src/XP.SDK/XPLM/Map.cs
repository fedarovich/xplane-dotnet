using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Unicode;
using XP.SDK.XPLM.Interop;

namespace XP.SDK.XPLM
{
    public static class Map
    {
        public static readonly string UserInterface = "XPLM_MAP_USER_INTERFACE";

        public static readonly string InstructorOperatorStation = "XPLM_MAP_IOS";

        internal static readonly ReadOnlyMemory<byte> UserInterfaceUtf8;

        internal static readonly ReadOnlyMemory<byte> InstructorOperatorStationUtf8;

        static unsafe Map()
        {
            var userInterfaceUtf8 = new byte[UserInterface.Length + 1];
            Utf8.FromUtf16(UserInterface, userInterfaceUtf8, out _, out _);
            UserInterfaceUtf8 = userInterfaceUtf8;

            var instructorOperatorStationUtf8 = new byte[InstructorOperatorStationUtf8.Length + 1];
            Utf8.FromUtf16(InstructorOperatorStation, instructorOperatorStationUtf8, out _, out _);
            InstructorOperatorStationUtf8 = instructorOperatorStationUtf8;
        }

        public static unsafe void RegisterMapCreationHook(Action<string> callback)
        {
            var hook = new MapCreationHook(callback);
            PluginBase.RegisterObject(hook);
            MapAPI.RegisterMapCreationHook(
                &OnMapCreated,
                GCHandle.ToIntPtr(hook.Handle).ToPointer());

            [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
            static void OnMapCreated(byte* mapidentifier, void* refcon) =>
                Utils.TryGetObject<MapCreationHook>(refcon)?.Invoke(mapidentifier);
        }

        public static bool Exists(string mapIdentifier)
        {
            return MapAPI.MapExists(mapIdentifier) != 0;
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
