using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using XP.SDK.XPLM.Internal;

namespace XP.SDK.XPLM
{
    /// <summary>
    /// Key sniffer provides low-level keyboard handlers.
    /// Allows for intercepting keystrokes outside the normal rules of the user interface.
    /// </summary>
    public static class KeySniffer
    {
        /// <summary>
        /// Key sniffer callback.
        /// </summary>
        /// <returns>
        /// Return <see langword="true"/> to pass the key on to the next sniffer, the window manager,
        /// X-Plane, or whomever is down stream.  Return <see langword="false"/> to consume the key.
        /// </returns>
        public delegate bool Callback(byte @char, KeyFlags flags, byte virtualKey);

        private static readonly KeySnifferCallback _keySnifferCallback;

        static unsafe KeySniffer()
        {
            _keySnifferCallback = HandleKeySnifferCallback;

            static int HandleKeySnifferCallback(byte inchar, KeyFlags inflags, byte invirtualkey, void* inrefcon) =>
                (Utils.TryGetObject<Callback>(inrefcon)?.Invoke(inchar, inflags, invirtualkey) == true).ToInt();
        }

        /// <summary>
        /// <para>
        /// This routine registers a key sniffing callback.  You specify whether you
        /// want to sniff before the window system, or only sniff keys the window
        /// system does not consume.  You should ALMOST ALWAYS sniff non-control keys
        /// after the window system.  When the window system consumes a key, it is
        /// because the user has "focused" a window.  Consuming the key or taking
        /// action based on the key will produce very weird results.  
        /// </para>
        /// </summary>
        /// <returns>
        /// The <see cref="IDisposable"/> object, than you must use for unsubscription, if the subscription succeeds.
        /// <see langword="null"/> otherwise.
        /// </returns>
        public static unsafe IDisposable TryRegisterCallback(Callback callback, bool beforeWindows = false)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            GCHandle handle = GCHandle.Alloc(callback);
            int result = DisplayAPI.RegisterKeySniffer(
                _keySnifferCallback, 
                beforeWindows.ToInt(),
                GCHandle.ToIntPtr(handle).ToPointer());
            if (result == 1)
            {
                return new Subscription(handle, beforeWindows);
            }

            handle.Free();
            return null;
        }

        private sealed class Subscription : IDisposable
        {
            private readonly bool _beforeWindows;
            private GCHandle _handle;
            private int _disposed;

            public Subscription(GCHandle handle, bool beforeWindows)
            {
                _handle = handle;
                _beforeWindows = beforeWindows;
            }

            public unsafe void Dispose()
            {
                if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 0)
                {
                    DisplayAPI.UnregisterKeySniffer(
                        _keySnifferCallback, 
                        _beforeWindows.ToInt(), 
                        GCHandle.ToIntPtr(_handle).ToPointer());
                    _handle.Free();
                }
            }
        }
    }
}
