using InlineIL;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Internal
{
    public static partial class Plugin
    {
        private static IntPtr GetMyIDPtr;
        private static IntPtr CountPluginsPtr;
        private static IntPtr GetNthPluginPtr;
        private static IntPtr FindPluginByPathPtr;
        private static IntPtr FindPluginBySignaturePtr;
        private static IntPtr GetPluginInfoPtr;
        private static IntPtr IsPluginEnabledPtr;
        private static IntPtr EnablePluginPtr;
        private static IntPtr DisablePluginPtr;
        private static IntPtr ReloadPluginsPtr;
        private static IntPtr SendMessageToPluginPtr;
        private static IntPtr HasFeaturePtr;
        private static IntPtr IsFeatureEnabledPtr;
        private static IntPtr EnableFeaturePtr;
        private static IntPtr EnumerateFeaturesPtr;
        static Plugin()
        {
            const string libraryName = "XPLM";
            GetMyIDPtr = FunctionResolver.Resolve(libraryName, "XPLMGetMyID");
            CountPluginsPtr = FunctionResolver.Resolve(libraryName, "XPLMCountPlugins");
            GetNthPluginPtr = FunctionResolver.Resolve(libraryName, "XPLMGetNthPlugin");
            FindPluginByPathPtr = FunctionResolver.Resolve(libraryName, "XPLMFindPluginByPath");
            FindPluginBySignaturePtr = FunctionResolver.Resolve(libraryName, "XPLMFindPluginBySignature");
            GetPluginInfoPtr = FunctionResolver.Resolve(libraryName, "XPLMGetPluginInfo");
            IsPluginEnabledPtr = FunctionResolver.Resolve(libraryName, "XPLMIsPluginEnabled");
            EnablePluginPtr = FunctionResolver.Resolve(libraryName, "XPLMEnablePlugin");
            DisablePluginPtr = FunctionResolver.Resolve(libraryName, "XPLMDisablePlugin");
            ReloadPluginsPtr = FunctionResolver.Resolve(libraryName, "XPLMReloadPlugins");
            SendMessageToPluginPtr = FunctionResolver.Resolve(libraryName, "XPLMSendMessageToPlugin");
            HasFeaturePtr = FunctionResolver.Resolve(libraryName, "XPLMHasFeature");
            IsFeatureEnabledPtr = FunctionResolver.Resolve(libraryName, "XPLMIsFeatureEnabled");
            EnableFeaturePtr = FunctionResolver.Resolve(libraryName, "XPLMEnableFeature");
            EnumerateFeaturesPtr = FunctionResolver.Resolve(libraryName, "XPLMEnumerateFeatures");
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static PluginID GetMyID()
        {
            IL.DeclareLocals(false);
            PluginID result;
            IL.Push(GetMyIDPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(PluginID)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int CountPlugins()
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(CountPluginsPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static PluginID GetNthPlugin(int inIndex)
        {
            IL.DeclareLocals(false);
            PluginID result;
            IL.Push(inIndex);
            IL.Push(GetNthPluginPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(PluginID), typeof(int)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe PluginID FindPluginByPath(byte *inPath)
        {
            IL.DeclareLocals(false);
            PluginID result;
            IL.Push(inPath);
            IL.Push(FindPluginByPathPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(PluginID), typeof(byte *)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe PluginID FindPluginBySignature(byte *inSignature)
        {
            IL.DeclareLocals(false);
            PluginID result;
            IL.Push(inSignature);
            IL.Push(FindPluginBySignaturePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(PluginID), typeof(byte *)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetPluginInfo(PluginID inPlugin, byte *outName, byte *outFilePath, byte *outSignature, byte *outDescription)
        {
            IL.DeclareLocals(false);
            IL.Push(inPlugin);
            IL.Push(outName);
            IL.Push(outFilePath);
            IL.Push(outSignature);
            IL.Push(outDescription);
            IL.Push(GetPluginInfoPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(PluginID), typeof(byte *), typeof(byte *), typeof(byte *), typeof(byte *)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int IsPluginEnabled(PluginID inPluginID)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inPluginID);
            IL.Push(IsPluginEnabledPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(PluginID)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int EnablePlugin(PluginID inPluginID)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inPluginID);
            IL.Push(EnablePluginPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(PluginID)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void DisablePlugin(PluginID inPluginID)
        {
            IL.DeclareLocals(false);
            IL.Push(inPluginID);
            IL.Push(DisablePluginPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(PluginID)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void ReloadPlugins()
        {
            IL.DeclareLocals(false);
            IL.Push(ReloadPluginsPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SendMessageToPlugin(PluginID inPlugin, int inMessage, void *inParam)
        {
            IL.DeclareLocals(false);
            IL.Push(inPlugin);
            IL.Push(inMessage);
            IL.Push(inParam);
            IL.Push(SendMessageToPluginPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(PluginID), typeof(int), typeof(void *)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int HasFeature(byte *inFeature)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inFeature);
            IL.Push(HasFeaturePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(byte *)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int IsFeatureEnabled(byte *inFeature)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inFeature);
            IL.Push(IsFeatureEnabledPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(byte *)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void EnableFeature(byte *inFeature, int inEnable)
        {
            IL.DeclareLocals(false);
            IL.Push(inFeature);
            IL.Push(inEnable);
            IL.Push(EnableFeaturePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(byte *), typeof(int)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void EnumerateFeatures(FeatureEnumeratorCallback inEnumerator, void *inRef)
        {
            IL.DeclareLocals(false);
            IntPtr inEnumeratorPtr = Marshal.GetFunctionPointerForDelegate(inEnumerator);
            IL.Push(inEnumeratorPtr);
            IL.Push(inRef);
            IL.Push(EnumerateFeaturesPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(IntPtr), typeof(void *)));
            GC.KeepAlive(inEnumerator);
        }
    }
}