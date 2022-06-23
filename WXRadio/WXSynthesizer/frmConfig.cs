using Microsoft.DirectX.DirectSound;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WXRadio.WXSynthesizer
{
    public partial class frmConfig : Form
    {
        private Config config;
        internal frmConfig(Config config) : this()
        {
            this.config = config;

            using (SpeechSynthesizer synth = new SpeechSynthesizer())
            {
                foreach(InstalledVoice voice in synth.GetInstalledVoices())
                {
                    cboVoice.Items.Add(voice.VoiceInfo.Name);
                }
            }

            foreach (DeviceInformation deviceInformation in new DevicesCollection())
            {
                cboOutputDevice.Items.Add(deviceInformation.Description);
            }

            cboVoice.Text = config.VoiceName;
            cboOutputDevice.Text = config.DeviceName;
        }

        protected frmConfig()
        {
            InitializeComponent();
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            config.VoiceName = cboVoice.Text;
            config.DeviceName = cboOutputDevice.Text;

            DialogResult = DialogResult.OK;

            Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        internal Config GetConfig()
        {
            return config;
        }
    }
}
