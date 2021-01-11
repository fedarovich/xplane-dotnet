#nullable enable
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using XP.SDK.XPLM.Interop;

namespace XP.SDK.XPLM
{
    /// <summary>
    /// Provides a way to register hot keys.
    /// </summary>
    public sealed class HotKey : IDisposable
    {
        private const int DescriptionMaxLength = 512;
        
        private readonly Action<HotKey> _action;
        private GCHandle _handle;
        private HotKeyID _id;
        private int _disposed;

        #region Constructors

        private HotKey(Action<HotKey> action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
            _handle = GCHandle.Alloc(_handle);
        }

        /// <summary>
        /// Registers a new hotkey.
        /// </summary>
        /// <param name="virtualKey">The virtual key.</param>
        /// <param name="keyFlags">The flags combination.</param>
        /// <param name="description">The hot key description.</param>
        /// <param name="action">The action to invoke when the hot key is activated.</param>
        /// <returns>The new <see cref="HotKey"/> instance.</returns>
        /// <remarks>
        /// During execution, the actual key associated with your hot key may change, but you are insulated from this.
        /// </remarks>
        /// <exception cref="ArgumentException"><paramref name="description"/> or <paramref name="action"/> is <see langword="null"/>.</exception>
        public static unsafe HotKey Register(byte virtualKey, KeyFlags keyFlags, string description, Action<HotKey> action)
        {
            if (description == null) 
                throw new ArgumentNullException(nameof(description));
            
            if (action == null) 
                throw new ArgumentNullException(nameof(action));
            
            var hotKey = new HotKey(action);
            hotKey._id = DisplayAPI.RegisterHotKey(
                virtualKey, 
                keyFlags, 
                description,
                &OnHotKey,
                GCHandle.ToIntPtr(hotKey._handle).ToPointer());
            return hotKey;

            [UnmanagedCallersOnly]
            static void OnHotKey(void* inrefcon) =>
                Utils.TryGetObject<HotKey>(inrefcon)?.OnHotKey();
        }

        #endregion

        /// <summary>
        /// The hot key ID.
        /// </summary>
        public HotKeyID Id
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _id;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnHotKey() => _action(this);

        /// <inheritdoc />
        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 0)
            {
                DisplayAPI.UnregisterHotKey(_id);
                _handle.Free();
                _id = default;
            }
        }

        /// <summary>
        /// Remaps a hot keys keystrokes. You may remap another plugin's keystrokes.
        /// </summary>
        public static void SetHotKeyCombination(HotKeyID id, byte virtualKey, KeyFlags keyFlags)
        {
            DisplayAPI.SetHotKeyCombination(id, virtualKey, keyFlags);
        }

        /// <summary>
        /// Gets the number of current hot keys.
        /// </summary>
        public static int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => DisplayAPI.CountHotKeys();
        }

        /// <summary>
        /// Gets the hot key ID by index.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HotKeyID GetIdByIndex(int index) => DisplayAPI.GetNthHotKey(index);

        /// <summary>
        /// Gets the hot key description by index as UTF-8 string.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Utf8String GetDescriptionByIndex(int index) => GetDescriptionById(DisplayAPI.GetNthHotKey(index));

        /// <summary>
        /// Gets the hot key description by ID as UTF-8 string.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Utf8String GetDescriptionById(HotKeyID id)
        {
            Span<byte> buffer = GC.AllocateUninitializedArray<byte>(DescriptionMaxLength);
            fixed (byte* pBuffer = buffer)
            {
                DisplayAPI.GetHotKeyInfo(id, null, null, pBuffer, null);
                return new Utf8String(buffer, (int)Utils.CStringLength(pBuffer));
            }
        }

        /// <summary>
        /// Gets the hot key description by index as UTF-8 string.
        /// </summary>
        /// <param name="index">The hot key index.</param>
        /// <param name="buffer">The buffer to fill with the description in UTF-8 encoding.</param>
        /// <returns><see langword="true"/> if the <paramref name="buffer"/> is at least 512 bytes long; <see langword="false"/> otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetDescriptionByIndex(int index, in Span<byte> buffer) => TryGetDescriptionById(DisplayAPI.GetNthHotKey(index), in buffer);

        /// <summary>
        /// Gets the hot key description by index as UTF-8 string.
        /// </summary>
        /// <param name="index">The hot key index.</param>
        /// <param name="buffer">The buffer to fill with the description in UTF-8 encoding.</param>
        /// <param name="utf8String">The resulting <see cref="Utf8String"/>.</param>
        /// <returns><see langword="true"/> if the <paramref name="buffer"/> is at least 512 bytes long; <see langword="false"/> otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetDescriptionByIndex(int index, in Span<byte> buffer, out Utf8String utf8String) => TryGetDescriptionById(DisplayAPI.GetNthHotKey(index), in buffer, out utf8String);

        /// <summary>
        /// Gets the hot key description by ID as UTF-8 string.
        /// </summary>
        /// <param name="id">The hot key ID.</param>
        /// <param name="buffer">The buffer to fill with the description in UTF-8 encoding.</param>
        /// <returns><see langword="true"/> if the <paramref name="buffer"/> is at least 512 bytes long; <see langword="false"/> otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool TryGetDescriptionById(HotKeyID id, in Span<byte> buffer)
        {
            if (buffer.Length < DescriptionMaxLength)
                return false;

            fixed (byte* pBuffer = buffer)
            {
                DisplayAPI.GetHotKeyInfo(id, null, null, pBuffer, null);
                return true;
            }
        }

        /// <summary>
        /// Gets the hot key description by ID as UTF-8 string.
        /// </summary>
        /// <param name="id">The hot key ID.</param>
        /// <param name="buffer">The buffer to fill with the description in UTF-8 encoding.</param>
        /// <param name="utf8String">The resulting <see cref="Utf8String"/>.</param>
        /// <returns><see langword="true"/> if the <paramref name="buffer"/> is at least 512 bytes long; <see langword="false"/> otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool TryGetDescriptionById(HotKeyID id, in Span<byte> buffer, out Utf8String utf8String)
        {
            if (buffer.Length < DescriptionMaxLength)
            {
                utf8String = default;
                return false;
            }

            fixed (byte* pBuffer = buffer)
            {
                DisplayAPI.GetHotKeyInfo(id, null, null, pBuffer, null);
                utf8String = new Utf8String(buffer, (int)Utils.CStringLength(pBuffer));
                return true;
            }
        }

        /// <summary>
        /// Gets the information about the hot key with the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The hot key index.</param>
        /// <param name="withDescription"><see langword="true"/> to include the description.</param>
        /// <returns>The instance of <see cref="HotKeyInfo"/> providing the information about the hot key.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HotKeyInfo GetInfoByIndex(int index, bool withDescription = true) => new (DisplayAPI.GetNthHotKey(index), withDescription);

        /// <summary>
        /// Gets the information about the hot key with the specified <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The hot key ID.</param>
        /// <param name="withDescription"><see langword="true"/> to include the description.</param>
        /// <returns>The instance of <see cref="HotKeyInfo"/> providing the information about the hot key.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HotKeyInfo GetInfoById(HotKeyID id, bool withDescription = true) => new (id, withDescription);

        /// <summary>
        /// Contains the information about a hot key.
        /// </summary>
        public readonly struct HotKeyInfo
        {
            [SkipLocalsInit]
            internal unsafe HotKeyInfo(HotKeyID id, bool withDescription)
            {
                Id = id;
                PluginID pluginId;
                KeyFlags flags;
                byte virtualKey;
                Span<byte> description = withDescription ? stackalloc byte[DescriptionMaxLength] : default;
                byte* pDescription = (byte*) Unsafe.AsPointer(ref description.GetPinnableReference());
                
                DisplayAPI.GetHotKeyInfo(id, 
                    &virtualKey, 
                    &flags,
                    pDescription,
                    &pluginId);
                PluginId = pluginId;
                KeyFlags = flags;
                VirtualKey = virtualKey;
                Description = withDescription 
                    ? Encoding.UTF8.GetString(description[..(int)Utils.CStringLength(pDescription)])
                    : null;
            }

            /// <summary>
            /// Gets the hot key ID.
            /// </summary>
            public HotKeyID Id { get; }

            /// <summary>
            /// Gets the hot key description.
            /// </summary>
            public string? Description { get; }

            /// <summary>
            /// Gets the ID of the plugin which has registered this hot key.
            /// </summary>
            public PluginID PluginId { get; }

            /// <summary>
            /// Gets the key flags.
            /// </summary>
            public KeyFlags KeyFlags { get; }

            /// <summary>
            /// Gets the virtual key.
            /// </summary>
            public byte VirtualKey { get; }
        }
    }
}
