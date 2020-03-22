using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using XP.SDK.XPLM.Internal;

namespace XP.SDK.XPLM
{
    public readonly struct PluginInfo
    {
        private readonly int _idPlusOne;

        public PluginID Id => unchecked(_idPlusOne - 1);

        public PluginInfo(PluginID id)
        {
            _idPlusOne = unchecked((int) id + 1);
        }

        public static PluginInfo ThisPlugin => new PluginInfo(PluginAPI.GetMyID());

        public static IReadOnlyList<PluginInfo> AllPlugins { get; } = new PluginInfoList();

        public unsafe string Name
        {
            get
            {
                Span<byte> buffer = stackalloc byte[256];
                PluginAPI.GetPluginInfo(Id, (byte*)Unsafe.AsPointer(ref buffer.GetPinnableReference()), null, null, null);
                return Encoding.UTF8.GetString(buffer[..buffer.IndexOf((byte) 0)]);
            }
        }

        public unsafe string FilePath
        {
            get
            {
                Span<byte> buffer = stackalloc byte[256];
                PluginAPI.GetPluginInfo(Id, null, (byte*)Unsafe.AsPointer(ref buffer.GetPinnableReference()), null, null);
                return Encoding.UTF8.GetString(buffer[..buffer.IndexOf((byte)0)]);
            }
        }

        public unsafe string Signature
        {
            get
            {
                Span<byte> buffer = stackalloc byte[256];
                PluginAPI.GetPluginInfo(Id, null, null, (byte*)Unsafe.AsPointer(ref buffer.GetPinnableReference()), null);
                return Encoding.UTF8.GetString(buffer[..buffer.IndexOf((byte)0)]);
            }
        }

        public unsafe string Description
        {
            get
            {
                Span<byte> buffer = stackalloc byte[256];
                PluginAPI.GetPluginInfo(Id, null, null, null, (byte*)Unsafe.AsPointer(ref buffer.GetPinnableReference()));
                return Encoding.UTF8.GetString(buffer[..buffer.IndexOf((byte)0)]);
            }
        }

        public bool IsEnabled => PluginAPI.IsPluginEnabled(Id) != 0;

        public bool Enable() => PluginAPI.EnablePlugin(Id) != 0;

        public void Disable() => PluginAPI.DisablePlugin(Id);

        public unsafe void SendMessage(int message, IntPtr param = default) => PluginAPI.SendMessageToPlugin(Id, message, param.ToPointer());

        public static unsafe void BroadcastMessage(int message, IntPtr param = default) => PluginAPI.SendMessageToPlugin(PluginID.NoPlugin, message, param.ToPointer());

        public static PluginInfo FindByPath(string path) => new PluginInfo(PluginAPI.FindPluginByPath(path));

        public static PluginInfo FindBySignature(string signature) => new PluginInfo(PluginAPI.FindPluginBySignature(signature));

        public static void ReloadPlugins() => PluginAPI.ReloadPlugins();

        private class PluginInfoList : IReadOnlyList<PluginInfo>
        {
            public IEnumerator<PluginInfo> GetEnumerator()
            {
                int count = PluginAPI.CountPlugins();
                for (int index = 0; index < count; index++)
                {
                    yield return new PluginInfo(PluginAPI.GetNthPlugin(index));
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public int Count => PluginAPI.CountPlugins();

            public PluginInfo this[int index]
            {
                get
                {
                    if (index < 0 || index >= Count)
                        throw new ArgumentOutOfRangeException(nameof(index));

                    return new PluginInfo(PluginAPI.GetNthPlugin(index));
                }
            }
        }
    }
}
