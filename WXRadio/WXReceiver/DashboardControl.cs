using System;
using System.Windows.Forms;

namespace WXReceiver
{
    public partial class DashboardControl : UserControl
    {
        public event EventHandler<StartStopEventArgs> StartStopReceivingEvent;
        private bool hasStarted;

        internal DashboardControl()
        {
            InitializeComponent();
        }

        private void cmdStartStop_Click(object sender, EventArgs e)
        {
            SetButtonsEnabled(false);

            if (hasStarted)
            {
                StopReceiving();
            }
            else
            {
                StartReceiving();
            }
        }

        private void StartReceiving()
        {
            SetStatus("Starting RX...");

            StartStopEventArgs startStopEventArgs = new StartStopEventArgs() { IsStarting = true };
            StartStopReceivingEvent?.Invoke(this, startStopEventArgs);

            if (!startStopEventArgs.StartStopSuccessful)
            {
                MessageBox.Show("Could not start receiving.  Check Configuration and try again.");
                SetStatus("Stopped");
                SetStartStop(false);
                return;
            }

            SetStatus("Ready");
            SetStartStop(true);
        }

        private void StopReceiving()
        {
            SetStatus("Stopping RX...");

            StartStopEventArgs startStopEventArgs = new StartStopEventArgs() { IsStarting = false };
            StartStopReceivingEvent?.Invoke(this, startStopEventArgs);

            if (!startStopEventArgs.StartStopSuccessful)
            {
                MessageBox.Show("Could not stop receiving.  Check Configuration and try again.");
                SetStatus("Ready");
                SetStartStop(true);
                return;
            }

            SetStatus("Stopped");
            SetStartStop(false);
        }

        internal void SetStatus(string status, bool notThreadSafe = false)
        {
            Action action = new Action(() => lblStatus.Text = status);

            if (notThreadSafe)
            {
                Invoke(new MethodInvoker(action));
                return;
            }

            action();
        }

        private void SetButtonsEnabled(bool enabled)
        {
            cmdStartStop.Enabled = enabled;
        }

        internal void SetStartStop(bool isStarted, bool notThreadSafe = false)
        {
            Action action = new Action(() =>
            {
                hasStarted = isStarted;
                if (isStarted)
                {
                    SetButtonsEnabled(false);
                    cmdStartStop.Enabled = true;
                    cmdStartStop.Text = "Stop RX";
                }
                else
                {
                    SetButtonsEnabled(true);
                    cmdStartStop.Text = "Start RX";
                }
            });

            if (notThreadSafe)
            {
                Invoke(new MethodInvoker(action));
            }
            else
            {
                action();
            }
        }

        public class StartStopEventArgs : EventArgs
        {
            public bool IsStarting { get; set; }
            public bool StartStopSuccessful { get; set; }
        }
    }
}
