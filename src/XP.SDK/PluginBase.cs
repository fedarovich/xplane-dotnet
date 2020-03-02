using System;
using System.Collections.Generic;
using System.Text;
using XP.SDK.XPLM.Internal;

namespace XP.SDK
{
    public abstract class PluginBase
    {
        public abstract string Name { get; }

        public abstract string Signature { get; }

        public abstract string Description { get; }

        public bool Start()
        {
            return OnStart();
        }

        public bool Enable()
        {
            return OnEnable();
        }

        public void Disable()
        {
            OnDisable();
        }

        public void Stop()
        {
            OnStop();
        }

        public void ReceiveMessage(PluginID pluginId, int message, IntPtr param)
        {
            OnReceiveMessage(pluginId, message, param);
        }

        protected abstract bool OnStart();

        protected abstract bool OnEnable();

        protected abstract void OnDisable();

        protected abstract void OnStop();

        protected abstract void OnReceiveMessage(PluginID pluginId, int message, IntPtr param);
    }
}
