using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

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
