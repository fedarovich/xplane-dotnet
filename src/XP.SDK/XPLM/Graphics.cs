using System;
using System.Runtime.CompilerServices;
using System.Text.Unicode;
using XP.SDK.XPLM.Internal;

namespace XP.SDK.XPLM
{
    public static class Graphics
    {
        /// <summary>
        /// Changes OpenGL’s fixed function pipeline state.
        /// You are not responsible for restoring any state that is accessed via SetGraphicsState,
        /// but you are responsible for not accessing this state directly.
        /// </summary>
        /// <param name="enableFog">Enables or disables fog, equivalent to: glEnable(GL_FOG).</param>
        /// <param name="numberTexUnits">
        /// Enables or disables a number of multitexturing units.
        /// If the number is 0, 2d texturing is disabled entirely, as in glDisable(GL_TEXTURE_2D);
        /// Otherwise, 2d texturing is enabled, and a number of multitexturing units are enabled sequentially,
        /// starting with unit 0, e.g. glActiveTextureARB(GL_TEXTURE0_ARB); glEnable (GL_TEXTURE_2D).
        /// </param>
        /// <param name="enableLighting">Enables or disables OpenGL lighting, e.g. glEnable(GL_LIGHTING); glEnable(GL_LIGHT0).</param>
        /// <param name="enableAlphaTesting">Enables or disables the alpha test per pixel, e.g. glEnable(GL_ALPHA_TEST).</param>
        /// <param name="enableAlphaBlending">Enables or disables alpha blending per pixel, e.g. glEnable(GL_BLEND).</param>
        /// <param name="enableDepthTesting">Enables per pixel depth testing, as in glEnable(GL_DEPTH_TEST).</param>
        /// <param name="enableDepthWriting">Enables writing back of depth information to the depth bufffer, as in glDepthMask(GL_TRUE).</param>
        /// <remarks>
        /// <para>
        /// The purpose of this function is to change OpenGL state while keeping
        /// X-Plane aware of the state changes; this keeps X-Plane from getting
        /// surprised by OGL state changes, and prevents X-Plane and plug-ins from
        /// having to set all state before all draws; SetGraphicsState internally
        /// skips calls to change state that is already properly enabled.
        /// </para>
        /// <para>
        /// X-Plane does not have a 'default' OGL state to plug-ins; plug-ins should
        /// totally set OGL state before drawing.  Use SetGraphicsState instead of
        /// any of the above OpenGL calls.
        /// </para>
        /// <para>
        /// WARNING: Any routine that performs drawing (e.g. DrawString or widget
        /// code) may change X-Plane's state.  Always set state before drawing after
        /// unknown code has executed.
        /// </para>
        /// <para>
        /// Deprecation Warnings: X-Plane’s lighting and fog environment is significantly more complex than the fixed function pipeline can express;
        /// do not assume that lighting and fog state is a good approximation for 3-d drawing.
        /// Prefer to use XPLMInstancing to draw objects.
        /// All calls to SetGraphicsState should have no fog or lighting.
        /// </para>
        /// </remarks>
        [Obsolete("Prefer SetState(bool, int, int, int, int) overload instead.")]
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void SetGraphicsState(
            bool enableFog,
            int numberTexUnits,
            bool enableLighting,
            bool enableAlphaTesting,
            bool enableAlphaBlending,
            bool enableDepthTesting,
            bool enableDepthWriting)
        {
            GraphicsAPI.SetGraphicsState(
                enableFog.ToInt(),
                numberTexUnits,
                enableLighting.ToInt(),
                enableAlphaTesting.ToInt(),
                enableAlphaBlending.ToInt(),
                enableDepthTesting.ToInt(),
                enableDepthWriting.ToInt());
        }

        /// <summary>
        /// Changes OpenGL’s fixed function pipeline state.
        /// You are not responsible for restoring any state that is accessed via SetGraphicsState,
        /// but you are responsible for not accessing this state directly.
        /// </summary>
        /// <param name="numberTexUnits">
        /// Enables or disables a number of multitexturing units.
        /// If the number is 0, 2d texturing is disabled entirely, as in glDisable(GL_TEXTURE_2D);
        /// Otherwise, 2d texturing is enabled, and a number of multitexturing units are enabled sequentially,
        /// starting with unit 0, e.g. glActiveTextureARB(GL_TEXTURE0_ARB); glEnable (GL_TEXTURE_2D).
        /// </param>
        /// <param name="enableAlphaTesting">Enables or disables the alpha test per pixel, e.g. glEnable(GL_ALPHA_TEST).</param>
        /// <param name="enableAlphaBlending">Enables or disables alpha blending per pixel, e.g. glEnable(GL_BLEND).</param>
        /// <param name="enableDepthTesting">Enables per pixel depth testing, as in glEnable(GL_DEPTH_TEST).</param>
        /// <param name="enableDepthWriting">Enables writing back of depth information to the depth bufffer, as in glDepthMask(GL_TRUE).</param>
        /// <remarks>
        /// <para>
        /// The purpose of this function is to change OpenGL state while keeping
        /// X-Plane aware of the state changes; this keeps X-Plane from getting
        /// surprised by OGL state changes, and prevents X-Plane and plug-ins from
        /// having to set all state before all draws; SetGraphicsState internally
        /// skips calls to change state that is already properly enabled.
        /// </para>
        /// <para>
        /// X-Plane does not have a 'default' OGL state to plug-ins; plug-ins should
        /// totally set OGL state before drawing.  Use SetGraphicsState instead of
        /// any of the above OpenGL calls.
        /// </para>
        /// <para>
        /// WARNING: Any routine that performs drawing (e.g. DrawString or widget
        /// code) may change X-Plane's state.  Always set state before drawing after
        /// unknown code has executed.
        /// </para>
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void SetGraphicsState(
            int numberTexUnits = 0,
            bool enableAlphaTesting = false,
            bool enableAlphaBlending = true,
            bool enableDepthTesting = true,
            bool enableDepthWriting = false)
        {
            GraphicsAPI.SetGraphicsState(
                0,
                numberTexUnits,
                0,
                enableAlphaTesting.ToInt(),
                enableAlphaBlending.ToInt(),
                enableDepthTesting.ToInt(),
                enableDepthWriting.ToInt());
        }

        /// <summary>
        /// Changes what texture is bound to the 2d texturing target.
        /// This routine caches the current 2d texture across all texturing units in the sim and plug-ins, preventing extraneous binding.
        /// For example, consider several plug-ins running in series;
        /// if they all use the ‘general interface’ bitmap to do UI,
        /// calling this function will skip the rebinding of the general interface texture on all but the first plug-in,
        /// which can provide better frame rate son some graphics cards.
        /// </summary>
        /// <param name="textureId">ID of the texture object to bind.</param>
        /// <param name="textureUnit">
        /// A zero-based texture unit (e.g. 0 for the first one), up to a maximum of 4 units.
        /// (This number may increase in future versions of X-Plane.)
        /// </param>
        /// <remarks>
        /// Use this routine instead of glBindTexture(GL_TEXTURE_2D, ….).
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void BindTexture2d(int textureId, int textureUnit)
        {
            GraphicsAPI.BindTexture2d(textureId, textureUnit);
        }

        /// <summary>
        /// Use this routine instead of glGenTextures to generate new texture object IDs.
        /// This routine historically ensured that plugins don’t use texure IDs that X-Plane is reserving for its own use.
        /// </summary>
        /// <param name="textureIds"></param>
        public static unsafe void GenerateTextureNumbers(in Span<int> textureIds)
        {
            fixed (int* textureIdsPtr = textureIds)
            {
                GraphicsAPI.GenerateTextureNumbers(textureIdsPtr, textureIds.Length);
            }
        }

        /// <summary>
        /// Translates coordinates from latitude, longitude, and altitude to local scene coordinates.
        /// Latitude and longitude are in decimal degrees, and altitude is in meters MSL (mean sea level).
        /// The XYZ coordinates are in meters in the local OpenGL coordinate system.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static unsafe (double x, double y, double z) WorldToLocal(double latitude, double longitude, double altitude)
        {
            (double x, double y , double z) result = default;
            GraphicsAPI.WorldToLocal(latitude, longitude, altitude, &result.x, &result.y, &result.z);
            return result;
        }

        /// <summary>
        /// This routine translates a local coordinate triplet back into latitude, longitude, and altitude.
        /// Latitude and longitude are in decimal degrees, and altitude is in meters MSL (mean sea level).
        /// The XYZ coordinates are in meters in the local OpenGL coordinate system.
        /// </summary>
        /// <remarks>
        /// NOTE: world coordinates are less precise than local coordinates; you should try to avoid round tripping from local to world and back.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static unsafe (double latitude, double longitude, double altitude) LocalToWorld(double x, double y, double z)
        {
            (double lat, double lon, double alt) result = default;
            GraphicsAPI.LocalToWorld(x, y, z, &result.lat, &result.lon, &result.alt);
            return result;
        }

        /// <summary>
        /// Draws a translucent dark box, partially obscuring parts of the screen but making text easy to read.
        /// This is the same graphics primitive used by X-Plane to show text files and ATC info.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawTranslucentDarkBox(int left, int top, int right, int bottom)
        {
            GraphicsAPI.DrawTranslucentDarkBox(left, top, right, bottom);
        }

        /// <summary>
        /// This routine draws a NULL terminated string in a given font.
        /// Pass in the lower left pixel that the character is to be drawn onto.
        /// Also pass the character and font ID.
        /// This function returns the x offset plus the width of all drawn characters.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static unsafe void DrawString(in RGBColor color, 
            int xOffset, int yOffset, 
            in ReadOnlySpan<char> text,
            ref int wordWrapWidth, 
            FontID fontId)
        {
            fixed (int* inWordWrapWidth = &wordWrapWidth)
            {
                GraphicsAPI.DrawString(color, xOffset, yOffset, text, inWordWrapWidth, fontId);
            }
        }

        /// <summary>
        /// This routine draws a NULL terminated string in a given font.
        /// Pass in the lower left pixel that the character is to be drawn onto.
        /// Also pass the character and font ID.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void DrawString(in RGBColor color,
            int xOffset, int yOffset,
            in ReadOnlySpan<char> text,
            FontID fontId)
        {
            GraphicsAPI.DrawString(color, xOffset, yOffset, text, null, fontId);
        }

        /// <summary>
        /// This routine draws a number similar to the digit editing fields in PlaneMaker and data output display in X-Plane.
        /// Pass in a color, a position, a floating point value, and formatting info.
        /// Specify how many integer and how many decimal digits to show and whether to show a sign, as well as a character set.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static unsafe void DrawNumber(in RGBColor color,
            int xOffset, int yOffset,
            double value,
            int digits, int decimals, bool showSign,
            FontID fontId)
        {
            fixed (void* r = &color)
            {
                GraphicsAPI.DrawNumber((float*) r, xOffset, yOffset, value, digits, decimals, showSign.ToInt(), fontId);
            }
        }

        /// <summary>
        /// This routine returns the width and height of a character in a given font. It also tells you if the font only supports numeric digits. 
        /// </summary>
        /// <remarks>
        /// For a proportional font the width will be an arbitrary, hopefully average width.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static unsafe (int charWidth, int charHeight, bool digitsOnly) GetFontDimensions(FontID fontId)
        {
            int charWidth, charHeight, digits;
            GraphicsAPI.GetFontDimensions(fontId, &charWidth, &charHeight, &digits);
            return (charWidth, charHeight, digits != 0);
        }

        /// <summary>
        /// Returns the width in pixels of a string using a given font.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static unsafe float MeasureString(FontID fontId, in ReadOnlySpan<char> @string)
        {
            Span<byte> bytes = stackalloc byte[@string.Length << 1];
            Utf8.FromUtf16(@string, bytes, out _, out var length);
            return GraphicsAPI.MeasureString(fontId, (byte*) Unsafe.AsPointer(ref bytes.GetPinnableReference()), length);
        }

        /// <summary>
        /// Returns the width in pixels of a UTF8 string using a given font.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static unsafe float MeasureString(FontID fontId, in ReadOnlySpan<byte> utf8string)
        {
            fixed (byte* ptr = utf8string)
            {
                return GraphicsAPI.MeasureString(fontId, ptr, utf8string.Length);
            }
        }
    }
}
