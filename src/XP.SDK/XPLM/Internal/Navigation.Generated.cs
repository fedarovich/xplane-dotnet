using InlineIL;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Internal
{
    public static partial class Navigation
    {
        private static IntPtr GetFirstNavAidPtr;
        private static IntPtr GetNextNavAidPtr;
        private static IntPtr FindFirstNavAidOfTypePtr;
        private static IntPtr FindLastNavAidOfTypePtr;
        private static IntPtr FindNavAidPtr;
        private static IntPtr GetNavAidInfoPtr;
        private static IntPtr CountFMSEntriesPtr;
        private static IntPtr GetDisplayedFMSEntryPtr;
        private static IntPtr GetDestinationFMSEntryPtr;
        private static IntPtr SetDisplayedFMSEntryPtr;
        private static IntPtr SetDestinationFMSEntryPtr;
        private static IntPtr GetFMSEntryInfoPtr;
        private static IntPtr SetFMSEntryInfoPtr;
        private static IntPtr SetFMSEntryLatLonPtr;
        private static IntPtr ClearFMSEntryPtr;
        private static IntPtr GetGPSDestinationTypePtr;
        private static IntPtr GetGPSDestinationPtr;
        static Navigation()
        {
            const string libraryName = "XPLM";
            GetFirstNavAidPtr = FunctionResolver.Resolve(libraryName, "XPLMGetFirstNavAid");
            GetNextNavAidPtr = FunctionResolver.Resolve(libraryName, "XPLMGetNextNavAid");
            FindFirstNavAidOfTypePtr = FunctionResolver.Resolve(libraryName, "XPLMFindFirstNavAidOfType");
            FindLastNavAidOfTypePtr = FunctionResolver.Resolve(libraryName, "XPLMFindLastNavAidOfType");
            FindNavAidPtr = FunctionResolver.Resolve(libraryName, "XPLMFindNavAid");
            GetNavAidInfoPtr = FunctionResolver.Resolve(libraryName, "XPLMGetNavAidInfo");
            CountFMSEntriesPtr = FunctionResolver.Resolve(libraryName, "XPLMCountFMSEntries");
            GetDisplayedFMSEntryPtr = FunctionResolver.Resolve(libraryName, "XPLMGetDisplayedFMSEntry");
            GetDestinationFMSEntryPtr = FunctionResolver.Resolve(libraryName, "XPLMGetDestinationFMSEntry");
            SetDisplayedFMSEntryPtr = FunctionResolver.Resolve(libraryName, "XPLMSetDisplayedFMSEntry");
            SetDestinationFMSEntryPtr = FunctionResolver.Resolve(libraryName, "XPLMSetDestinationFMSEntry");
            GetFMSEntryInfoPtr = FunctionResolver.Resolve(libraryName, "XPLMGetFMSEntryInfo");
            SetFMSEntryInfoPtr = FunctionResolver.Resolve(libraryName, "XPLMSetFMSEntryInfo");
            SetFMSEntryLatLonPtr = FunctionResolver.Resolve(libraryName, "XPLMSetFMSEntryLatLon");
            ClearFMSEntryPtr = FunctionResolver.Resolve(libraryName, "XPLMClearFMSEntry");
            GetGPSDestinationTypePtr = FunctionResolver.Resolve(libraryName, "XPLMGetGPSDestinationType");
            GetGPSDestinationPtr = FunctionResolver.Resolve(libraryName, "XPLMGetGPSDestination");
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static NavRef GetFirstNavAid()
        {
            IL.DeclareLocals(false);
            NavRef result;
            IL.Push(GetFirstNavAidPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(NavRef)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static NavRef GetNextNavAid(NavRef inNavAidRef)
        {
            IL.DeclareLocals(false);
            NavRef result;
            IL.Push(inNavAidRef);
            IL.Push(GetNextNavAidPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(NavRef), typeof(NavRef)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static NavRef FindFirstNavAidOfType(NavType inType)
        {
            IL.DeclareLocals(false);
            NavRef result;
            IL.Push(inType);
            IL.Push(FindFirstNavAidOfTypePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(NavRef), typeof(NavType)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static NavRef FindLastNavAidOfType(NavType inType)
        {
            IL.DeclareLocals(false);
            NavRef result;
            IL.Push(inType);
            IL.Push(FindLastNavAidOfTypePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(NavRef), typeof(NavType)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe NavRef FindNavAid(byte *inNameFragment, byte *inIDFragment, float *inLat, float *inLon, int *inFrequency, NavType inType)
        {
            IL.DeclareLocals(false);
            NavRef result;
            IL.Push(inNameFragment);
            IL.Push(inIDFragment);
            IL.Push(inLat);
            IL.Push(inLon);
            IL.Push(inFrequency);
            IL.Push(inType);
            IL.Push(FindNavAidPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(NavRef), typeof(byte *), typeof(byte *), typeof(float *), typeof(float *), typeof(int *), typeof(NavType)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetNavAidInfo(NavRef inRef, NavType*outType, float *outLatitude, float *outLongitude, float *outHeight, int *outFrequency, float *outHeading, byte *outID, byte *outName, byte *outReg)
        {
            IL.DeclareLocals(false);
            IL.Push(inRef);
            IL.Push(outType);
            IL.Push(outLatitude);
            IL.Push(outLongitude);
            IL.Push(outHeight);
            IL.Push(outFrequency);
            IL.Push(outHeading);
            IL.Push(outID);
            IL.Push(outName);
            IL.Push(outReg);
            IL.Push(GetNavAidInfoPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(NavRef), typeof(NavType*), typeof(float *), typeof(float *), typeof(float *), typeof(int *), typeof(float *), typeof(byte *), typeof(byte *), typeof(byte *)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int CountFMSEntries()
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(CountFMSEntriesPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int GetDisplayedFMSEntry()
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(GetDisplayedFMSEntryPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int GetDestinationFMSEntry()
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(GetDestinationFMSEntryPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetDisplayedFMSEntry(int inIndex)
        {
            IL.DeclareLocals(false);
            IL.Push(inIndex);
            IL.Push(SetDisplayedFMSEntryPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetDestinationFMSEntry(int inIndex)
        {
            IL.DeclareLocals(false);
            IL.Push(inIndex);
            IL.Push(SetDestinationFMSEntryPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetFMSEntryInfo(int inIndex, NavType*outType, byte *outID, NavRef*outRef, int *outAltitude, float *outLat, float *outLon)
        {
            IL.DeclareLocals(false);
            IL.Push(inIndex);
            IL.Push(outType);
            IL.Push(outID);
            IL.Push(outRef);
            IL.Push(outAltitude);
            IL.Push(outLat);
            IL.Push(outLon);
            IL.Push(GetFMSEntryInfoPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int), typeof(NavType*), typeof(byte *), typeof(NavRef*), typeof(int *), typeof(float *), typeof(float *)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetFMSEntryInfo(int inIndex, NavRef inRef, int inAltitude)
        {
            IL.DeclareLocals(false);
            IL.Push(inIndex);
            IL.Push(inRef);
            IL.Push(inAltitude);
            IL.Push(SetFMSEntryInfoPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int), typeof(NavRef), typeof(int)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetFMSEntryLatLon(int inIndex, float inLat, float inLon, int inAltitude)
        {
            IL.DeclareLocals(false);
            IL.Push(inIndex);
            IL.Push(inLat);
            IL.Push(inLon);
            IL.Push(inAltitude);
            IL.Push(SetFMSEntryLatLonPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int), typeof(float), typeof(float), typeof(int)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void ClearFMSEntry(int inIndex)
        {
            IL.DeclareLocals(false);
            IL.Push(inIndex);
            IL.Push(ClearFMSEntryPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static NavType GetGPSDestinationType()
        {
            IL.DeclareLocals(false);
            NavType result;
            IL.Push(GetGPSDestinationTypePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(NavType)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static NavRef GetGPSDestination()
        {
            IL.DeclareLocals(false);
            NavRef result;
            IL.Push(GetGPSDestinationPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(NavRef)));
            IL.Pop(out result);
            return result;
        }
    }
}