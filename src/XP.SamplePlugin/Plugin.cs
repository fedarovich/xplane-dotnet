using System;
using XP.SDK;
using XP.SDK.XPLM.Internal;

[assembly: Plugin(typeof(Plugin))]

namespace XP.SamplePlugin
{

    public class Plugin : PluginBase
    {
        public override string Name => "Sample";
        public override string Signature => "com.fedarovich.xplane-dotnet.sample";
        public override string Description => "Sample plugin.";
        
        protected override bool OnStart()
        {
            return true;
        }

        protected override bool OnEnable()
        {
            return true;
        }

        protected override void OnDisable()
        {
        }

        protected override void OnStop()
        {
        }

        protected override void OnReceiveMessage(PluginID pluginId, int message, IntPtr param)
        {
        }
    }
}
