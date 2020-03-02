using System;
using System.Collections.Generic;
using System.Text;

namespace XP.SDK
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public class PluginAttribute : Attribute
    {
        public Type PluginType { get; }
        public string Name { get; }
        public string Signature { get; }
        public string Description { get; }

        public PluginAttribute(Type pluginType, string name, string signature, string description)
        {
            PluginType = pluginType;
            Name = name;
            Signature = signature;
            Description = description;
        }
    }
}
