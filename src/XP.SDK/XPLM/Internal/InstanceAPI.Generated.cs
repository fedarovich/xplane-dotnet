using System;
using System.Runtime.InteropServices;
using InlineIL;

namespace XP.SDK.XPLM.Internal
{
    public static partial class InstanceAPI
    {
        
        /// <summary>
        /// <para>
        /// XPLMCreateInstance creates a new instance, managed by your plug-in, and
        /// returns a handle to the instance. A few important requirements:
        /// </para>
        /// <para>
        /// * The object passed in must be fully loaded and returned from the XPLM
        /// before you can create your instance; you cannot pass a null obj ref, nor
        /// can you change the ref later.
        /// </para>
        /// <para>
        /// * If you use any custom datarefs in your object, they must be registered
        /// before the object is loaded. This is true even if their data will be
        /// provided via the instance dataref list.
        /// </para>
        /// <para>
        /// * The instance dataref array must be a valid ptr to an array of at least
        /// one item that is null terminated.  That is, if you do not want any
        /// datarefs, you must passa ptr to an array with a null item.  You cannot
        /// pass null for this.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMCreateInstance", ExactSpelling = true)]
        public static extern unsafe InstanceRef CreateInstance(ObjectRef obj, byte** datarefs);

        
        /// <summary>
        /// <para>
        /// XPLMDestroyInstance destroys and deallocates your instance; once called,
        /// you are still responsible for releasing the OBJ ref.
        /// </para>
        /// <para>
        /// Tip: you can release your OBJ ref after you call XPLMCreateInstance as long
        /// as you never use it again; the instance will maintain its own reference to
        /// the OBJ and the object OBJ be deallocated when the instance is destroyed.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMDestroyInstance", ExactSpelling = true)]
        public static extern void DestroyInstance(InstanceRef instance);

        
        /// <summary>
        /// <para>
        /// Updates both the position of the instance and all datarefs you registered
        /// for it.  Call this from a flight loop callback or UI callback.
        /// </para>
        /// <para>
        /// __DO NOT__ call XPLMInstanceSetPosition from a drawing callback; the whole
        /// point of instancing is that you do not need any drawing callbacks. Setting
        /// instance data from a drawing callback may have undefined consequences, and
        /// the drawing callback hurts FPS unnecessarily.
        /// </para>
        /// <para>
        /// The memory pointed to by the data pointer must be large enough to hold one
        /// float for every data ref you have registered, and must contain valid
        /// floating point data.
        /// </para>
        /// <para>
        /// BUG: before X-Plane 11.50, if you have no dataref registered, you must
        /// still pass a valid pointer for data and not null.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMInstanceSetPosition", ExactSpelling = true)]
        public static extern unsafe void InstanceSetPosition(InstanceRef instance, DrawInfo* new_position, float* data);
    }
}