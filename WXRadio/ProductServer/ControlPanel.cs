using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductServer
{
    public partial class ControlPanel : UserControl
    {
        public event EventHandler<int> PortChanged;
        public event EventHandler ServerToggle;
        public event EventHandler<bool> AutoStartChanged;

        private bool _isStarted;
        public bool IsStarted
        {
            get => _isStarted;
            set
            {
                _isStarted = value;
                cmdStart.Text = value ? "Stop Server" : "Start Server";
                numPort.Enabled = !value;
            }
        }

        public int Port
        {
            get => (int)numPort.Value;
            set
            {
                _suppressEvents = true;
                numPort.Value = value;
                _suppressEvents = false;
            }
        }

        public bool AutoStart
        {
            get => chkAutoStart.Checked;
            set
            {
                _suppressEvents = true;
                chkAutoStart.Checked = value;
                _suppressEvents = false;
            }
        }

        public ControlPanel()
        {
            InitializeComponent();
        }

        private bool _suppressEvents = false;
        private void numPort_ValueChanged(object sender, EventArgs e)
        {
            if (_suppressEvents) return;
            PortChanged?.Invoke(this, (int)numPort.Value);
        }

        private void cmdStart_Click(object sender, EventArgs e)
        {
            if (_suppressEvents) return;
            ServerToggle?.Invoke(this, EventArgs.Empty);
        }

        private void chkAutoStart_CheckedChanged(object sender, EventArgs e)
        {
            if (_suppressEvents) return;
            AutoStartChanged?.Invoke(this, chkAutoStart.Checked);
        }
    }
}
