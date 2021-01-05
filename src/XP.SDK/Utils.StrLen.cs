using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace XP.SDK
{
    internal static partial class Utils
    {
        internal static unsafe nuint StrLen(byte* str)
        {
            if (Avx2.IsSupported)
            {
                Vector256<byte> zero = Vector256<byte>.Zero;
                byte* aligned = (byte*)((nint)str & ~0x1FL);
                byte misbits = (byte)((nint)str & 0x1FL);

                Vector256<byte> vec256 = Avx.LoadAlignedVector256(aligned);
                Vector256<byte> bytemask = Avx2.CompareEqual(vec256, zero);
                int bitmask = Avx2.MoveMask(bytemask);
                bitmask = (bitmask >> misbits) << misbits;

                while (bitmask == 0)
                {
                    aligned += 32;
                    vec256 = Avx.LoadAlignedVector256(aligned);
                    bytemask = Avx2.CompareEqual(vec256, zero);
                    bitmask = Avx2.MoveMask(bytemask);
                }

                var delta = (nuint) (aligned - str);
                if (Bmi1.IsSupported)
                {
                    return delta + Bmi1.TrailingZeroCount((uint) bitmask);
                }
                else
                {
                    return delta + TrailingZeroCount((uint)bitmask);
                }
            }
            else if (Sse2.IsSupported)
            {
                Vector128<byte> zero = Vector128<byte>.Zero;
                byte* aligned = (byte*)((nint)str & ~0x0FL);
                byte misbits = (byte)((nint)str & 0x0FL);

                Vector128<byte> vec128 = Sse2.LoadAlignedVector128(aligned);
                Vector128<byte> bytemask = Sse2.CompareEqual(vec128, zero);
                int bitmask = Sse2.MoveMask(bytemask);
                bitmask = (bitmask >> misbits) << misbits;

                while (bitmask == 0)
                {
                    aligned += 16;
                    vec128 = Sse2.LoadAlignedVector128(aligned);
                    bytemask = Sse2.CompareEqual(vec128, zero);
                    bitmask = Sse2.MoveMask(bytemask);
                }

                var delta = (nuint)(aligned - str);
                if (Bmi1.IsSupported)
                {
                    return delta + Bmi1.TrailingZeroCount((uint) bitmask);
                }
                else
                {
                    return delta + TrailingZeroCount((uint) bitmask);
                }
            }
            else
            {
                // Based on glibc implementation.

                byte* char_ptr;
                ulong* longword_ptr;
                ulong longword, himagic, lomagic;

                /* Handle the first few characters by reading one character at a time.
                   Do this until CHAR_PTR is aligned on a longword boundary.  */
                for (char_ptr = str; ((ulong)char_ptr & (sizeof(ulong) - 1)) != 0; ++char_ptr)
                    if (*char_ptr == 0)
                        return (nuint)(char_ptr - str);

                longword_ptr = (ulong*)char_ptr;

                /* Bits 31, 24, 16, and 8 of this number are zero.  Call these bits
                 the "holes."  Note that there is a hole just to the left of
                 each byte, with an extra at the end:
                 bits:  01111110 11111110 11111110 11111111
                 bytes: AAAAAAAA BBBBBBBB CCCCCCCC DDDDDDDD
                 The 1-bits make sure that carries propagate to the next 0-bit.
                 The 0-bits provide holes for carries to fall into.  */
                himagic = 0x80808080_80808080L;
                lomagic = 0x01010101_01010101L;

                for (; ; )
                {
                    longword = *longword_ptr++;

                    if (((longword - lomagic) & ~longword & himagic) != 0)
                    {
                        /* Which of the bytes was the zero?  If none of them were, it was
                           a misfire; continue the search.  */

                        byte* cp = (byte*)(longword_ptr - 1);

                        if (cp[0] == 0)
                            return (nuint)(cp - str);
                        if (cp[1] == 0)
                            return (nuint)(cp - str + 1);
                        if (cp[2] == 0)
                            return (nuint)(cp - str + 2);
                        if (cp[3] == 0)
                            return (nuint)(cp - str + 3);
                        if (cp[4] == 0)
                            return (nuint)(cp - str + 4);
                        if (cp[5] == 0)
                            return (nuint)(cp - str + 5);
                        if (cp[6] == 0)
                            return (nuint)(cp - str + 6);
                        if (cp[7] == 0)
                            return (nuint)(cp - str + 7);
                    }
                }
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private static unsafe uint TrailingZeroCount(uint x)
        {
            byte* bytes = stackalloc byte[]
            {
                32, 0, 1, 26, 2, 23, 27, 0, 3, 16, 24, 30, 28, 11, 0, 13, 4, 7, 17, 0, 25, 22, 31, 15, 29, 10, 12, 6, 0,
                21, 14, 9, 5, 20, 8, 19, 18
            };

            return bytes[(uint)(-x & x) % 37];
        }
    }
}
