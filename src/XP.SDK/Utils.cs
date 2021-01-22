using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Unicode;

#nullable enable

namespace XP.SDK
{
    public static partial class Utils
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe T? TryGetObject<T>(void* refcon) where T : class
        {
            return refcon != null
                ? GCHandle.FromIntPtr(new IntPtr(refcon)).Target as T
                : null;
        }
    }
}
