using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using WXRadio.WeatherManager.Plugin;

namespace StandardAdvisoryPack
{
    public class StandardAdvisoryPackPlugin : BasePlugin
    {
        StormMonitor stormMonitor;
        public override string PluginID => "standard_advisory_pack";

        public override string FriendlyName => "Standard Advisory Pack";

        public override void Initialize()
        {
            stormMonitor = new StormMonitor();
        }

        public override Control GetDashboardControl()
        {
            return new Label() { Text = "Installed!", Dock = DockStyle.Fill };
        }
    }
}
