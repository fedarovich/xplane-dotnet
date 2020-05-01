using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using InlineIL;

namespace XP.SDK.Widgets.Internal
{
    public static partial class UIGraphicsAPI
    {
        private static IntPtr DrawWindowPtr;
        private static IntPtr GetWindowDefaultDimensionsPtr;
        private static IntPtr DrawElementPtr;
        private static IntPtr GetElementDefaultDimensionsPtr;
        private static IntPtr DrawTrackPtr;
        private static IntPtr GetTrackDefaultDimensionsPtr;
        private static IntPtr GetTrackMetricsPtr;

        static UIGraphicsAPI()
        {
            DrawWindowPtr = Lib.GetExport("XPDrawWindow");
            GetWindowDefaultDimensionsPtr = Lib.GetExport("XPGetWindowDefaultDimensions");
            DrawElementPtr = Lib.GetExport("XPDrawElement");
            GetElementDefaultDimensionsPtr = Lib.GetExport("XPGetElementDefaultDimensions");
            DrawTrackPtr = Lib.GetExport("XPDrawTrack");
            GetTrackDefaultDimensionsPtr = Lib.GetExport("XPGetTrackDefaultDimensions");
            GetTrackMetricsPtr = Lib.GetExport("XPGetTrackMetrics");
        }

        
        /// <summary>
        /// <para>
        /// This routine draws a window of the given dimensions at the given offset on
        /// the virtual screen in a given style. The window is automatically scaled as
        /// appropriate using a bitmap scaling technique (scaling or repeating) as
        /// appropriate to the style.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void DrawWindow(int inX1, int inY1, int inX2, int inY2, WindowStyle inStyle)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(DrawWindowPtr);
            IL.Push(inX1);
            IL.Push(inY1);
            IL.Push(inX2);
            IL.Push(inY2);
            IL.Push(inStyle);
            IL.Push(DrawWindowPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int), typeof(int), typeof(int), typeof(int), typeof(WindowStyle)));
        }

        
        /// <summary>
        /// <para>
        /// This routine returns the default dimensions for a window. Output is either
        /// a minimum or fixed value depending on whether the window is scalable.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetWindowDefaultDimensions(WindowStyle inStyle, int* outWidth, int* outHeight)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(GetWindowDefaultDimensionsPtr);
            IL.Push(inStyle);
            IL.Push(outWidth);
            IL.Push(outHeight);
            IL.Push(GetWindowDefaultDimensionsPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WindowStyle), typeof(int*), typeof(int*)));
        }

        
        /// <summary>
        /// <para>
        /// XPDrawElement draws a given element at an offset on the virtual screen in
        /// set dimensions.
        /// *Even* if the element is not scalable, it will be scaled if the width and
        /// height do not match the preferred dimensions; it'll just look ugly. Pass
        /// inLit to see the lit version of the element; if the element cannot be lit
        /// this is ignored.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void DrawElement(int inX1, int inY1, int inX2, int inY2, ElementStyle inStyle, int inLit)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(DrawElementPtr);
            IL.Push(inX1);
            IL.Push(inY1);
            IL.Push(inX2);
            IL.Push(inY2);
            IL.Push(inStyle);
            IL.Push(inLit);
            IL.Push(DrawElementPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int), typeof(int), typeof(int), typeof(int), typeof(ElementStyle), typeof(int)));
        }

        
        /// <summary>
        /// <para>
        /// This routine returns the recommended or minimum dimensions of a given UI
        /// element. outCanBeLit tells whether the element has both a lit and unlit
        /// state. Pass `NULL` to not receive any of these parameters.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetElementDefaultDimensions(ElementStyle inStyle, int* outWidth, int* outHeight, int* outCanBeLit)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(GetElementDefaultDimensionsPtr);
            IL.Push(inStyle);
            IL.Push(outWidth);
            IL.Push(outHeight);
            IL.Push(outCanBeLit);
            IL.Push(GetElementDefaultDimensionsPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(ElementStyle), typeof(int*), typeof(int*), typeof(int*)));
        }

        
        /// <summary>
        /// <para>
        /// This routine draws a track. You pass in the track dimensions and size; the
        /// track picks the optimal orientation for these dimensions. Pass in the
        /// track's minimum current and maximum values; the indicator will be
        /// positioned appropriately. You can also specify whether the track is lit or
        /// not.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void DrawTrack(int inX1, int inY1, int inX2, int inY2, int inMin, int inMax, int inValue, TrackStyle inTrackStyle, int inLit)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(DrawTrackPtr);
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

        
        /// <summary>
        /// <para>
        /// This routine returns a track's default smaller dimension; all tracks are
        /// scalable in the larger dimension. It also returns whether a track can be
        /// lit.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetTrackDefaultDimensions(TrackStyle inStyle, int* outWidth, int* outCanBeLit)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(GetTrackDefaultDimensionsPtr);
            IL.Push(inStyle);
            IL.Push(outWidth);
            IL.Push(outCanBeLit);
            IL.Push(GetTrackDefaultDimensionsPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(TrackStyle), typeof(int*), typeof(int*)));
        }

        
        /// <summary>
        /// <para>
        /// This routine returns the metrics of a track. If you want to write UI code
        /// to manipulate a track, this routine helps you know where the mouse
        /// locations are. For most other elements, the rectangle the element is drawn
        /// in is enough information. However, the scrollbar drawing routine does some
        /// automatic placement; this routine lets you know where things ended up. You
        /// pass almost everything you would pass to the draw routine. You get out the
        /// orientation, and other useful stuff.
        /// </para>
        /// <para>
        /// Besides orientation, you get five dimensions for the five parts of a
        /// scrollbar, which are the down button, down area (area before the thumb),
        /// the thumb, and the up area and button. For horizontal scrollers, the left
        /// button decreases; for vertical scrollers, the top button decreases.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetTrackMetrics(int inX1, int inY1, int inX2, int inY2, int inMin, int inMax, int inValue, TrackStyle inTrackStyle, int* outIsVertical, int* outDownBtnSize, int* outDownPageSize, int* outThumbSize, int* outUpPageSize, int* outUpBtnSize)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(GetTrackMetricsPtr);
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
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(TrackStyle), typeof(int*), typeof(int*), typeof(int*), typeof(int*), typeof(int*), typeof(int*)));
        }
    }
}