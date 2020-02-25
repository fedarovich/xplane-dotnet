using InlineIL;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Internal
{
    public static partial class Graphics
    {
        private static IntPtr SetGraphicsStatePtr;
        private static IntPtr BindTexture2dPtr;
        private static IntPtr GenerateTextureNumbersPtr;
        private static IntPtr GetTexturePtr;
        private static IntPtr WorldToLocalPtr;
        private static IntPtr LocalToWorldPtr;
        private static IntPtr DrawTranslucentDarkBoxPtr;
        private static IntPtr DrawStringPtr;
        private static IntPtr DrawNumberPtr;
        private static IntPtr GetFontDimensionsPtr;
        private static IntPtr MeasureStringPtr;
        static Graphics()
        {
            const string libraryName = "XPLM";
            SetGraphicsStatePtr = FunctionResolver.Resolve(libraryName, "XPLMSetGraphicsState");
            BindTexture2dPtr = FunctionResolver.Resolve(libraryName, "XPLMBindTexture2d");
            GenerateTextureNumbersPtr = FunctionResolver.Resolve(libraryName, "XPLMGenerateTextureNumbers");
            GetTexturePtr = FunctionResolver.Resolve(libraryName, "XPLMGetTexture");
            WorldToLocalPtr = FunctionResolver.Resolve(libraryName, "XPLMWorldToLocal");
            LocalToWorldPtr = FunctionResolver.Resolve(libraryName, "XPLMLocalToWorld");
            DrawTranslucentDarkBoxPtr = FunctionResolver.Resolve(libraryName, "XPLMDrawTranslucentDarkBox");
            DrawStringPtr = FunctionResolver.Resolve(libraryName, "XPLMDrawString");
            DrawNumberPtr = FunctionResolver.Resolve(libraryName, "XPLMDrawNumber");
            GetFontDimensionsPtr = FunctionResolver.Resolve(libraryName, "XPLMGetFontDimensions");
            MeasureStringPtr = FunctionResolver.Resolve(libraryName, "XPLMMeasureString");
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetGraphicsState(int inEnableFog, int inNumberTexUnits, int inEnableLighting, int inEnableAlphaTesting, int inEnableAlphaBlending, int inEnableDepthTesting, int inEnableDepthWriting)
        {
            IL.DeclareLocals(false);
            IL.Push(inEnableFog);
            IL.Push(inNumberTexUnits);
            IL.Push(inEnableLighting);
            IL.Push(inEnableAlphaTesting);
            IL.Push(inEnableAlphaBlending);
            IL.Push(inEnableDepthTesting);
            IL.Push(inEnableDepthWriting);
            IL.Push(SetGraphicsStatePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void BindTexture2d(int inTextureNum, int inTextureUnit)
        {
            IL.DeclareLocals(false);
            IL.Push(inTextureNum);
            IL.Push(inTextureUnit);
            IL.Push(BindTexture2dPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int), typeof(int)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GenerateTextureNumbers(int *outTextureIDs, int inCount)
        {
            IL.DeclareLocals(false);
            IL.Push(outTextureIDs);
            IL.Push(inCount);
            IL.Push(GenerateTextureNumbersPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int *), typeof(int)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int GetTexture(TextureID inTexture)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inTexture);
            IL.Push(GetTexturePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(TextureID)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void WorldToLocal(double inLatitude, double inLongitude, double inAltitude, double *outX, double *outY, double *outZ)
        {
            IL.DeclareLocals(false);
            IL.Push(inLatitude);
            IL.Push(inLongitude);
            IL.Push(inAltitude);
            IL.Push(outX);
            IL.Push(outY);
            IL.Push(outZ);
            IL.Push(WorldToLocalPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(double), typeof(double), typeof(double), typeof(double *), typeof(double *), typeof(double *)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void LocalToWorld(double inX, double inY, double inZ, double *outLatitude, double *outLongitude, double *outAltitude)
        {
            IL.DeclareLocals(false);
            IL.Push(inX);
            IL.Push(inY);
            IL.Push(inZ);
            IL.Push(outLatitude);
            IL.Push(outLongitude);
            IL.Push(outAltitude);
            IL.Push(LocalToWorldPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(double), typeof(double), typeof(double), typeof(double *), typeof(double *), typeof(double *)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void DrawTranslucentDarkBox(int inLeft, int inTop, int inRight, int inBottom)
        {
            IL.DeclareLocals(false);
            IL.Push(inLeft);
            IL.Push(inTop);
            IL.Push(inRight);
            IL.Push(inBottom);
            IL.Push(DrawTranslucentDarkBoxPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int), typeof(int), typeof(int), typeof(int)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void DrawString(float *inColorRGB, int inXOffset, int inYOffset, byte *inChar, int *inWordWrapWidth, FontID inFontID)
        {
            IL.DeclareLocals(false);
            IL.Push(inColorRGB);
            IL.Push(inXOffset);
            IL.Push(inYOffset);
            IL.Push(inChar);
            IL.Push(inWordWrapWidth);
            IL.Push(inFontID);
            IL.Push(DrawStringPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(float *), typeof(int), typeof(int), typeof(byte *), typeof(int *), typeof(FontID)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void DrawNumber(float *inColorRGB, int inXOffset, int inYOffset, double inValue, int inDigits, int inDecimals, int inShowSign, FontID inFontID)
        {
            IL.DeclareLocals(false);
            IL.Push(inColorRGB);
            IL.Push(inXOffset);
            IL.Push(inYOffset);
            IL.Push(inValue);
            IL.Push(inDigits);
            IL.Push(inDecimals);
            IL.Push(inShowSign);
            IL.Push(inFontID);
            IL.Push(DrawNumberPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(float *), typeof(int), typeof(int), typeof(double), typeof(int), typeof(int), typeof(int), typeof(FontID)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetFontDimensions(FontID inFontID, int *outCharWidth, int *outCharHeight, int *outDigitsOnly)
        {
            IL.DeclareLocals(false);
            IL.Push(inFontID);
            IL.Push(outCharWidth);
            IL.Push(outCharHeight);
            IL.Push(outDigitsOnly);
            IL.Push(GetFontDimensionsPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(FontID), typeof(int *), typeof(int *), typeof(int *)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe float MeasureString(FontID inFontID, byte *inChar, int inNumChars)
        {
            IL.DeclareLocals(false);
            float result;
            IL.Push(inFontID);
            IL.Push(inChar);
            IL.Push(inNumChars);
            IL.Push(MeasureStringPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(float), typeof(FontID), typeof(byte *), typeof(int)));
            IL.Pop(out result);
            return result;
        }
    }
}