using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace XP.SDK
{
    static partial class Utils
    {
        /// <summary>
        /// Gets the length on null-terminated string. This function is equivalent to <c>strlen_s</c> C function.
        /// </summary>
        /// <param name="str">The pointer to the string start.</param>
        /// <returns>
        /// <para>The length of the null-terminated byte string <paramref name="str"/>.</para>
        /// <para><c>0</c> if <paramref name="str"/> is <see langword="null"/>.</para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static unsafe nuint CStringLength(byte* str)
        {
            if (str == null)
                return 0;
            
            if (Avx2.IsSupported)
            {
                Vector256<byte> zero = Vector256<byte>.Zero;
                byte* aligned = (byte*)((nint)str & ~0x1FL);
                byte misBits = (byte)((nint)str & 0x1FL);

                Vector256<byte> vec256 = Avx.LoadAlignedVector256(aligned);
                Vector256<byte> byteMask = Avx2.CompareEqual(vec256, zero);
                int bitMask = Avx2.MoveMask(byteMask);
                bitMask = (bitMask >> misBits) << misBits;

                while (bitMask == 0)
                {
                    aligned += 32;
                    vec256 = Avx.LoadAlignedVector256(aligned);
                    byteMask = Avx2.CompareEqual(vec256, zero);
                    bitMask = Avx2.MoveMask(byteMask);
                }

                return (nuint)((aligned - str) + BitOperations.TrailingZeroCount(bitMask));
            }
            else if (Sse2.IsSupported)
            {
                Vector128<byte> zero = Vector128<byte>.Zero;
                byte* aligned = (byte*)((nint)str & ~0x0FL);
                byte misBits = (byte)((nint)str & 0x0FL);

                Vector128<byte> vec128 = Sse2.LoadAlignedVector128(aligned);
                Vector128<byte> byteMask = Sse2.CompareEqual(vec128, zero);
                int bitMask = Sse2.MoveMask(byteMask);
                bitMask = (bitMask >> misBits) << misBits;

                while (bitMask == 0)
                {
                    aligned += 16;
                    vec128 = Sse2.LoadAlignedVector128(aligned);
                    byteMask = Sse2.CompareEqual(vec128, zero);
                    bitMask = Sse2.MoveMask(byteMask);
                }

                return (nuint)((aligned - str) + BitOperations.TrailingZeroCount(bitMask));
            }
            else
            {
                // Based on glibc implementation.
                // Optimized for 64-bit processors.

                byte* charPtr;
                ulong* ulongPtr;
                ulong ulongValue, hiMagic, loMagic;

                // Handle the first few characters by reading one character at a time.
                // Do this until charPtr is aligned on a ulong boundary.
                for (charPtr = str; ((ulong)charPtr & (sizeof(ulong) - 1)) != 0; ++charPtr)
                { 
                    if (*charPtr == 0)
                        return (nuint)(charPtr - str);
                }

                ulongPtr = (ulong*)charPtr;

                // Bits 31, 24, 16, and 8 of this number are zero.  Call these bits
                // the "holes."  Note that there is a hole just to the left of
                // each byte, with an extra at the end:
                //
                // bits:  01111110 11111110 11111110 11111111
                // bytes: AAAAAAAA BBBBBBBB CCCCCCCC DDDDDDDD
                //
                // The 1-bits make sure that carries propagate to the next 0-bit.
                // The 0-bits provide holes for carries to fall into.
                hiMagic = 0x80808080_80808080L;
                loMagic = 0x01010101_01010101L;

                while (true)
                {
                    ulongValue = *ulongPtr++;

                    if (((ulongValue - loMagic) & ~ulongValue & hiMagic) != 0)
                    { 
                        // Which of the bytes was the zero?  If none of them were, it was a misfire; continue the search. 

                        byte* cp = (byte*)(ulongPtr - 1);

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
    }
}
