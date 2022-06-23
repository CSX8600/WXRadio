using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WXRadio.WeatherManager.Plugin;

namespace WXReceiver
{
    public class WXReceiverPlugin : BasePlugin
    {
        private bool isReceiving;
        private static DashboardControl DashboardControl = new DashboardControl();
        private static ReceiverThread receiverThread;
        public override string PluginID => "receiver";

        public override string FriendlyName => "Weather Receiver";

        public override Control GetDashboardControl()
        {
            return DashboardControl;
        }

        public override void PreInitialize()
        {
            receiverThread = new ReceiverThread(this);
            DashboardControl.StartStopReceivingEvent += DashboardControl_StartStopReceivingEvent;
        }

        private void DashboardControl_StartStopReceivingEvent(object sender, DashboardControl.StartStopEventArgs e)
        {
            if ((e.IsStarting && isReceiving) || (!e.IsStarting && !isReceiving))
            {
                e.StartStopSuccessful = true;
                return;
            }

            if (e.IsStarting)
            {
                Config config = GetConfigFile<Config>();
                try
                {
                    receiverThread.Start(config.StreamAddress, config.StreamPort);
                }
                catch(Exception)
                {
                    e.StartStopSuccessful = false;
                    return;
                }
            }

            if (!e.IsStarting)
            {
                try
                {
                    receiverThread.Abort();
                }
                catch(Exception)
                {
                    e.StartStopSuccessful = false;
                }
            }

            e.StartStopSuccessful = true;
        }

        public void ThreadException(Exception ex)
        {
            isReceiving = false;
            DashboardControl.SetStatus("An error occurred - stopped", true);
            DashboardControl.SetStartStop(false, true);
            DashboardControl.Invoke(new MethodInvoker(() => MessageBox.Show("An error occurred:\n" + ex.ToString())));
        }

        public override void Shutdown()
        {
            receiverThread.Abort();
        }
    }
}
