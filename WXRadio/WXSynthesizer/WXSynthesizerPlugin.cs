using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WXRadio.WeatherManager;
using WXRadio.WeatherManager.Plugin;
using WXRadio.WeatherManager.Product;

namespace WXRadio.WXSynthesizer
{
    public class WXSynthesizerPlugin : BasePlugin
    {
        Button dashboardControl;
        frmRadioInterface radioInterface;
        public override string PluginID => "synthesizer";

        public override string FriendlyName => "Synthesizer";

        public override void PreInitialize()
        {
            // Initialize fonts
            PrivateFontCollection collection = new PrivateFontCollection();
            InitFont(Properties.Resources.digital_7, collection);
            InitFont(Properties.Resources.digital_7__italic_, collection);
            InitFont(Properties.Resources.digital_7__mono_, collection);
            InitFont(Properties.Resources.digital_7__mono_italic_, collection);
        }

        private void InitFont(byte[] fontData, PrivateFontCollection pfc)
        {
            int fontLength = fontData.Length;
            IntPtr pointer = Marshal.AllocCoTaskMem(fontLength);
            Marshal.Copy(fontData, 0, pointer, fontLength);
            pfc.AddMemoryFont(pointer, fontLength);
        }

        public override void Initialize()
        {
            dashboardControl = new Button();
            dashboardControl.Text = "Show Radio";
            dashboardControl.Dock = DockStyle.Fill;
            dashboardControl.Click += ShowRadio_Click;

            ProductManager.INSTANCE.ProductAdded += ProductAdded;
        }

        public override void PostInitialize()
        {
            Config config = GetConfigFile<Config>();
            config.CoopControl = dashboardControl.Handle;

            radioInterface = new frmRadioInterface(config, cfg => SaveConfigFile(cfg), () => dashboardControl.Handle);

            dashboardControl.Invoke(new MethodInvoker(() => Synthesizer.INSTANCE.Start(config)));
        }

        private void ShowRadio_Click(object sender, EventArgs e)
        {
            radioInterface.Show();
        }

        private void ProductAdded(object sender, BaseProduct e)
        {
            if (e is IEmergency && ((IEmergency)e).QueueForImmediateBroadcast())
            {
                if (ProductManager.INSTANCE.GetProducts().Any(p => p != e && p is IEmergency && ((IEmergency)p).QueueForImmediateBroadcast()))
                {
                    return;
                }

                Synthesizer.INSTANCE.ConsiderRestart(e as IEmergency);
            }
        }

        public override Control GetDashboardControl()
        {
            return dashboardControl;
        }

        public override void Shutdown()
        {
            Synthesizer.INSTANCE.Stop();
        }
    }
}
