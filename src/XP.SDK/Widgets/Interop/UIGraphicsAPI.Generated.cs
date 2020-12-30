using System;
using System.Runtime.InteropServices;
using InlineIL;

namespace XP.SDK.Widgets.Interop
{
    public static partial class UIGraphicsAPI
    {
        
        /// <summary>
        /// <para>
        /// This routine draws a window of the given dimensions at the given offset on
        /// the virtual screen in a given style. The window is automatically scaled as
        /// appropriate using a bitmap scaling technique (scaling or repeating) as
        /// appropriate to the style.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPDrawWindow", ExactSpelling = true)]
        public static extern void DrawWindow(int inX1, int inY1, int inX2, int inY2, WindowStyle inStyle);

        
        /// <summary>
        /// <para>
        /// This routine returns the default dimensions for a window. Output is either
        /// a minimum or fixed value depending on whether the window is scalable.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPGetWindowDefaultDimensions", ExactSpelling = true)]
        public static extern unsafe void GetWindowDefaultDimensions(WindowStyle inStyle, int* outWidth, int* outHeight);

        
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
        [DllImportAttribute(Lib.Name, EntryPoint = "XPDrawElement", ExactSpelling = true)]
        public static extern void DrawElement(int inX1, int inY1, int inX2, int inY2, ElementStyle inStyle, int inLit);

        
        /// <summary>
        /// <para>
        /// This routine returns the recommended or minimum dimensions of a given UI
        /// element. outCanBeLit tells whether the element has both a lit and unlit
        /// state. Pass `NULL` to not receive any of these parameters.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPGetElementDefaultDimensions", ExactSpelling = true)]
        public static extern unsafe void GetElementDefaultDimensions(ElementStyle inStyle, int* outWidth, int* outHeight, int* outCanBeLit);

        
        /// <summary>
        /// <para>
        /// This routine draws a track. You pass in the track dimensions and size; the
        /// track picks the optimal orientation for these dimensions. Pass in the
        /// track's minimum current and maximum values; the indicator will be
        /// positioned appropriately. You can also specify whether the track is lit or
        /// not.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPDrawTrack", ExactSpelling = true)]
        public static extern void DrawTrack(int inX1, int inY1, int inX2, int inY2, int inMin, int inMax, int inValue, TrackStyle inTrackStyle, int inLit);

        
        /// <summary>
        /// <para>
        /// This routine returns a track's default smaller dimension; all tracks are
        /// scalable in the larger dimension. It also returns whether a track can be
        /// lit.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPGetTrackDefaultDimensions", ExactSpelling = true)]
        public static extern unsafe void GetTrackDefaultDimensions(TrackStyle inStyle, int* outWidth, int* outCanBeLit);

        
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
        [DllImportAttribute(Lib.Name, EntryPoint = "XPGetTrackMetrics", ExactSpelling = true)]
        public static extern unsafe void GetTrackMetrics(int inX1, int inY1, int inX2, int inY2, int inMin, int inMax, int inValue, TrackStyle inTrackStyle, int* outIsVertical, int* outDownBtnSize, int* outDownPageSize, int* outThumbSize, int* outUpPageSize, int* outUpBtnSize);
    }
}