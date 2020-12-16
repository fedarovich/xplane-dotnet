using System;
using System.Collections.Concurrent;
using XP.SDK.XPLM;

namespace XP.SDK
{
    public abstract class PluginBase
    {
        private static ConcurrentStack<object> _registeredObjects;

        public abstract string Name { get; }

        public abstract string Signature { get; }

        public abstract string Description { get; }

        protected PluginInfo ThisPlugin => PluginInfo.ThisPlugin;

        public bool Start()
        {
            _registeredObjects = new ConcurrentStack<object>();
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
            try
            {
                OnStop();
            }
            finally
            {
                ReleaseRegisteredObjects();
            }
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

        internal static void RegisterObject(object obj)
        {
            _registeredObjects.Push(obj);
        }

        private void ReleaseRegisteredObjects()
        {
            while (_registeredObjects.TryPop(out var obj))
            {
                (obj as IDisposable)?.Dispose();
            }

            _registeredObjects = null;
        }
    }
}
