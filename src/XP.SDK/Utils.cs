using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Unicode;

#nullable enable

namespace XP.SDK
{
    internal static partial class Utils
    {
        internal static readonly UTF8Encoding UTF8WithoutPreamble = new UTF8Encoding(false);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref byte ToUtf8(ReadOnlySpan<char> utf16, Span<byte> utf8)
        {
            Utf8.FromUtf16(utf16, utf8, out _, out int count);
            utf8[count] = 0;
            return ref utf8.GetPinnableReference();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref byte ToUtf8(ReadOnlySpan<char> utf16, Span<byte> utf8, out int count)
        {
            Utf8.FromUtf16(utf16, utf8, out _, out count);
            utf8[count] = 0;
            return ref utf8.GetPinnableReference();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe byte* ToUtf8Unsafe(ReadOnlySpan<char> utf16, Span<byte> utf8)
        {
            Utf8.FromUtf16(utf16, utf8, out _, out int count);
            utf8[count] = 0;
            return (byte*)Unsafe.AsPointer(ref utf8.GetPinnableReference());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe byte* ToUtf8Unsafe(ReadOnlySpan<char> utf16, Span<byte> utf8, out int count)
        {
            Utf8.FromUtf16(utf16, utf8, out _, out count);
            utf8[count] = 0;
            return (byte*)Unsafe.AsPointer(ref utf8.GetPinnableReference());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe T? TryGetObject<T>(void* refcon) where T : class
        {
            return refcon != null
                ? GCHandle.FromIntPtr(new IntPtr(refcon)).Target as T
                : null;
        }
    }
}
