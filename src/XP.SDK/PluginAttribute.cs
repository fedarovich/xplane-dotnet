using System;
using System.Collections.Generic;
using System.Text;

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
