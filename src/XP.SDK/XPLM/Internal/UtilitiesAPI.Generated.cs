using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using InlineIL;

namespace XP.SDK.XPLM.Internal
{
    public static partial class UtilitiesAPI
    {
        private static IntPtr GetSystemPathPtr;
        private static IntPtr GetPrefsPathPtr;
        private static IntPtr GetDirectorySeparatorPtr;
        private static IntPtr ExtractFileAndPathPtr;
        private static IntPtr GetDirectoryContentsPtr;
        private static IntPtr LoadDataFilePtr;
        private static IntPtr SaveDataFilePtr;
        private static IntPtr GetVersionsPtr;
        private static IntPtr GetLanguagePtr;
        private static IntPtr FindSymbolPtr;
        private static IntPtr SetErrorCallbackPtr;
        private static IntPtr DebugStringPtr;
        private static IntPtr SpeakStringPtr;
        private static IntPtr GetVirtualKeyDescriptionPtr;
        private static IntPtr ReloadSceneryPtr;
        private static IntPtr FindCommandPtr;
        private static IntPtr CommandBeginPtr;
        private static IntPtr CommandEndPtr;
        private static IntPtr CommandOncePtr;
        private static IntPtr CreateCommandPtr;
        private static IntPtr RegisterCommandHandlerPtr;
        private static IntPtr UnregisterCommandHandlerPtr;

        static UtilitiesAPI()
        {
            GetSystemPathPtr = Lib.GetExport("XPLMGetSystemPath");
            GetPrefsPathPtr = Lib.GetExport("XPLMGetPrefsPath");
            GetDirectorySeparatorPtr = Lib.GetExport("XPLMGetDirectorySeparator");
            ExtractFileAndPathPtr = Lib.GetExport("XPLMExtractFileAndPath");
            GetDirectoryContentsPtr = Lib.GetExport("XPLMGetDirectoryContents");
            LoadDataFilePtr = Lib.GetExport("XPLMLoadDataFile");
            SaveDataFilePtr = Lib.GetExport("XPLMSaveDataFile");
            GetVersionsPtr = Lib.GetExport("XPLMGetVersions");
            GetLanguagePtr = Lib.GetExport("XPLMGetLanguage");
            FindSymbolPtr = Lib.GetExport("XPLMFindSymbol");
            SetErrorCallbackPtr = Lib.GetExport("XPLMSetErrorCallback");
            DebugStringPtr = Lib.GetExport("XPLMDebugString");
            SpeakStringPtr = Lib.GetExport("XPLMSpeakString");
            GetVirtualKeyDescriptionPtr = Lib.GetExport("XPLMGetVirtualKeyDescription");
            ReloadSceneryPtr = Lib.GetExport("XPLMReloadScenery");
            FindCommandPtr = Lib.GetExport("XPLMFindCommand");
            CommandBeginPtr = Lib.GetExport("XPLMCommandBegin");
            CommandEndPtr = Lib.GetExport("XPLMCommandEnd");
            CommandOncePtr = Lib.GetExport("XPLMCommandOnce");
            CreateCommandPtr = Lib.GetExport("XPLMCreateCommand");
            RegisterCommandHandlerPtr = Lib.GetExport("XPLMRegisterCommandHandler");
            UnregisterCommandHandlerPtr = Lib.GetExport("XPLMUnregisterCommandHandler");
        }

        
        /// <summary>
        /// <para>
        /// This function returns the full path to the X-System folder. Note that this
        /// is a directory path, so it ends in a trailing : or /.
        /// </para>
        /// <para>
        /// The buffer you pass should be at least 512 characters long.  The path is
        /// returned using the current native or OS path conventions.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetSystemPath(byte* outSystemPath)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(GetSystemPathPtr);
            IL.Push(outSystemPath);
            IL.Push(GetSystemPathPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(byte*)));
        }

        
        /// <summary>
        /// <para>
        /// This routine returns a full path to a file that is within X-Plane's
        /// preferences directory. (You should remove the file name back to the last
        /// directory separator to get the preferences directory using
        /// XPLMExtractFileAndPath.)
        /// </para>
        /// <para>
        /// The buffer you pass should be at least 512 characters long.  The path is
        /// returned using the current native or OS path conventions.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetPrefsPath(byte* outPrefsPath)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(GetPrefsPathPtr);
            IL.Push(outPrefsPath);
            IL.Push(GetPrefsPathPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(byte*)));
        }

        
        /// <summary>
        /// <para>
        /// This routine returns a string with one char and a null terminator that is
        /// the directory separator for the current platform. This allows you to write
        /// code that concatinates directory paths without having to #ifdef for
        /// platform. The character returned will reflect the current file path mode.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe byte* GetDirectorySeparator()
        {
            IL.DeclareLocals(false);
            Guard.NotNull(GetDirectorySeparatorPtr);
            byte* result;
            IL.Push(GetDirectorySeparatorPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(byte*)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// Given a full path to a file, this routine separates the path from the file.
        /// If the path is a partial directory (e.g. ends in : or
        /// \
        /// ) the trailing
        /// directory separator is removed. This routine works in-place; a pointer to
        /// the file part of the buffer is returned; the original buffer still starts
        /// with the path and is null terminated with no trailing separator.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe byte* ExtractFileAndPath(byte* inFullPath)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(ExtractFileAndPathPtr);
            byte* result;
            IL.Push(inFullPath);
            IL.Push(ExtractFileAndPathPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(byte*), typeof(byte*)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine returns a list of files in a directory (specified by a full
        /// path, no trailing : or
        /// \
        /// ). The output is returned as a list of NULL
        /// terminated strings. An index array (if specified) is filled with pointers
        /// into the strings. The last file is indicated by a zero-length string (and
        /// NULL in the indices). This routine will return 1 if you had capacity for
        /// all files or 0 if you did not. You can also skip a given number of files.
        /// </para>
        /// <para>
        /// * inDirectoryPath - a null terminated C string containing the full path to
        /// the directory with no trailing directory char.
        /// </para>
        /// <para>
        /// * inFirstReturn - the zero-based index of the first file in the directory
        /// to return. (Usually zero to fetch all in one pass.)
        /// </para>
        /// <para>
        /// * outFileNames - a buffer to receive a series of sequential null
        /// terminated C-string file names. A zero-length C string will be appended
        /// to the very end.
        /// </para>
        /// <para>
        /// * inFileNameBufSize - the size of the file name buffer in bytes.
        /// </para>
        /// <para>
        /// * outIndices - a pointer to an array of character pointers that will
        /// become an index into the directory. The last file will be followed by a
        /// NULL value. Pass NULL if you do not want indexing information.
        /// </para>
        /// <para>
        /// * inIndexCount - the max size of the index in entries.
        /// </para>
        /// <para>
        /// * outTotalFiles - if not NULL, this is filled in with the number of files
        /// in the directory.
        /// </para>
        /// <para>
        /// * outReturnedFiles - if not NULL, the number of files returned by this
        /// iteration.
        /// </para>
        /// <para>
        /// Return value: 1 if all info could be returned, 0 if there was a buffer
        /// overrun.
        /// </para>
        /// <para>
        /// WARNING: Before X-Plane 7 this routine did not properly iterate through
        /// directories. If X-Plane
        /// 6 compatibility is needed, use your own code to iterate directories.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int GetDirectoryContents(byte* inDirectoryPath, int inFirstReturn, byte* outFileNames, int inFileNameBufSize, byte** outIndices, int inIndexCount, int* outTotalFiles, int* outReturnedFiles)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(GetDirectoryContentsPtr);
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
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(byte*), typeof(int), typeof(byte*), typeof(int), typeof(byte**), typeof(int), typeof(int*), typeof(int*)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine returns a list of files in a directory (specified by a full
        /// path, no trailing : or
        /// \
        /// ). The output is returned as a list of NULL
        /// terminated strings. An index array (if specified) is filled with pointers
        /// into the strings. The last file is indicated by a zero-length string (and
        /// NULL in the indices). This routine will return 1 if you had capacity for
        /// all files or 0 if you did not. You can also skip a given number of files.
        /// </para>
        /// <para>
        /// * inDirectoryPath - a null terminated C string containing the full path to
        /// the directory with no trailing directory char.
        /// </para>
        /// <para>
        /// * inFirstReturn - the zero-based index of the first file in the directory
        /// to return. (Usually zero to fetch all in one pass.)
        /// </para>
        /// <para>
        /// * outFileNames - a buffer to receive a series of sequential null
        /// terminated C-string file names. A zero-length C string will be appended
        /// to the very end.
        /// </para>
        /// <para>
        /// * inFileNameBufSize - the size of the file name buffer in bytes.
        /// </para>
        /// <para>
        /// * outIndices - a pointer to an array of character pointers that will
        /// become an index into the directory. The last file will be followed by a
        /// NULL value. Pass NULL if you do not want indexing information.
        /// </para>
        /// <para>
        /// * inIndexCount - the max size of the index in entries.
        /// </para>
        /// <para>
        /// * outTotalFiles - if not NULL, this is filled in with the number of files
        /// in the directory.
        /// </para>
        /// <para>
        /// * outReturnedFiles - if not NULL, the number of files returned by this
        /// iteration.
        /// </para>
        /// <para>
        /// Return value: 1 if all info could be returned, 0 if there was a buffer
        /// overrun.
        /// </para>
        /// <para>
        /// WARNING: Before X-Plane 7 this routine did not properly iterate through
        /// directories. If X-Plane
        /// 6 compatibility is needed, use your own code to iterate directories.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int GetDirectoryContents(in ReadOnlySpan<char> inDirectoryPath, int inFirstReturn, byte* outFileNames, int inFileNameBufSize, byte** outIndices, int inIndexCount, int* outTotalFiles, int* outReturnedFiles)
        {
            IL.DeclareLocals(false);
            Span<byte> inDirectoryPathUtf8 = stackalloc byte[(inDirectoryPath.Length << 1) | 1];
            var inDirectoryPathPtr = Utils.ToUtf8Unsafe(inDirectoryPath, inDirectoryPathUtf8);
            return GetDirectoryContents(inDirectoryPathPtr, inFirstReturn, outFileNames, inFileNameBufSize, outIndices, inIndexCount, outTotalFiles, outReturnedFiles);
        }

        
        /// <summary>
        /// <para>
        /// Loads a data file of a given type. Paths must be relative to the X-System
        /// folder. To clear the replay, pass a NULL file name (this is only valid with
        /// replay movies, not sit files).
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int LoadDataFile(DataFileType inFileType, byte* inFilePath)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(LoadDataFilePtr);
            int result;
            IL.Push(inFileType);
            IL.Push(inFilePath);
            IL.Push(LoadDataFilePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(DataFileType), typeof(byte*)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// Loads a data file of a given type. Paths must be relative to the X-System
        /// folder. To clear the replay, pass a NULL file name (this is only valid with
        /// replay movies, not sit files).
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int LoadDataFile(DataFileType inFileType, in ReadOnlySpan<char> inFilePath)
        {
            IL.DeclareLocals(false);
            Span<byte> inFilePathUtf8 = stackalloc byte[(inFilePath.Length << 1) | 1];
            var inFilePathPtr = Utils.ToUtf8Unsafe(inFilePath, inFilePathUtf8);
            return LoadDataFile(inFileType, inFilePathPtr);
        }

        
        /// <summary>
        /// <para>
        /// Saves the current situation or replay; paths are relative to the X-System
        /// folder.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int SaveDataFile(DataFileType inFileType, byte* inFilePath)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(SaveDataFilePtr);
            int result;
            IL.Push(inFileType);
            IL.Push(inFilePath);
            IL.Push(SaveDataFilePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(DataFileType), typeof(byte*)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// Saves the current situation or replay; paths are relative to the X-System
        /// folder.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int SaveDataFile(DataFileType inFileType, in ReadOnlySpan<char> inFilePath)
        {
            IL.DeclareLocals(false);
            Span<byte> inFilePathUtf8 = stackalloc byte[(inFilePath.Length << 1) | 1];
            var inFilePathPtr = Utils.ToUtf8Unsafe(inFilePath, inFilePathUtf8);
            return SaveDataFile(inFileType, inFilePathPtr);
        }

        
        /// <summary>
        /// <para>
        /// This routine returns the revision of both X-Plane and the XPLM DLL. All
        /// versions are three-digit decimal numbers (e.g. 606 for version 6.06 of
        /// X-Plane); the current revision of the XPLM is 200 (2.00). This routine also
        /// returns the host ID of the app running us.
        /// </para>
        /// <para>
        /// The most common use of this routine is to special-case around X-Plane
        /// version-specific behavior.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetVersions(int* outXPlaneVersion, int* outXPLMVersion, HostApplicationID* outHostID)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(GetVersionsPtr);
            IL.Push(outXPlaneVersion);
            IL.Push(outXPLMVersion);
            IL.Push(outHostID);
            IL.Push(GetVersionsPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int*), typeof(int*), typeof(HostApplicationID*)));
        }

        
        /// <summary>
        /// <para>
        /// This routine returns the langauge the sim is running in.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static LanguageCode GetLanguage()
        {
            IL.DeclareLocals(false);
            Guard.NotNull(GetLanguagePtr);
            LanguageCode result;
            IL.Push(GetLanguagePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(LanguageCode)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine will attempt to find the symbol passed in the inString
        /// parameter. If the symbol is found a pointer the function is returned,
        /// othewise the function will return NULL.
        /// </para>
        /// <para>
        /// You can use XPLMFindSymbol to utilize newer SDK API features without
        /// requiring newer versions of the SDK (and X-Plane) as your minimum X-Plane
        /// version as follows:
        /// </para>
        /// <para>
        /// * Define the XPLMnnn macro to the minimum required XPLM version you will
        /// ship with (e.g. XPLM210 for X-Plane 10 compatibility).
        /// </para>
        /// <para>
        /// * Use XPLMGetVersions and XPLMFindSymbol to detect that the host sim is
        /// new enough to use new functions and resolve function pointers.
        /// </para>
        /// <para>
        /// * Conditionally use the new functions if and only if XPLMFindSymbol only
        /// returns a non- NULL pointer.
        /// </para>
        /// <para>
        /// Warning: you should always check the XPLM API version as well as the
        /// results of XPLMFindSymbol to determine if funtionality is safe to use.
        /// </para>
        /// <para>
        /// To use functionality via XPLMFindSymbol you will need to copy your own
        /// definitions of the X-Plane API prototypes and cast the returned pointer to
        /// the correct type.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void* FindSymbol(byte* inString)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(FindSymbolPtr);
            void* result;
            IL.Push(inString);
            IL.Push(FindSymbolPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void*), typeof(byte*)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine will attempt to find the symbol passed in the inString
        /// parameter. If the symbol is found a pointer the function is returned,
        /// othewise the function will return NULL.
        /// </para>
        /// <para>
        /// You can use XPLMFindSymbol to utilize newer SDK API features without
        /// requiring newer versions of the SDK (and X-Plane) as your minimum X-Plane
        /// version as follows:
        /// </para>
        /// <para>
        /// * Define the XPLMnnn macro to the minimum required XPLM version you will
        /// ship with (e.g. XPLM210 for X-Plane 10 compatibility).
        /// </para>
        /// <para>
        /// * Use XPLMGetVersions and XPLMFindSymbol to detect that the host sim is
        /// new enough to use new functions and resolve function pointers.
        /// </para>
        /// <para>
        /// * Conditionally use the new functions if and only if XPLMFindSymbol only
        /// returns a non- NULL pointer.
        /// </para>
        /// <para>
        /// Warning: you should always check the XPLM API version as well as the
        /// results of XPLMFindSymbol to determine if funtionality is safe to use.
        /// </para>
        /// <para>
        /// To use functionality via XPLMFindSymbol you will need to copy your own
        /// definitions of the X-Plane API prototypes and cast the returned pointer to
        /// the correct type.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void* FindSymbol(in ReadOnlySpan<char> inString)
        {
            IL.DeclareLocals(false);
            Span<byte> inStringUtf8 = stackalloc byte[(inString.Length << 1) | 1];
            var inStringPtr = Utils.ToUtf8Unsafe(inString, inStringUtf8);
            return FindSymbol(inStringPtr);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static void SetErrorCallbackPrivate(IntPtr inCallback)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(SetErrorCallbackPtr);
            IL.Push(inCallback);
            IL.Push(SetErrorCallbackPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(IntPtr)));
        }

        
        /// <summary>
        /// <para>
        /// XPLMSetErrorCallback installs an error-reporting callback for your plugin.
        /// Normally the plugin system performs minimum diagnostics to maximize
        /// performance. When you install an error callback, you will receive calls due
        /// to certain plugin errors, such as passing bad parameters or incorrect data.
        /// </para>
        /// <para>
        /// Important: the error callback determines *programming* errors, e.g. bad API
        /// parameters. Every error that is returned by the error callback represents a
        /// mistake in your plugin that you should fix. Error callbacks are not used to
        /// report expected run-time problems (e.g. disk I/O errors).
        /// </para>
        /// <para>
        /// The intention is for you to install the error callback during debug
        /// sections and put a break-point inside your callback. This will cause you to
        /// break into the debugger from within the SDK at the point in your plugin
        /// where you made an illegal call.
        /// </para>
        /// <para>
        /// Installing an error callback may activate error checking code that would
        /// not normally run, and this may adversely affect performance, so do not
        /// leave error callbacks installed in shipping plugins. Since the only useful
        /// response to an error is to change code, error callbacks are not useful "in
        /// the field".
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetErrorCallback(ErrorCallback inCallback)
        {
            IL.DeclareLocals(false);
            IntPtr inCallbackPtr = inCallback != null ? Marshal.GetFunctionPointerForDelegate(inCallback) : default;
            SetErrorCallbackPrivate(inCallbackPtr);
            GC.KeepAlive(inCallback);
        }

        
        /// <summary>
        /// <para>
        /// This routine outputs a C-style string to the Log.txt file. The file is
        /// immediately flushed so you will not lose data. (This does cause a
        /// performance penalty.)
        /// </para>
        /// <para>
        /// Please do *not* leave routine diagnostic logging enabled in your shipping
        /// plugin. The X-Plane Log file is shared by X-Plane and every plugin in the
        /// system, and plugins that (when functioning normally) print verbose log
        /// output make it difficult for developers to find error conditions from other
        /// parts of the system.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void DebugString(byte* inString)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(DebugStringPtr);
            IL.Push(inString);
            IL.Push(DebugStringPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(byte*)));
        }

        
        /// <summary>
        /// <para>
        /// This routine outputs a C-style string to the Log.txt file. The file is
        /// immediately flushed so you will not lose data. (This does cause a
        /// performance penalty.)
        /// </para>
        /// <para>
        /// Please do *not* leave routine diagnostic logging enabled in your shipping
        /// plugin. The X-Plane Log file is shared by X-Plane and every plugin in the
        /// system, and plugins that (when functioning normally) print verbose log
        /// output make it difficult for developers to find error conditions from other
        /// parts of the system.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void DebugString(in ReadOnlySpan<char> inString)
        {
            IL.DeclareLocals(false);
            Span<byte> inStringUtf8 = stackalloc byte[(inString.Length << 1) | 1];
            var inStringPtr = Utils.ToUtf8Unsafe(inString, inStringUtf8);
            DebugString(inStringPtr);
        }

        
        /// <summary>
        /// <para>
        /// This function displays the string in a translucent overlay over the current
        /// display and also speaks the string if text-to-speech is enabled. The string
        /// is spoken asynchronously, this function returns immediately. This function
        /// may not speak or print depending on user preferences.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SpeakString(byte* inString)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(SpeakStringPtr);
            IL.Push(inString);
            IL.Push(SpeakStringPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(byte*)));
        }

        
        /// <summary>
        /// <para>
        /// This function displays the string in a translucent overlay over the current
        /// display and also speaks the string if text-to-speech is enabled. The string
        /// is spoken asynchronously, this function returns immediately. This function
        /// may not speak or print depending on user preferences.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SpeakString(in ReadOnlySpan<char> inString)
        {
            IL.DeclareLocals(false);
            Span<byte> inStringUtf8 = stackalloc byte[(inString.Length << 1) | 1];
            var inStringPtr = Utils.ToUtf8Unsafe(inString, inStringUtf8);
            SpeakString(inStringPtr);
        }

        
        /// <summary>
        /// <para>
        /// Given a virtual key code (as defined in XPLMDefs.h) this routine returns a
        /// human-readable string describing the character. This routine is provided
        /// for showing users what keyboard mappings they have set up. The string may
        /// read 'unknown' or be a blank or NULL string if the virtual key is unknown.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe byte* GetVirtualKeyDescription(byte inVirtualKey)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(GetVirtualKeyDescriptionPtr);
            byte* result;
            IL.Push(inVirtualKey);
            IL.Push(GetVirtualKeyDescriptionPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(byte*), typeof(byte)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// XPLMReloadScenery reloads the current set of scenery. You can use this
        /// function in two typical ways: simply call it to reload the scenery, picking
        /// up any new installed scenery, .env files, etc. from disk. Or, change the
        /// lat/ref and lon/ref data refs and then call this function to shift the
        /// scenery environment.  This routine is equivalent to picking "reload
        /// scenery" from the developer menu.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void ReloadScenery()
        {
            IL.DeclareLocals(false);
            Guard.NotNull(ReloadSceneryPtr);
            IL.Push(ReloadSceneryPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void)));
        }

        
        /// <summary>
        /// <para>
        /// XPLMFindCommand looks up a command by name, and returns its command
        /// reference or NULL if the command does not exist.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe CommandRef FindCommand(byte* inName)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(FindCommandPtr);
            CommandRef result;
            IL.Push(inName);
            IL.Push(FindCommandPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(CommandRef), typeof(byte*)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// XPLMFindCommand looks up a command by name, and returns its command
        /// reference or NULL if the command does not exist.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe CommandRef FindCommand(in ReadOnlySpan<char> inName)
        {
            IL.DeclareLocals(false);
            Span<byte> inNameUtf8 = stackalloc byte[(inName.Length << 1) | 1];
            var inNamePtr = Utils.ToUtf8Unsafe(inName, inNameUtf8);
            return FindCommand(inNamePtr);
        }

        
        /// <summary>
        /// <para>
        /// XPLMCommandBegin starts the execution of a command, specified by its
        /// command reference. The command is "held down" until XPLMCommandEnd is
        /// called.  You must balance each XPLMCommandBegin call with an XPLMCommandEnd
        /// call.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void CommandBegin(CommandRef inCommand)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(CommandBeginPtr);
            IL.Push(inCommand);
            IL.Push(CommandBeginPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(CommandRef)));
        }

        
        /// <summary>
        /// <para>
        /// XPLMCommandEnd ends the execution of a given command that was started with
        /// XPLMCommandBegin.  You must not issue XPLMCommandEnd for a command you did
        /// not begin.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void CommandEnd(CommandRef inCommand)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(CommandEndPtr);
            IL.Push(inCommand);
            IL.Push(CommandEndPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(CommandRef)));
        }

        
        /// <summary>
        /// <para>
        /// This executes a given command momentarily, that is, the command begins and
        /// ends immediately. This is the equivalent of calling XPLMCommandBegin() and
        /// XPLMCommandEnd() back ot back.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void CommandOnce(CommandRef inCommand)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(CommandOncePtr);
            IL.Push(inCommand);
            IL.Push(CommandOncePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(CommandRef)));
        }

        
        /// <summary>
        /// <para>
        /// XPLMCreateCommand creates a new command for a given string. If the command
        /// already exists, the existing command reference is returned. The description
        /// may appear in user interface contexts, such as the joystick configuration
        /// screen.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe CommandRef CreateCommand(byte* inName, byte* inDescription)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(CreateCommandPtr);
            CommandRef result;
            IL.Push(inName);
            IL.Push(inDescription);
            IL.Push(CreateCommandPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(CommandRef), typeof(byte*), typeof(byte*)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// XPLMCreateCommand creates a new command for a given string. If the command
        /// already exists, the existing command reference is returned. The description
        /// may appear in user interface contexts, such as the joystick configuration
        /// screen.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe CommandRef CreateCommand(in ReadOnlySpan<char> inName, in ReadOnlySpan<char> inDescription)
        {
            IL.DeclareLocals(false);
            Span<byte> inNameUtf8 = stackalloc byte[(inName.Length << 1) | 1];
            var inNamePtr = Utils.ToUtf8Unsafe(inName, inNameUtf8);
            Span<byte> inDescriptionUtf8 = stackalloc byte[(inDescription.Length << 1) | 1];
            var inDescriptionPtr = Utils.ToUtf8Unsafe(inDescription, inDescriptionUtf8);
            return CreateCommand(inNamePtr, inDescriptionPtr);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static unsafe void RegisterCommandHandlerPrivate(CommandRef inComand, IntPtr inHandler, int inBefore, void* inRefcon)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(RegisterCommandHandlerPtr);
            IL.Push(inComand);
            IL.Push(inHandler);
            IL.Push(inBefore);
            IL.Push(inRefcon);
            IL.Push(RegisterCommandHandlerPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(CommandRef), typeof(IntPtr), typeof(int), typeof(void*)));
        }

        
        /// <summary>
        /// <para>
        /// XPLMRegisterCommandHandler registers a callback to be called when a command
        /// is executed. You provide a callback with a reference pointer.
        /// </para>
        /// <para>
        /// If inBefore is true, your command handler callback will be executed before
        /// X-Plane executes the command, and returning 0 from your callback will
        /// disable X-Plane's processing of the command. If inBefore is false, your
        /// callback will run after X-Plane. (You can register a single callback both
        /// before and after a command.)
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void RegisterCommandHandler(CommandRef inComand, CommandCallback inHandler, int inBefore, void* inRefcon)
        {
            IL.DeclareLocals(false);
            IntPtr inHandlerPtr = inHandler != null ? Marshal.GetFunctionPointerForDelegate(inHandler) : default;
            RegisterCommandHandlerPrivate(inComand, inHandlerPtr, inBefore, inRefcon);
            GC.KeepAlive(inHandler);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static unsafe void UnregisterCommandHandlerPrivate(CommandRef inComand, IntPtr inHandler, int inBefore, void* inRefcon)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(UnregisterCommandHandlerPtr);
            IL.Push(inComand);
            IL.Push(inHandler);
            IL.Push(inBefore);
            IL.Push(inRefcon);
            IL.Push(UnregisterCommandHandlerPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(CommandRef), typeof(IntPtr), typeof(int), typeof(void*)));
        }

        
        /// <summary>
        /// <para>
        /// XPLMUnregisterCommandHandler removes a command callback registered with
        /// XPLMRegisterCommandHandler.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void UnregisterCommandHandler(CommandRef inComand, CommandCallback inHandler, int inBefore, void* inRefcon)
        {
            IL.DeclareLocals(false);
            IntPtr inHandlerPtr = inHandler != null ? Marshal.GetFunctionPointerForDelegate(inHandler) : default;
            UnregisterCommandHandlerPrivate(inComand, inHandlerPtr, inBefore, inRefcon);
            GC.KeepAlive(inHandler);
        }
    }
}