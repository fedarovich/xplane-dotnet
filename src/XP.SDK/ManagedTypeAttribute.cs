using System;

namespace XP.SDK
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class ManagedTypeAttribute : Attribute
    {
        public ManagedTypeAttribute(Type managedType)
        {
            ManagedType = managedType;
        }

        public Type ManagedType { get; }
    }
}
