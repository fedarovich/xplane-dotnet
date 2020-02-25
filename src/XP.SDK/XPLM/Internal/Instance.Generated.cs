using InlineIL;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Internal
{
    public static partial class Instance
    {
        private static IntPtr CreateInstancePtr;
        private static IntPtr DestroyInstancePtr;
        private static IntPtr InstanceSetPositionPtr;
        static Instance()
        {
            const string libraryName = "XPLM";
            CreateInstancePtr = FunctionResolver.Resolve(libraryName, "XPLMCreateInstance");
            DestroyInstancePtr = FunctionResolver.Resolve(libraryName, "XPLMDestroyInstance");
            InstanceSetPositionPtr = FunctionResolver.Resolve(libraryName, "XPLMInstanceSetPosition");
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe InstanceRef CreateInstance(ObjectRef obj, byte **datarefs)
        {
            IL.DeclareLocals(false);
            InstanceRef result;
            IL.Push(obj);
            IL.Push(datarefs);
            IL.Push(CreateInstancePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(InstanceRef), typeof(ObjectRef), typeof(byte **)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void DestroyInstance(InstanceRef instance)
        {
            IL.DeclareLocals(false);
            IL.Push(instance);
            IL.Push(DestroyInstancePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(InstanceRef)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void InstanceSetPosition(InstanceRef instance, DrawInfo*new_position, float *data)
        {
            IL.DeclareLocals(false);
            IL.Push(instance);
            IL.Push(new_position);
            IL.Push(data);
            IL.Push(InstanceSetPositionPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(InstanceRef), typeof(DrawInfo*), typeof(float *)));
        }
    }
}