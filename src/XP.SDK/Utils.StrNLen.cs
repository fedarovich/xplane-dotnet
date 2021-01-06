using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace XP.SDK
{
    static partial class Utils
    {
        /// <summary>
        /// Gets the length on null-terminated string. This function is equivalent to <c>strnlen_s</c> C function.
        /// </summary>
        /// <param name="str">The pointer to the string start.</param>
        /// <param name="maxLength">The maximal length to check.</param>
        /// <returns>
        /// <para>The length of the null-terminated byte string <paramref name="str"/>.</para>
        /// <para><c>0</c> if <paramref name="str"/> is <see langword="null"/>.</para>
        /// <para><c>maxLength</c> if the string does not contain null character in the first <paramref name="maxLength"/> bytes.</para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static unsafe nuint CStringLength(byte* str, nuint maxLength)
        {
            if (str == null || maxLength == 0)
                return 0;
            
            if (Avx2.IsSupported)
            {
                Vector256<byte> zero = Vector256<byte>.Zero;
                byte* aligned = (byte*)((nint)str & ~0x1FL);
                byte* end = str + maxLength;
                byte misBits = (byte)((nint)str & 0x1FL);

                Vector256<byte> vec256 = Avx.LoadAlignedVector256(aligned);
                Vector256<byte> byteMask = Avx2.CompareEqual(vec256, zero);
                int bitMask = Avx2.MoveMask(byteMask);
                bitMask = (bitMask >> misBits) << misBits;

                while (bitMask == 0)
                {
                    aligned += 32;
                    if (aligned >= end)
                        break;

                    vec256 = Avx.LoadAlignedVector256(aligned);
                    byteMask = Avx2.CompareEqual(vec256, zero);
                    bitMask = Avx2.MoveMask(byteMask);
                }

                return (nuint) Math.Min((aligned - str) + BitOperations.TrailingZeroCount(bitMask), (long) maxLength);
            }
            else if (Sse2.IsSupported)
            {
                Vector128<byte> zero = Vector128<byte>.Zero;
                byte* aligned = (byte*)((nint)str & ~0x0FL);
                byte* end = str + maxLength;
                byte misBits = (byte)((nint)str & 0x0FL);

                Vector128<byte> vec128 = Sse2.LoadAlignedVector128(aligned);
                Vector128<byte> byteMask = Sse2.CompareEqual(vec128, zero);
                int bitMask = Sse2.MoveMask(byteMask);
                bitMask = (bitMask >> misBits) << misBits;

                while (bitMask == 0)
                {
                    aligned += 16;
                    if (aligned >= end)
                        break;

                    vec128 = Sse2.LoadAlignedVector128(aligned);
                    byteMask = Sse2.CompareEqual(vec128, zero);
                    bitMask = Sse2.MoveMask(byteMask);
                }

                return (nuint)Math.Min((aligned - str) + BitOperations.TrailingZeroCount(bitMask), (long) maxLength);
            }
            else
            {
                // Based on glibc implementation.
                // Optimized for 64-bit processors.

                byte* endPtr = str + maxLength;
                
                byte* charPtr;
                ulong* ulongPtr;
                ulong ulongValue, hiMagic, loMagic;

                if (maxLength == 0)
                    return 0;

                if (endPtr < str)
                    endPtr = (byte*) ~0UL;

                // Handle the first few characters by reading one character at a time.
                // Do this until charPtr is aligned on a ulong boundary.
                for (charPtr = str; ((ulong) charPtr & 7) != 0; ++charPtr)
                {
                    if (*charPtr == 0)
                    {
                        if (charPtr > endPtr)
                            charPtr = endPtr;
                        return (nuint)(charPtr - str);
                    }
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

                // Instead of the traditional loop which tests each character,
                // we will test a ulong at a time.  The tricky part is testing
                // if *any of the eight* bytes in the ulong in question are zero.
                while (ulongPtr < (ulong*) endPtr)
                {
                   // We tentatively exit the loop if adding MAGIC_BITS to
                   // ulong fails to change any of the hole bits of ulong.
                   // 1) Is this safe?  Will it catch all the zero bytes?
                   // Suppose there is a byte with all zeros.  Any carry bits
                   // propagating from its left will fall into the hole at its
                   // least significant bit and stop.  Since there will be no
                   // carry from its most significant bit, the LSB of the
                   // byte to the left will be unchanged, and the zero will be
                   // detected.
                   // 2) Is this worthwhile?  Will it ignore everything except
                   // zero bytes?  Suppose every byte of ulong has a bit set
                   // somewhere.  There will be a carry into bit 8.  If bit 8
                   // is set, this will carry into bit 16.  If bit 8 is clear,
                   // one of bits 9-15 must be set, so there will be a carry
                   // into bit 16.  Similarly, there will be a carry into bit
                   // 24.  If one of bits 24-30 is set, there will be a carry
                   // into bit 31, so all of the hole bits will be changed.
                   // The one misfire occurs when bits 24-30 are clear and bit
                   // 31 is set; in this case, the hole at bit 31 is not
                   // changed.  If we had access to the processor carry flag,
                   // we could close this loophole by putting the fourth hole
                   // at bit 32!
                   // So it ignores everything except 128's, when they're aligned
                   // properly.

                    ulongValue = *ulongPtr++;

                    if (((ulongValue - loMagic) & hiMagic) != 0)
                    {
                        // Which of the bytes was the zero?  If none of them were, it was a misfire; continue the search.
                        byte* cp = (byte*)(ulongPtr - 1);

                        charPtr = cp;
                        if (cp[0] == 0)
                            break;
                        charPtr = cp + 1;
                        if (cp[1] == 0)
                            break;
                        charPtr = cp + 2;
                        if (cp[2] == 0)
                            break;
                        charPtr = cp + 3;
                        if (cp[3] == 0)
                            break;
                        charPtr = cp + 4;
                        if (cp[4] == 0)
                            break;
                        charPtr = cp + 5;
                        if (cp[5] == 0)
                            break;
                        charPtr = cp + 6;
                        if (cp[6] == 0)
                            break;
                        charPtr = cp + 7;
                        if (cp[7] == 0)
                            break;
                    }
                    
                    charPtr = endPtr;
                }

                if (charPtr > endPtr)
                    charPtr = endPtr;
                
                return (nuint)(charPtr - str);
            }
        }
    }
}
