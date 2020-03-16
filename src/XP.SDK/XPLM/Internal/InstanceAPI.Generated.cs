using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using InlineIL;

namespace XP.SDK.XPLM.Internal
{
    public static partial class InstanceAPI
    {
        private static IntPtr CreateInstancePtr;
        private static IntPtr DestroyInstancePtr;
        private static IntPtr InstanceSetPositionPtr;

        static InstanceAPI()
        {
            CreateInstancePtr = Lib.GetExport("XPLMCreateInstance");
            DestroyInstancePtr = Lib.GetExport("XPLMDestroyInstance");
            InstanceSetPositionPtr = Lib.GetExport("XPLMInstanceSetPosition");
        }

        
        /// <summary>
        /// <para>
        /// Registers an instance of an X-Plane object.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe InstanceRef CreateInstance(ObjectRef obj, byte** datarefs)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(CreateInstancePtr);
            InstanceRef result;
            IL.Push(obj);
            IL.Push(datarefs);
            IL.Push(CreateInstancePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(InstanceRef), typeof(ObjectRef), typeof(byte**)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// Unregisters an instance.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void DestroyInstance(InstanceRef instance)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(DestroyInstancePtr);
            IL.Push(instance);
            IL.Push(DestroyInstancePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(InstanceRef)));
        }

        
        /// <summary>
        /// <para>
        /// Updates both the position of the instance and all datarefs you registered
        /// for it.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void InstanceSetPosition(InstanceRef instance, DrawInfo* new_position, float* data)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(InstanceSetPositionPtr);
            IL.Push(instance);
            IL.Push(new_position);
            IL.Push(data);
            IL.Push(InstanceSetPositionPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(InstanceRef), typeof(DrawInfo*), typeof(float*)));
        }
    }
}