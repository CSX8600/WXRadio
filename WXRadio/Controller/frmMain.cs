using System;
using System.Windows.Forms;
using WXRadio.WeatherManager;
using WXRadio.WeatherManager.Plugin;

namespace WXRadio.WXController
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void cmdExit_Click(object sender, EventArgs e)
        {
            PluginManager.INSTANCE.Shutdown();

            Application.Exit();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            PluginManager pluginManager = PluginManager.INSTANCE;
            pluginManager.Initialize();

            foreach (BasePlugin plugin in pluginManager.GetPlugins())
            {
                GroupBox groupBox = new GroupBox();
                groupBox.Text = plugin.FriendlyName;
                groupBox.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
                Control ctrl = plugin.GetDashboardControl();
                ctrl.Dock = DockStyle.Fill;
                groupBox.Controls.Add(ctrl);
                tblPlugins.Controls.Add(groupBox);
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            PluginManager.INSTANCE.Shutdown();
        }
    }
}
