using System.Windows.Forms;
using WXRadio.WeatherManager.Plugin;

namespace AdvisoryDisplay
{
    public class AdvisoryDisplayPlugin : BasePlugin
    {
        AdvisoryDisplayControl _advisoryDisplayControl;
        public override string PluginID => "advisory_display";

        public override string FriendlyName => "Advisory Display";

        public override void Initialize()
        {
            Config config = GetConfigFile<Config>();
            _advisoryDisplayControl = new AdvisoryDisplayControl(config);
        }

        public override Control GetDashboardControl()
        {
            return _advisoryDisplayControl;
        }
    }
}
