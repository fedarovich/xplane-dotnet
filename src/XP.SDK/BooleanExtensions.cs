using System.Runtime.CompilerServices;
using InlineIL;

namespace XP.SDK
{
    internal static class BooleanExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        internal static int ToInt(this bool value)
        {
            IL.Emit.Ldarg_0();
            IL.Emit.Ldc_I4_0();
            IL.Emit.Cgt_Un();
            return IL.Return<int>();
        }
    }
}
