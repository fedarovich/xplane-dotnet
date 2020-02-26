using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using InlineIL;

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

        
        /// <summary>
        /// <para>
        /// This routine returns the plugin ID of the calling plug-in.  Call this to
        /// get your own ID.
        /// </para>
        /// </summary>
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

        
        /// <summary>
        /// <para>
        /// This routine returns the total number of plug-ins that are loaded, both
        /// disabled and enabled.
        /// </para>
        /// </summary>
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

        
        /// <summary>
        /// <para>
        /// This routine returns the ID of a plug-in by index.  Index is 0 based from 0
        /// to XPLMCountPlugins-1, inclusive. Plugins may be returned in any arbitrary
        /// order.
        /// </para>
        /// </summary>
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

        
        /// <summary>
        /// <para>
        /// This routine returns the plug-in ID of the plug-in whose file exists at the
        /// passed in absolute file system path.  XPLM_NO_PLUGIN_ID is returned if the
        /// path does not point to a currently loaded plug-in.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe PluginID FindPluginByPath(byte* inPath)
        {
            IL.DeclareLocals(false);
            PluginID result;
            IL.Push(inPath);
            IL.Push(FindPluginByPathPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(PluginID), typeof(byte*)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine returns the plug-in ID of the plug-in whose file exists at the
        /// passed in absolute file system path.  XPLM_NO_PLUGIN_ID is returned if the
        /// path does not point to a currently loaded plug-in.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe PluginID FindPluginByPath(in ReadOnlySpan<char> inPath)
        {
            IL.DeclareLocals(false);
            Span<byte> inPathUtf8 = stackalloc byte[(inPath.Length << 1) | 1];
            var inPathPtr = Utils.ToUtf8Unsafe(inPath, inPathUtf8);
            return FindPluginByPath(inPathPtr);
        }

        
        /// <summary>
        /// <para>
        /// This routine returns the plug-in ID of the plug-in whose signature matches
        /// what is passed in or XPLM_NO_PLUGIN_ID if no running plug-in has this
        /// signature.  Signatures are the best way to identify another plug-in as they
        /// are independent of the file system path of a plug-in or the human-readable
        /// plug-in name, and should be unique for all plug-ins.  Use this routine to
        /// locate another plugin that your plugin interoperates with
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe PluginID FindPluginBySignature(byte* inSignature)
        {
            IL.DeclareLocals(false);
            PluginID result;
            IL.Push(inSignature);
            IL.Push(FindPluginBySignaturePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(PluginID), typeof(byte*)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine returns the plug-in ID of the plug-in whose signature matches
        /// what is passed in or XPLM_NO_PLUGIN_ID if no running plug-in has this
        /// signature.  Signatures are the best way to identify another plug-in as they
        /// are independent of the file system path of a plug-in or the human-readable
        /// plug-in name, and should be unique for all plug-ins.  Use this routine to
        /// locate another plugin that your plugin interoperates with
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe PluginID FindPluginBySignature(in ReadOnlySpan<char> inSignature)
        {
            IL.DeclareLocals(false);
            Span<byte> inSignatureUtf8 = stackalloc byte[(inSignature.Length << 1) | 1];
            var inSignaturePtr = Utils.ToUtf8Unsafe(inSignature, inSignatureUtf8);
            return FindPluginBySignature(inSignaturePtr);
        }

        
        /// <summary>
        /// <para>
        /// This routine returns information about a plug-in.  Each parameter should be
        /// a pointer to a buffer of at least 256 characters, or NULL to not receive
        /// the information.
        /// </para>
        /// <para>
        /// outName - the human-readable name of the plug-in.   outFilePath - the
        /// absolute file path to the file that contains this plug-in. outSignature - a
        /// unique string that identifies this plug-in. outDescription - a
        /// human-readable description of this plug-in.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetPluginInfo(PluginID inPlugin, byte* outName, byte* outFilePath, byte* outSignature, byte* outDescription)
        {
            IL.DeclareLocals(false);
            IL.Push(inPlugin);
            IL.Push(outName);
            IL.Push(outFilePath);
            IL.Push(outSignature);
            IL.Push(outDescription);
            IL.Push(GetPluginInfoPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(PluginID), typeof(byte*), typeof(byte*), typeof(byte*), typeof(byte*)));
        }

        
        /// <summary>
        /// <para>
        /// Returns whether the specified plug-in is enabled for running.
        /// </para>
        /// </summary>
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

        
        /// <summary>
        /// <para>
        /// This routine enables a plug-in if it is not already enabled.  It returns 1
        /// if the plugin was enabled or successfully enables itself, 0 if it does not.
        /// Plugins may fail to enable (for example, if resources cannot be acquired)
        /// by returning 0 from their XPluginEnable callback.
        /// </para>
        /// </summary>
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

        
        /// <summary>
        /// <para>
        /// This routine disableds an enabled plug-in.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void DisablePlugin(PluginID inPluginID)
        {
            IL.DeclareLocals(false);
            IL.Push(inPluginID);
            IL.Push(DisablePluginPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(PluginID)));
        }

        
        /// <summary>
        /// <para>
        /// This routine reloads all plug-ins.  Once this routine is called and you
        /// return from the callback you were within (e.g. a menu select callback) you
        /// will receive your XPluginDisable and XPluginStop callbacks and your  DLL
        /// will be unloaded, then the start process happens as if the sim was starting
        /// up.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void ReloadPlugins()
        {
            IL.DeclareLocals(false);
            IL.Push(ReloadPluginsPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void)));
        }

        
        /// <summary>
        /// <para>
        /// This function sends a message to another plug-in or X-Plane.  Pass
        /// XPLM_NO_PLUGIN_ID to broadcast to all plug-ins.  Only enabled plug-ins with
        /// a message receive function receive the message.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SendMessageToPlugin(PluginID inPlugin, int inMessage, void* inParam)
        {
            IL.DeclareLocals(false);
            IL.Push(inPlugin);
            IL.Push(inMessage);
            IL.Push(inParam);
            IL.Push(SendMessageToPluginPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(PluginID), typeof(int), typeof(void*)));
        }

        
        /// <summary>
        /// <para>
        /// This returns 1 if the given installation of X-Plane supports a feature, or
        /// 0 if it does not.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int HasFeature(byte* inFeature)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inFeature);
            IL.Push(HasFeaturePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(byte*)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This returns 1 if the given installation of X-Plane supports a feature, or
        /// 0 if it does not.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int HasFeature(in ReadOnlySpan<char> inFeature)
        {
            IL.DeclareLocals(false);
            Span<byte> inFeatureUtf8 = stackalloc byte[(inFeature.Length << 1) | 1];
            var inFeaturePtr = Utils.ToUtf8Unsafe(inFeature, inFeatureUtf8);
            return HasFeature(inFeaturePtr);
        }

        
        /// <summary>
        /// <para>
        /// This returns 1 if a feature is currently enabled for your plugin, or 0 if
        /// it is not enabled.  It is an error to call this routine with an unsupported
        /// feature.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int IsFeatureEnabled(byte* inFeature)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inFeature);
            IL.Push(IsFeatureEnabledPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(byte*)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This returns 1 if a feature is currently enabled for your plugin, or 0 if
        /// it is not enabled.  It is an error to call this routine with an unsupported
        /// feature.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int IsFeatureEnabled(in ReadOnlySpan<char> inFeature)
        {
            IL.DeclareLocals(false);
            Span<byte> inFeatureUtf8 = stackalloc byte[(inFeature.Length << 1) | 1];
            var inFeaturePtr = Utils.ToUtf8Unsafe(inFeature, inFeatureUtf8);
            return IsFeatureEnabled(inFeaturePtr);
        }

        
        /// <summary>
        /// <para>
        /// This routine enables or disables a feature for your plugin.  This will
        /// change the running behavior of X-Plane and your plugin in some way,
        /// depending on the feature.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void EnableFeature(byte* inFeature, int inEnable)
        {
            IL.DeclareLocals(false);
            IL.Push(inFeature);
            IL.Push(inEnable);
            IL.Push(EnableFeaturePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(byte*), typeof(int)));
        }

        
        /// <summary>
        /// <para>
        /// This routine enables or disables a feature for your plugin.  This will
        /// change the running behavior of X-Plane and your plugin in some way,
        /// depending on the feature.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void EnableFeature(in ReadOnlySpan<char> inFeature, int inEnable)
        {
            IL.DeclareLocals(false);
            Span<byte> inFeatureUtf8 = stackalloc byte[(inFeature.Length << 1) | 1];
            var inFeaturePtr = Utils.ToUtf8Unsafe(inFeature, inFeatureUtf8);
            EnableFeature(inFeaturePtr, inEnable);
        }

        
        /// <summary>
        /// <para>
        /// This routine calls your enumerator callback once for each feature that this
        /// running version of X-Plane supports. Use this routine to determine all of
        /// the features that X-Plane can support.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void EnumerateFeatures(FeatureEnumeratorCallback inEnumerator, void* inRef)
        {
            IL.DeclareLocals(false);
            IntPtr inEnumeratorPtr = Marshal.GetFunctionPointerForDelegate(inEnumerator);
            IL.Push(inEnumeratorPtr);
            IL.Push(inRef);
            IL.Push(EnumerateFeaturesPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(IntPtr), typeof(void*)));
            GC.KeepAlive(inEnumerator);
        }
    }
}