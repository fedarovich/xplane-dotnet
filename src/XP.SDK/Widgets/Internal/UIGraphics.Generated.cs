using InlineIL;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace XP.SDK.Widgets.Internal
{
    public static partial class UIGraphics
    {
        private static IntPtr DrawWindowPtr;
        private static IntPtr GetWindowDefaultDimensionsPtr;
        private static IntPtr DrawElementPtr;
        private static IntPtr GetElementDefaultDimensionsPtr;
        private static IntPtr DrawTrackPtr;
        private static IntPtr GetTrackDefaultDimensionsPtr;
        private static IntPtr GetTrackMetricsPtr;
        static UIGraphics()
        {
            const string libraryName = "Widgets";
            DrawWindowPtr = FunctionResolver.Resolve(libraryName, "XPDrawWindow");
            GetWindowDefaultDimensionsPtr = FunctionResolver.Resolve(libraryName, "XPGetWindowDefaultDimensions");
            DrawElementPtr = FunctionResolver.Resolve(libraryName, "XPDrawElement");
            GetElementDefaultDimensionsPtr = FunctionResolver.Resolve(libraryName, "XPGetElementDefaultDimensions");
            DrawTrackPtr = FunctionResolver.Resolve(libraryName, "XPDrawTrack");
            GetTrackDefaultDimensionsPtr = FunctionResolver.Resolve(libraryName, "XPGetTrackDefaultDimensions");
            GetTrackMetricsPtr = FunctionResolver.Resolve(libraryName, "XPGetTrackMetrics");
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void DrawWindow(int inX1, int inY1, int inX2, int inY2, WindowStyle inStyle)
        {
            IL.DeclareLocals(false);
            IL.Push(inX1);
            IL.Push(inY1);
            IL.Push(inX2);
            IL.Push(inY2);
            IL.Push(inStyle);
            IL.Push(DrawWindowPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int), typeof(int), typeof(int), typeof(int), typeof(WindowStyle)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetWindowDefaultDimensions(WindowStyle inStyle, int *outWidth, int *outHeight)
        {
            IL.DeclareLocals(false);
            IL.Push(inStyle);
            IL.Push(outWidth);
            IL.Push(outHeight);
            IL.Push(GetWindowDefaultDimensionsPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WindowStyle), typeof(int *), typeof(int *)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void DrawElement(int inX1, int inY1, int inX2, int inY2, ElementStyle inStyle, int inLit)
        {
            IL.DeclareLocals(false);
            IL.Push(inX1);
            IL.Push(inY1);
            IL.Push(inX2);
            IL.Push(inY2);
            IL.Push(inStyle);
            IL.Push(inLit);
            IL.Push(DrawElementPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int), typeof(int), typeof(int), typeof(int), typeof(ElementStyle), typeof(int)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetElementDefaultDimensions(ElementStyle inStyle, int *outWidth, int *outHeight, int *outCanBeLit)
        {
            IL.DeclareLocals(false);
            IL.Push(inStyle);
            IL.Push(outWidth);
            IL.Push(outHeight);
            IL.Push(outCanBeLit);
            IL.Push(GetElementDefaultDimensionsPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(ElementStyle), typeof(int *), typeof(int *), typeof(int *)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void DrawTrack(int inX1, int inY1, int inX2, int inY2, int inMin, int inMax, int inValue, TrackStyle inTrackStyle, int inLit)
        {
            IL.DeclareLocals(false);
            IL.Push(inX1);
            IL.Push(inY1);
            IL.Push(inX2);
            IL.Push(inY2);
            IL.Push(inMin);
            IL.Push(inMax);
            IL.Push(inValue);
            IL.Push(inTrackStyle);
            IL.Push(inLit);
            IL.Push(DrawTrackPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(TrackStyle), typeof(int)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetTrackDefaultDimensions(TrackStyle inStyle, int *outWidth, int *outCanBeLit)
        {
            IL.DeclareLocals(false);
            IL.Push(inStyle);
            IL.Push(outWidth);
            IL.Push(outCanBeLit);
            IL.Push(GetTrackDefaultDimensionsPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(TrackStyle), typeof(int *), typeof(int *)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetTrackMetrics(int inX1, int inY1, int inX2, int inY2, int inMin, int inMax, int inValue, TrackStyle inTrackStyle, int *outIsVertical, int *outDownBtnSize, int *outDownPageSize, int *outThumbSize, int *outUpPageSize, int *outUpBtnSize)
        {
            IL.DeclareLocals(false);
            IL.Push(inX1);
            IL.Push(inY1);
            IL.Push(inX2);
            IL.Push(inY2);
            IL.Push(inMin);
            IL.Push(inMax);
            IL.Push(inValue);
            IL.Push(inTrackStyle);
            IL.Push(outIsVertical);
            IL.Push(outDownBtnSize);
            IL.Push(outDownPageSize);
            IL.Push(outThumbSize);
            IL.Push(outUpPageSize);
            IL.Push(outUpBtnSize);
            IL.Push(GetTrackMetricsPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(TrackStyle), typeof(int *), typeof(int *), typeof(int *), typeof(int *), typeof(int *), typeof(int *)));
        }
    }
}