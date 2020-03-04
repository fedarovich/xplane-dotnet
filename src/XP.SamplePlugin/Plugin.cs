using System;
using XP.SDK;
using XP.SDK.XPLM.Internal;

[assembly: Plugin(typeof(XP.SamplePlugin.Plugin))]

namespace XP.SamplePlugin
{

    public class Plugin : PluginBase
    {
        public override string Name => "Sample";
        public override string Signature => "com.fedarovich.xplane-dotnet.sample";
        public override string Description => "Sample plugin.";
        
        protected override bool OnStart()
        {
            Utilities.DebugString("Start sample plugin.");
            return true;
        }

        protected override bool OnEnable()
        {
            Utilities.DebugString("Enable sample plugin.");
            return true;
        }

        protected override void OnDisable()
        {
            Utilities.DebugString("Disable sample plugin.");
        }

        protected override void OnStop()
        {
            Utilities.DebugString("Stop sample plugin.");
        }

        protected override void OnReceiveMessage(PluginID pluginId, int message, IntPtr param)
        {
            Utilities.DebugString($"Received message {message} from plugin {pluginId} with payload 0x{param.ToInt64():X8}.");
        }
    }
}
