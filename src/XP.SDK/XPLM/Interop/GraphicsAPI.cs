using System;
using System.Runtime.CompilerServices;

namespace XP.SDK.XPLM.Interop
{
    public static partial class GraphicsAPI
    {
        /// <summary>
        /// <para>
        /// This routine draws a NULL terminated string in a given font.  Pass in the
        /// lower left pixel that the character is to be drawn onto.  Also pass the
        /// character and font ID. This function returns the x offset plus the width of
        /// all drawn characters. 
        /// </para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void DrawString(in RGBColor inColorRGB, int inXOffset, int inYOffset, in Utf8String inString, int* inWordWrapWidth, FontID inFontID)
        {
            fixed (void* color = &inColorRGB)
            {
                fixed (byte* inChar = inString)
                {
                    DrawString((float*) color, inXOffset, inYOffset, inChar, inWordWrapWidth, inFontID);
                }
            }
        }


        /// <summary>
        /// <para>
        /// This routine draws a NULL terminated string in a given font.  Pass in the
        /// lower left pixel that the character is to be drawn onto.  Also pass the
        /// character and font ID. This function returns the x offset plus the width of
        /// all drawn characters. 
        /// </para>
        /// </summary>
        [SkipLocalsInit]
        public static unsafe void DrawString(in RGBColor inColorRGB, int inXOffset, int inYOffset, in ReadOnlySpan<char> inChar, int* inWordWrapWidth, FontID inFontID)
        {
            int inCharLength = inChar.Length * 3 + 4;
            Span<byte> inCharUtf8 = inCharLength <= 4096 ? stackalloc byte[inCharLength] : GC.AllocateUninitializedArray<byte>(inCharLength);
            var inCharUtf8Str = Utf8String.FromUtf16Unsafe(inChar, inCharUtf8);
            DrawString(inColorRGB, inXOffset, inYOffset, inCharUtf8Str, inWordWrapWidth, inFontID);
        }
    }
}
