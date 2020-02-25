using InlineIL;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Internal
{
    public static partial class Utilities
    {
        private static IntPtr SimulateKeyPressPtr;
        private static IntPtr SpeakStringPtr;
        private static IntPtr CommandKeyStrokePtr;
        private static IntPtr CommandButtonPressPtr;
        private static IntPtr CommandButtonReleasePtr;
        private static IntPtr GetVirtualKeyDescriptionPtr;
        private static IntPtr ReloadSceneryPtr;
        private static IntPtr GetSystemPathPtr;
        private static IntPtr GetPrefsPathPtr;
        private static IntPtr GetDirectorySeparatorPtr;
        private static IntPtr ExtractFileAndPathPtr;
        private static IntPtr GetDirectoryContentsPtr;
        private static IntPtr InitializedPtr;
        private static IntPtr GetVersionsPtr;
        private static IntPtr GetLanguagePtr;
        private static IntPtr DebugStringPtr;
        private static IntPtr SetErrorCallbackPtr;
        private static IntPtr FindSymbolPtr;
        private static IntPtr LoadDataFilePtr;
        private static IntPtr SaveDataFilePtr;
        private static IntPtr FindCommandPtr;
        private static IntPtr CommandBeginPtr;
        private static IntPtr CommandEndPtr;
        private static IntPtr CommandOncePtr;
        private static IntPtr CreateCommandPtr;
        private static IntPtr RegisterCommandHandlerPtr;
        private static IntPtr UnregisterCommandHandlerPtr;
        static Utilities()
        {
            const string libraryName = "XPLM";
            SimulateKeyPressPtr = FunctionResolver.Resolve(libraryName, "XPLMSimulateKeyPress");
            SpeakStringPtr = FunctionResolver.Resolve(libraryName, "XPLMSpeakString");
            CommandKeyStrokePtr = FunctionResolver.Resolve(libraryName, "XPLMCommandKeyStroke");
            CommandButtonPressPtr = FunctionResolver.Resolve(libraryName, "XPLMCommandButtonPress");
            CommandButtonReleasePtr = FunctionResolver.Resolve(libraryName, "XPLMCommandButtonRelease");
            GetVirtualKeyDescriptionPtr = FunctionResolver.Resolve(libraryName, "XPLMGetVirtualKeyDescription");
            ReloadSceneryPtr = FunctionResolver.Resolve(libraryName, "XPLMReloadScenery");
            GetSystemPathPtr = FunctionResolver.Resolve(libraryName, "XPLMGetSystemPath");
            GetPrefsPathPtr = FunctionResolver.Resolve(libraryName, "XPLMGetPrefsPath");
            GetDirectorySeparatorPtr = FunctionResolver.Resolve(libraryName, "XPLMGetDirectorySeparator");
            ExtractFileAndPathPtr = FunctionResolver.Resolve(libraryName, "XPLMExtractFileAndPath");
            GetDirectoryContentsPtr = FunctionResolver.Resolve(libraryName, "XPLMGetDirectoryContents");
            InitializedPtr = FunctionResolver.Resolve(libraryName, "XPLMInitialized");
            GetVersionsPtr = FunctionResolver.Resolve(libraryName, "XPLMGetVersions");
            GetLanguagePtr = FunctionResolver.Resolve(libraryName, "XPLMGetLanguage");
            DebugStringPtr = FunctionResolver.Resolve(libraryName, "XPLMDebugString");
            SetErrorCallbackPtr = FunctionResolver.Resolve(libraryName, "XPLMSetErrorCallback");
            FindSymbolPtr = FunctionResolver.Resolve(libraryName, "XPLMFindSymbol");
            LoadDataFilePtr = FunctionResolver.Resolve(libraryName, "XPLMLoadDataFile");
            SaveDataFilePtr = FunctionResolver.Resolve(libraryName, "XPLMSaveDataFile");
            FindCommandPtr = FunctionResolver.Resolve(libraryName, "XPLMFindCommand");
            CommandBeginPtr = FunctionResolver.Resolve(libraryName, "XPLMCommandBegin");
            CommandEndPtr = FunctionResolver.Resolve(libraryName, "XPLMCommandEnd");
            CommandOncePtr = FunctionResolver.Resolve(libraryName, "XPLMCommandOnce");
            CreateCommandPtr = FunctionResolver.Resolve(libraryName, "XPLMCreateCommand");
            RegisterCommandHandlerPtr = FunctionResolver.Resolve(libraryName, "XPLMRegisterCommandHandler");
            UnregisterCommandHandlerPtr = FunctionResolver.Resolve(libraryName, "XPLMUnregisterCommandHandler");
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SimulateKeyPress(int inKeyType, int inKey)
        {
            IL.DeclareLocals(false);
            IL.Push(inKeyType);
            IL.Push(inKey);
            IL.Push(SimulateKeyPressPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int), typeof(int)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SpeakString(byte *inString)
        {
            IL.DeclareLocals(false);
            IL.Push(inString);
            IL.Push(SpeakStringPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(byte *)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void CommandKeyStroke(CommandKeyID inKey)
        {
            IL.DeclareLocals(false);
            IL.Push(inKey);
            IL.Push(CommandKeyStrokePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(CommandKeyID)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void CommandButtonPress(CommandButtonID inButton)
        {
            IL.DeclareLocals(false);
            IL.Push(inButton);
            IL.Push(CommandButtonPressPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(CommandButtonID)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void CommandButtonRelease(CommandButtonID inButton)
        {
            IL.DeclareLocals(false);
            IL.Push(inButton);
            IL.Push(CommandButtonReleasePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(CommandButtonID)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe byte *GetVirtualKeyDescription(byte inVirtualKey)
        {
            IL.DeclareLocals(false);
            byte *result;
            IL.Push(inVirtualKey);
            IL.Push(GetVirtualKeyDescriptionPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(byte *), typeof(byte)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void ReloadScenery()
        {
            IL.DeclareLocals(false);
            IL.Push(ReloadSceneryPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetSystemPath(byte *outSystemPath)
        {
            IL.DeclareLocals(false);
            IL.Push(outSystemPath);
            IL.Push(GetSystemPathPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(byte *)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetPrefsPath(byte *outPrefsPath)
        {
            IL.DeclareLocals(false);
            IL.Push(outPrefsPath);
            IL.Push(GetPrefsPathPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(byte *)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe byte *GetDirectorySeparator()
        {
            IL.DeclareLocals(false);
            byte *result;
            IL.Push(GetDirectorySeparatorPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(byte *)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe byte *ExtractFileAndPath(byte *inFullPath)
        {
            IL.DeclareLocals(false);
            byte *result;
            IL.Push(inFullPath);
            IL.Push(ExtractFileAndPathPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(byte *), typeof(byte *)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int GetDirectoryContents(byte *inDirectoryPath, int inFirstReturn, byte *outFileNames, int inFileNameBufSize, byte **outIndices, int inIndexCount, int *outTotalFiles, int *outReturnedFiles)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inDirectoryPath);
            IL.Push(inFirstReturn);
            IL.Push(outFileNames);
            IL.Push(inFileNameBufSize);
            IL.Push(outIndices);
            IL.Push(inIndexCount);
            IL.Push(outTotalFiles);
            IL.Push(outReturnedFiles);
            IL.Push(GetDirectoryContentsPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(byte *), typeof(int), typeof(byte *), typeof(int), typeof(byte **), typeof(int), typeof(int *), typeof(int *)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int Initialized()
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(InitializedPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetVersions(int *outXPlaneVersion, int *outXPLMVersion, HostApplicationID*outHostID)
        {
            IL.DeclareLocals(false);
            IL.Push(outXPlaneVersion);
            IL.Push(outXPLMVersion);
            IL.Push(outHostID);
            IL.Push(GetVersionsPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int *), typeof(int *), typeof(HostApplicationID*)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static LanguageCode GetLanguage()
        {
            IL.DeclareLocals(false);
            LanguageCode result;
            IL.Push(GetLanguagePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(LanguageCode)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void DebugString(byte *inString)
        {
            IL.DeclareLocals(false);
            IL.Push(inString);
            IL.Push(DebugStringPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(byte *)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetErrorCallback(ErrorCallback inCallback)
        {
            IL.DeclareLocals(false);
            IntPtr inCallbackPtr = Marshal.GetFunctionPointerForDelegate(inCallback);
            IL.Push(inCallbackPtr);
            IL.Push(SetErrorCallbackPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(IntPtr)));
            GC.KeepAlive(inCallback);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void *FindSymbol(byte *inString)
        {
            IL.DeclareLocals(false);
            void *result;
            IL.Push(inString);
            IL.Push(FindSymbolPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void *), typeof(byte *)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int LoadDataFile(DataFileType inFileType, byte *inFilePath)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inFileType);
            IL.Push(inFilePath);
            IL.Push(LoadDataFilePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(DataFileType), typeof(byte *)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int SaveDataFile(DataFileType inFileType, byte *inFilePath)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inFileType);
            IL.Push(inFilePath);
            IL.Push(SaveDataFilePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(DataFileType), typeof(byte *)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe CommandRef FindCommand(byte *inName)
        {
            IL.DeclareLocals(false);
            CommandRef result;
            IL.Push(inName);
            IL.Push(FindCommandPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(CommandRef), typeof(byte *)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void CommandBegin(CommandRef inCommand)
        {
            IL.DeclareLocals(false);
            IL.Push(inCommand);
            IL.Push(CommandBeginPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(CommandRef)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void CommandEnd(CommandRef inCommand)
        {
            IL.DeclareLocals(false);
            IL.Push(inCommand);
            IL.Push(CommandEndPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(CommandRef)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void CommandOnce(CommandRef inCommand)
        {
            IL.DeclareLocals(false);
            IL.Push(inCommand);
            IL.Push(CommandOncePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(CommandRef)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe CommandRef CreateCommand(byte *inName, byte *inDescription)
        {
            IL.DeclareLocals(false);
            CommandRef result;
            IL.Push(inName);
            IL.Push(inDescription);
            IL.Push(CreateCommandPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(CommandRef), typeof(byte *), typeof(byte *)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void RegisterCommandHandler(CommandRef inComand, CommandCallback inHandler, int inBefore, void *inRefcon)
        {
            IL.DeclareLocals(false);
            IntPtr inHandlerPtr = Marshal.GetFunctionPointerForDelegate(inHandler);
            IL.Push(inComand);
            IL.Push(inHandlerPtr);
            IL.Push(inBefore);
            IL.Push(inRefcon);
            IL.Push(RegisterCommandHandlerPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(CommandRef), typeof(IntPtr), typeof(int), typeof(void *)));
            GC.KeepAlive(inHandler);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void UnregisterCommandHandler(CommandRef inComand, CommandCallback inHandler, int inBefore, void *inRefcon)
        {
            IL.DeclareLocals(false);
            IntPtr inHandlerPtr = Marshal.GetFunctionPointerForDelegate(inHandler);
            IL.Push(inComand);
            IL.Push(inHandlerPtr);
            IL.Push(inBefore);
            IL.Push(inRefcon);
            IL.Push(UnregisterCommandHandlerPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(CommandRef), typeof(IntPtr), typeof(int), typeof(void *)));
            GC.KeepAlive(inHandler);
        }
    }
}