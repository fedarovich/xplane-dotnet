using System;

namespace XP.SDK
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public class PluginAttribute : Attribute
    {
        public Type PluginType { get; }

        public PluginAttribute(Type pluginType)
        {
            PluginType = pluginType;
        }
    }
}
