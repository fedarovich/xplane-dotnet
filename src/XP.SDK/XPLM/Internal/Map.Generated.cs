using InlineIL;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Internal
{
    public static partial class Map
    {
        private static IntPtr CreateMapLayerPtr;
        private static IntPtr DestroyMapLayerPtr;
        private static IntPtr RegisterMapCreationHookPtr;
        private static IntPtr MapExistsPtr;
        private static IntPtr DrawMapIconFromSheetPtr;
        private static IntPtr DrawMapLabelPtr;
        private static IntPtr MapProjectPtr;
        private static IntPtr MapUnprojectPtr;
        private static IntPtr MapScaleMeterPtr;
        private static IntPtr MapGetNorthHeadingPtr;
        static Map()
        {
            const string libraryName = "XPLM";
            CreateMapLayerPtr = FunctionResolver.Resolve(libraryName, "XPLMCreateMapLayer");
            DestroyMapLayerPtr = FunctionResolver.Resolve(libraryName, "XPLMDestroyMapLayer");
            RegisterMapCreationHookPtr = FunctionResolver.Resolve(libraryName, "XPLMRegisterMapCreationHook");
            MapExistsPtr = FunctionResolver.Resolve(libraryName, "XPLMMapExists");
            DrawMapIconFromSheetPtr = FunctionResolver.Resolve(libraryName, "XPLMDrawMapIconFromSheet");
            DrawMapLabelPtr = FunctionResolver.Resolve(libraryName, "XPLMDrawMapLabel");
            MapProjectPtr = FunctionResolver.Resolve(libraryName, "XPLMMapProject");
            MapUnprojectPtr = FunctionResolver.Resolve(libraryName, "XPLMMapUnproject");
            MapScaleMeterPtr = FunctionResolver.Resolve(libraryName, "XPLMMapScaleMeter");
            MapGetNorthHeadingPtr = FunctionResolver.Resolve(libraryName, "XPLMMapGetNorthHeading");
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe MapLayerID CreateMapLayer(CreateMapLayer*inParams)
        {
            IL.DeclareLocals(false);
            MapLayerID result;
            IL.Push(inParams);
            IL.Push(CreateMapLayerPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(MapLayerID), typeof(CreateMapLayer*)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int DestroyMapLayer(MapLayerID inLayer)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inLayer);
            IL.Push(DestroyMapLayerPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(MapLayerID)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void RegisterMapCreationHook(MapCreatedCallback callback, void *refcon)
        {
            IL.DeclareLocals(false);
            IntPtr callbackPtr = Marshal.GetFunctionPointerForDelegate(callback);
            IL.Push(callbackPtr);
            IL.Push(refcon);
            IL.Push(RegisterMapCreationHookPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(IntPtr), typeof(void *)));
            GC.KeepAlive(callback);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int MapExists(byte *mapIdentifier)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(mapIdentifier);
            IL.Push(MapExistsPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(byte *)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void DrawMapIconFromSheet(MapLayerID layer, byte *inPngPath, int s, int t, int ds, int dt, float mapX, float mapY, MapOrientation orientation, float rotationDegrees, float mapWidth)
        {
            IL.DeclareLocals(false);
            IL.Push(layer);
            IL.Push(inPngPath);
            IL.Push(s);
            IL.Push(t);
            IL.Push(ds);
            IL.Push(dt);
            IL.Push(mapX);
            IL.Push(mapY);
            IL.Push(orientation);
            IL.Push(rotationDegrees);
            IL.Push(mapWidth);
            IL.Push(DrawMapIconFromSheetPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(MapLayerID), typeof(byte *), typeof(int), typeof(int), typeof(int), typeof(int), typeof(float), typeof(float), typeof(MapOrientation), typeof(float), typeof(float)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void DrawMapLabel(MapLayerID layer, byte *inText, float mapX, float mapY, MapOrientation orientation, float rotationDegrees)
        {
            IL.DeclareLocals(false);
            IL.Push(layer);
            IL.Push(inText);
            IL.Push(mapX);
            IL.Push(mapY);
            IL.Push(orientation);
            IL.Push(rotationDegrees);
            IL.Push(DrawMapLabelPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(MapLayerID), typeof(byte *), typeof(float), typeof(float), typeof(MapOrientation), typeof(float)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void MapProject(MapProjectionID projection, double latitude, double longitude, float *outX, float *outY)
        {
            IL.DeclareLocals(false);
            IL.Push(projection);
            IL.Push(latitude);
            IL.Push(longitude);
            IL.Push(outX);
            IL.Push(outY);
            IL.Push(MapProjectPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(MapProjectionID), typeof(double), typeof(double), typeof(float *), typeof(float *)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void MapUnproject(MapProjectionID projection, float mapX, float mapY, double *outLatitude, double *outLongitude)
        {
            IL.DeclareLocals(false);
            IL.Push(projection);
            IL.Push(mapX);
            IL.Push(mapY);
            IL.Push(outLatitude);
            IL.Push(outLongitude);
            IL.Push(MapUnprojectPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(MapProjectionID), typeof(float), typeof(float), typeof(double *), typeof(double *)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static float MapScaleMeter(MapProjectionID projection, float mapX, float mapY)
        {
            IL.DeclareLocals(false);
            float result;
            IL.Push(projection);
            IL.Push(mapX);
            IL.Push(mapY);
            IL.Push(MapScaleMeterPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(float), typeof(MapProjectionID), typeof(float), typeof(float)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static float MapGetNorthHeading(MapProjectionID projection, float mapX, float mapY)
        {
            IL.DeclareLocals(false);
            float result;
            IL.Push(projection);
            IL.Push(mapX);
            IL.Push(mapY);
            IL.Push(MapGetNorthHeadingPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(float), typeof(MapProjectionID), typeof(float), typeof(float)));
            IL.Pop(out result);
            return result;
        }
    }
}