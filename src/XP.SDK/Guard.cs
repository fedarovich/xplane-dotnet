using System;
using System.Runtime.CompilerServices;

namespace XP.SDK
{
    internal static class Guard
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        internal static void NotNull(IntPtr functionPointer)
        {
            if (functionPointer == default)
                throw new EntryPointNotFoundException();
        }
    }
}
