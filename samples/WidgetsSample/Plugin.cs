using System;
using WidgetsSample;
using XP.SDK;
using XP.SDK.Widgets;
using XP.SDK.Widgets.Behaviors;
using XP.SDK.XPLM;

[assembly: Plugin(typeof(Plugin))]

namespace WidgetsSample
{

    public class Plugin : PluginBase
    {
        private MainWindow _mainWindow;
        private MenuItem _visibilityItem;
        private ProgressBar _progressBar;

        protected override bool OnStart()
        {
            Features.EnableFeature(Features.UseNativeWidgetWindows);

            Menu.PluginsMenu
                .AddItem("Widgets Sample", item =>
                {
                    item.GetOrCreateSubMenu()
                        .AddItem("Main Window Visible", out _visibilityItem, OnVisibilityItemClick);
                });

            _visibilityItem.CheckState = MenuCheck.Checked;

            _mainWindow = new MainWindow(new Rect(0, 600, 800, 0), descriptor: "Widgets Sample")
            {
                Behaviors = { FixedLayoutBehavior.Shared },
                HasCloseBoxes = true,
                Children =
                {
                    new PushButton(new Rect(40, 80, 140, 40), "Push Button"),
                    new CheckBox(new Rect(160, 80, 260, 40), "Check Box"),
                    new RadioButton(new Rect(280, 80, 380, 40), "Radio Button 1"),
                    new RadioButton(new Rect(400, 80, 500, 40), "Radio Button 2"),

                    new Caption(new Rect(20, 240, 100, 220), "Text Field:"),
                    new TextField(new Rect(120, 240, 780, 220), "Text"),
                    new Caption(new Rect(20, 280, 100, 260), "Progress Bar:"),
                    (_progressBar = new ProgressBar(new Rect(120, 280, 780, 260)) { MinValue = 0, MaxValue = 100, Value = 50 }),
                    new Caption(new Rect(20, 320, 100, 300), "Scroll Bar:"),
                    new ScrollBar(new Rect(120, 320, 780, 300)) { MinValue = 0, MaxValue = 100, Value = 50, PageSize = 10 }
                        .AddValueChangedHandler((ScrollBar sender, ref bool handled) => _progressBar.Value = sender.Value)
                },
            }
            .AddShownHandler((Widget sender, Widget args, ref bool handled) => _visibilityItem.CheckState = MenuCheck.Checked)
            .AddHiddenHandler((Widget sender, Widget args, ref bool handled) => _visibilityItem.CheckState = MenuCheck.Unchecked)
            .AddCloseButtonPushedHandler((MainWindow sender, ref bool handled) => sender.Hide());

            return true;
        }

        private void OnVisibilityItemClick(MenuItem sender, EventArgs args)
        {
            _mainWindow.IsVisible = sender.CheckState == MenuCheck.Unchecked;
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
            _mainWindow.Destroy();
            Menu.PluginsMenu.Dispose();
        }

        protected override void OnReceiveMessage(PluginID pluginId, int message, IntPtr param)
        {
        }

        public override string Name => "Widgets Sample";
        public override string Signature => "com.fedarovich.xplane-dotnet.widgets-sample";
        public override string Description => "Shows the standard widgets";
    }
}
