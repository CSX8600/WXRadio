using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WXRadio.WeatherManager;
using WXRadio.WeatherManager.Plugin;

namespace ProductServer
{
    public class ProductServerPlugin : BasePlugin
    {
        ControlPanel controlPanel;

        public ProductServerPlugin()
        {
            controlPanel = new ControlPanel();
            controlPanel.Dock = DockStyle.Fill;
            controlPanel.PortChanged += ControlPanel_PortChanged;
            controlPanel.ServerToggle += ControlPanel_ServerToggle;
            controlPanel.AutoStartChanged += ControlPanel_AutoStartChanged;
        }

        private void ControlPanel_AutoStartChanged(object sender, bool e)
        {
            ProductServerConfiguration config = GetConfigFile<ProductServerConfiguration>();
            config.AutoStart = e;
            SaveConfigFile(config);
        }

        private void ControlPanel_ServerToggle(object sender, EventArgs e)
        {
            if (ProductServer.IsRunning)
            {
                ProductServer.Stop();
                controlPanel.IsStarted = false;
            }
            else
            {
                ProductServerConfiguration config = GetConfigFile<ProductServerConfiguration>();
                ProductServer.Start(config.ServerPort);
                controlPanel.IsStarted = true;
            }
        }

        private void ControlPanel_PortChanged(object sender, int e)
        {
            ProductServerConfiguration config = GetConfigFile<ProductServerConfiguration>();
            config.ServerPort = e;
            SaveConfigFile(config);
        }

        public override string PluginID => "productserver";

        public override string FriendlyName => "Advisory Product Server";

        public override Control GetDashboardControl()
        {
            return controlPanel;
        }

        public override void PostInitialize()
        {
            base.PostInitialize();
            ProductServerConfiguration config = GetConfigFile<ProductServerConfiguration>();
            controlPanel.Port = config.ServerPort;
            controlPanel.AutoStart = config.AutoStart;

            if (config.AutoStart)
            {
                ProductServer.Start(config.ServerPort);
                controlPanel.IsStarted = true;
            }
        }

        public override void Shutdown()
        {
            base.Shutdown();
            ProductServer.Stop();
        }
    }
}
