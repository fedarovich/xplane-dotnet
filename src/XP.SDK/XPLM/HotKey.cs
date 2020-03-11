using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using XP.SDK.XPLM.Internal;

namespace XP.SDK.XPLM
{
    public sealed class HotKey : IDisposable
    {
        private readonly Action<HotKey> _action;
        private GCHandle _handle;
        private HotKeyID _id;
        private int _disposed;

        #region Constructors

        private static class CallbackHolder
        {
            public static readonly HotKeyCallback HotKeyCallback;

            static unsafe CallbackHolder()
            {
                HotKeyCallback = OnHotKey;
            }

            private static unsafe void OnHotKey(void* inrefcon) =>
                Utils.TryGetObject<HotKey>(inrefcon)?.OnHotKey();

        }

        private HotKey(Action<HotKey> action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
            _handle = GCHandle.Alloc(_handle);
        }

        public static unsafe HotKey Register(byte virtualKey, KeyFlags keyFlags, string description, Action<HotKey> action)
        {
            var hotKey = new HotKey(action);
            hotKey._id = Display.RegisterHotKey(
                virtualKey, 
                keyFlags, 
                description,
                CallbackHolder.HotKeyCallback,
                GCHandle.ToIntPtr(hotKey._handle).ToPointer());
            return hotKey;
        }

        #endregion

        public HotKeyID Id
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _id;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnHotKey() => _action(this);

        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 0)
            {
                Display.UnregisterHotKey(_id);
                _handle.Free();
                _id = default;
            }
        }

        /// <summary>
        /// Remaps a hot keys keystrokes. You may remap another plugin's keystrokes.
        /// </summary>
        public static void SetHotKeyCombination(HotKeyID id, byte virtualKey, KeyFlags keyFlags)
        {
            Display.SetHotKeyCombination(id, virtualKey, keyFlags);
        }

        public static int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Display.CountHotKeys();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HotKeyID GetIdByIndex(int index) => Display.GetNthHotKey(index);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HotKeyInfo GetInfoByIndex(int index, bool withDescription = true) => new HotKeyInfo(Display.GetNthHotKey(index), withDescription);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HotKeyInfo GetInfoById(HotKeyID id, bool withDescription = true) => new HotKeyInfo(id, withDescription);

        public readonly struct HotKeyInfo
        {
            internal unsafe HotKeyInfo(HotKeyID id, bool withDescription)
            {
                Id = id;
                PluginID pluginId;
                KeyFlags flags;
                byte virtualKey;
                Span<byte> description = withDescription ? stackalloc byte[512] : default;
                Display.GetHotKeyInfo(id, 
                    &virtualKey, 
                    &flags,
                    (byte*)Unsafe.AsPointer(ref description.GetPinnableReference()),
                    &pluginId);
                PluginId = pluginId;
                KeyFlags = flags;
                VirtualKey = virtualKey;
                Description = withDescription 
                    ? Encoding.UTF8.GetString(description[..description.IndexOf((byte) 0)])
                    : null;
            }

            public HotKeyID Id { get; }

            public string Description { get; }

            public PluginID PluginId { get; }

            public KeyFlags KeyFlags { get; }

            public byte VirtualKey { get; }
        }
    }
}
