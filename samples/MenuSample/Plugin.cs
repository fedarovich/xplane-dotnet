using System;
using MenuSample;
using XP.SDK;
using XP.SDK.XPLM;

[assembly: Plugin(typeof(Plugin))]

namespace MenuSample
{
    public sealed class Plugin : PluginBase
    {
        private Menu _menu;
        private MenuItem _toggleShortcuts;

        protected override bool OnStart()
        {
            Menu.PluginsMenu
                .AddItem("Sample Menu", item =>
                {
                    _menu = item.CreateSubMenu()
                        .AddItem("Toggle Settings", out _, OnToggleSettings)
                        .AddSeparator()
                        .AddItem("Toggle Shortcuts", out _toggleShortcuts)
                        .AddItem("Toggle Flight Configuration (Command-Based)", CommandRef.Find("sim/operation/toggle_flight_config"));
                });
            _menu.Click += OnMenuClick;

            if (Menu.AircraftMenu != null)
            {
                // This will be null unless this plugin was loaded with an aircraft (i.e., it was located in the current aircraft's "plugins" subdirectory)
                Menu.AircraftMenu
                    .AddItem("Toggle Settings (Command-Based)", CommandRef.Find("sim/operation/toggle_settings_window"));
            }

            return true;
        }

        private void OnToggleSettings(MenuItem sender, EventArgs args)
        {
            // You can associate a handler with a particular menu item
            // using AddItem()'s argument or by subscribing to MenuItem.Click event, ....
            Command.Find("sim/operation/toggle_settings_window")?.Once();
        }

        private void OnMenuClick(Menu sender, MenuItem item)
        {
            // ... or you can use a single handler for all items
            // by subscribing to Menu.Click event.
            if (item == _toggleShortcuts)
            {
                Command.Find("sim/operation/toggle_key_shortcuts_window")?.Once();
            }
        }

        protected override void OnStop()
        {
            Menu.PluginsMenu.Dispose();
        }

        protected override bool OnEnable() => true;

        protected override void OnDisable()
        {
        }

        protected override void OnReceiveMessage(PluginID pluginId, int message, IntPtr param)
        {
        }

        public override string Name => "Menu Sample";
        public override string Signature => "com.fedarovich.xplane-dotnet.menu-sample";
        public override string Description => "A sample plug-in that demonstrates and exercises the X-Plane menu API.";
    }
}
