using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using InlineIL;

namespace XP.SDK.XPLM.Internal
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
        public static unsafe void DrawString(in RGBColor inColorRGB, int inXOffset, int inYOffset, in ReadOnlySpan<char> inChar, int* inWordWrapWidth, FontID inFontID)
        {
            IL.DeclareLocals(false);
            Span<byte> inCharUtf8 = stackalloc byte[(inChar.Length << 1) | 1];
            var inCharPtr = Utils.ToUtf8Unsafe(inChar, inCharUtf8);
            fixed (void* color = &inColorRGB)
            {
                DrawString((float*) color, inXOffset, inYOffset, inCharPtr, inWordWrapWidth, inFontID);
            }
        }
    }
}
