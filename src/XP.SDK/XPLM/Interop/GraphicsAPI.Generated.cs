using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using InlineIL;

namespace XP.SDK.XPLM.Interop
{
    public static partial class GraphicsAPI
    {
        
        /// <summary>
        /// <para>
        /// XPLMSetGraphicsState changes OpenGL's fixed function pipeline state.  You
        /// are not responsible for restoring any state that is accessed via
        /// XPLMSetGraphicsState, but you are responsible for not accessing this state
        /// directly.
        /// </para>
        /// <para>
        /// - inEnableFog - enables or disables fog, equivalent to: glEnable(GL_FOG);
        /// - inNumberTexUnits - enables or disables a number of multitexturing units.
        /// If the number is 0, 2d texturing is disabled entirely, as in
        /// glDisable(GL_TEXTURE_2D);  Otherwise, 2d texturing is enabled, and a
        /// number of multitexturing units are enabled sequentially, starting with
        /// unit 0, e.g. glActiveTextureARB(GL_TEXTURE0_ARB); glEnable
        /// (GL_TEXTURE_2D);
        /// - inEnableLighting - enables or disables OpenGL lighting, e.g.
        /// glEnable(GL_LIGHTING); glEnable(GL_LIGHT0);
        /// - inEnableAlphaTesting - enables or disables the alpha test per pixel, e.g.
        /// glEnable(GL_ALPHA_TEST);
        /// - inEnableAlphaBlending - enables or disables alpha blending per pixel,
        /// e.g. glEnable(GL_BLEND);
        /// - inEnableDepthTesting - enables per pixel depth testing, as in
        /// glEnable(GL_DEPTH_TEST);
        /// - inEnableDepthWriting - enables writing back of depth information to the
        /// depth bufffer, as in glDepthMask(GL_TRUE);
        /// </para>
        /// <para>
        /// The purpose of this function is to change OpenGL state while keeping
        /// X-Plane aware of the state changes; this keeps X-Plane from getting
        /// surprised by OGL state changes, and prevents X-Plane and plug-ins from
        /// having to set all state before all draws; XPLMSetGraphicsState internally
        /// skips calls to change state that is already properly enabled.
        /// </para>
        /// <para>
        /// X-Plane does not have a 'default' OGL state for plug-ins with respect to
        /// the above state vector; plug-ins should totally set OGL state using this
        /// API before drawing.  Use XPLMSetGraphicsState instead of any of the above
        /// OpenGL calls.
        /// </para>
        /// <para>
        /// WARNING: Any routine that performs drawing (e.g. XPLMDrawString or widget
        /// code) may change X-Plane's state.  Always set state before drawing after
        /// unknown code has executed.
        /// </para>
        /// <para>
        /// *Deprecation Warnings*: X-Plane's lighting and fog environemnt is
        /// significantly more complex than the fixed function pipeline can express;
        /// do not assume that lighting and fog state is a good approximation for 3-d
        /// drawing.  Prefer to use XPLMInstancing to draw objects.  All calls to
        /// XPLMSetGraphicsState should have no fog or lighting.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMSetGraphicsState", ExactSpelling = true)]
        public static extern void SetGraphicsState(int inEnableFog, int inNumberTexUnits, int inEnableLighting, int inEnableAlphaTesting, int inEnableAlphaBlending, int inEnableDepthTesting, int inEnableDepthWriting);

        
        /// <summary>
        /// <para>
        /// XPLMBindTexture2d changes what texture is bound to the 2d texturing
        /// target. This routine caches the current 2d texture across all texturing
        /// units in the sim and plug-ins, preventing extraneous binding.  For
        /// example, consider several plug-ins running in series; if they all use the
        /// 'general interface' bitmap to do UI, calling this function will skip the
        /// rebinding of the general interface texture on all but the first plug-in,
        /// which can provide better frame rate son some graphics cards.
        /// </para>
        /// <para>
        /// inTextureID is the ID of the texture object to bind; inTextureUnit is a
        /// zero-based texture unit (e.g. 0 for the first one), up to a maximum of 4
        /// units.  (This number may increase in future versions of X-Plane.)
        /// </para>
        /// <para>
        /// Use this routine instead of glBindTexture(GL_TEXTURE_2D, ....);
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMBindTexture2d", ExactSpelling = true)]
        public static extern void BindTexture2d(int inTextureNum, int inTextureUnit);

        
        /// <summary>
        /// <para>
        /// Use this routine instead of glGenTextures to generate new texture object
        /// IDs. This routine historically ensured that plugins don't use texure IDs
        /// that X-Plane is reserving for its own use.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMGenerateTextureNumbers", ExactSpelling = true)]
        public static extern unsafe void GenerateTextureNumbers(int* outTextureIDs, int inCount);

        
        /// <summary>
        /// <para>
        /// This routine translates coordinates from latitude, longitude, and altitude
        /// to local scene coordinates. Latitude and longitude are in decimal degrees,
        /// and altitude is in meters MSL (mean sea level).  The XYZ coordinates are in
        /// meters in the local OpenGL coordinate system.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMWorldToLocal", ExactSpelling = true)]
        public static extern unsafe void WorldToLocal(double inLatitude, double inLongitude, double inAltitude, double* outX, double* outY, double* outZ);

        
        /// <summary>
        /// <para>
        /// This routine translates a local coordinate triplet back into latitude,
        /// longitude, and altitude.  Latitude and longitude are in decimal degrees,
        /// and altitude is in meters MSL (mean sea level).  The XYZ coordinates are in
        /// meters in the local OpenGL coordinate system.
        /// </para>
        /// <para>
        /// NOTE: world coordinates are less precise than local coordinates; you should
        /// try to avoid round tripping from local to world and back.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMLocalToWorld", ExactSpelling = true)]
        public static extern unsafe void LocalToWorld(double inX, double inY, double inZ, double* outLatitude, double* outLongitude, double* outAltitude);

        
        /// <summary>
        /// <para>
        /// This routine draws a translucent dark box, partially obscuring parts of the
        /// screen but making text easy to read.  This is the same graphics primitive
        /// used by X-Plane to show text files and ATC info.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMDrawTranslucentDarkBox", ExactSpelling = true)]
        public static extern void DrawTranslucentDarkBox(int inLeft, int inTop, int inRight, int inBottom);

        
        /// <summary>
        /// <para>
        /// This routine draws a NULL termianted string in a given font.  Pass in the
        /// lower left pixel that the character is to be drawn onto.  Also pass the
        /// character and font ID. This function returns the x offset plus the width of
        /// all drawn characters. The color to draw in is specified as a pointer to an
        /// array of three floating point colors, representing RGB intensities from 0.0
        /// to 1.0.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMDrawString", ExactSpelling = true)]
        public static extern unsafe void DrawString(float* inColorRGB, int inXOffset, int inYOffset, byte* inChar, int* inWordWrapWidth, FontID inFontID);

        
        /// <summary>
        /// <para>
        /// This routine draws a number similar to the digit editing fields in
        /// PlaneMaker and data output display in X-Plane.  Pass in a color, a
        /// position, a floating point value, and formatting info.  Specify how many
        /// integer and how many decimal digits to show and whether to show a sign, as
        /// well as a character set. This routine returns the xOffset plus width of the
        /// string drawn.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMDrawNumber", ExactSpelling = true)]
        public static extern unsafe void DrawNumber(float* inColorRGB, int inXOffset, int inYOffset, double inValue, int inDigits, int inDecimals, int inShowSign, FontID inFontID);

        
        /// <summary>
        /// <para>
        /// This routine returns the width and height of a character in a given font.
        /// It also tells you if the font only supports numeric digits.  Pass NULL if
        /// you don't need a given field.  Note that for a proportional font the width
        /// will be an arbitrary, hopefully average width.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMGetFontDimensions", ExactSpelling = true)]
        public static extern unsafe void GetFontDimensions(FontID inFontID, int* outCharWidth, int* outCharHeight, int* outDigitsOnly);

        
        /// <summary>
        /// <para>
        /// This routine returns the width in pixels of a string using a given font.
        /// The string is passed as a pointer plus length (and does not need to be null
        /// terminated); this is used to allow for measuring substrings. The return
        /// value is floating point; it is possible that future font drawing may allow
        /// for fractional pixels.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMMeasureString", ExactSpelling = true)]
        public static extern unsafe float MeasureString(FontID inFontID, byte* inChar, int inNumChars);

        
        /// <summary>
        /// <para>
        /// This routine returns the width in pixels of a string using a given font.
        /// The string is passed as a pointer plus length (and does not need to be null
        /// terminated); this is used to allow for measuring substrings. The return
        /// value is floating point; it is possible that future font drawing may allow
        /// for fractional pixels.
        /// </para>
        /// </summary>
        [SkipLocalsInitAttribute]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe float MeasureString(FontID inFontID, in ReadOnlySpan<char> inChar, int inNumChars)
        {
            Span<byte> inCharUtf8 = stackalloc byte[(inChar.Length << 1) | 1];
            var inCharPtr = Utils.ToUtf8Unsafe(inChar, inCharUtf8);
            return MeasureString(inFontID, inCharPtr, inNumChars);
        }

        
        /// <summary>
        /// <para>
        /// This routine returns the width in pixels of a string using a given font.
        /// The string is passed as a pointer plus length (and does not need to be null
        /// terminated); this is used to allow for measuring substrings. The return
        /// value is floating point; it is possible that future font drawing may allow
        /// for fractional pixels.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe float MeasureString(FontID inFontID, in XP.SDK.Utf8String inChar, int inNumChars)
        {
            fixed (byte* inCharPtr = inChar)
                return MeasureString(inFontID, inCharPtr, inNumChars);
        }
    }
}