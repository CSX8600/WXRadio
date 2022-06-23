using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WXRadio.WXSynthesizer.RadioMenus;

namespace WXRadio.WXSynthesizer
{
    public partial class frmRadioInterface : Form
    {
        private BaseMenu currentMenu;
        private bool mouseDown;
        private Point lastLocation;
        private Config config;
        private Action<Config> configSave;
        private Func<IntPtr> getDashboardControlHandle;
        public frmRadioInterface(Config config, Action<Config> configSave, Func<IntPtr> getDashboardControlHandle) : this()
        {
            this.config = config;
            this.configSave = configSave;
            this.getDashboardControlHandle = getDashboardControlHandle;

            currentMenu = new RadioOffMenu();
            currentMenu.Config = this.config;
            currentMenu.LabelChangeCallback = text =>
            {
                if (IsHandleCreated)
                {
                    Invoke(new MethodInvoker(() => lblMessage.Text = text));
                }
            };
            currentMenu.SaveConfigCallback = this.configSave;
            currentMenu.GetDashboardControlHandleCallback = this.getDashboardControlHandle;
            currentMenu.ForceMenuCallback = result => HandleButtonResult(result);
            lblMessage.Text = currentMenu.GetTextForInitialDisplay();
        }

        protected frmRadioInterface()
        {
            InitializeComponent();
        }

        private void lblMenu_Click(object sender, EventArgs e)
        {
            HandleButtonResult(currentMenu.ButtonPressed(BaseMenu.ButtonTypes.Menu));
        }

        private void HandleButtonResult(BaseMenu.ButtonResult result)
        {
            if (!result.PressSuccess)
            {
                if (result.PlayButtonSound && config.ButtonBeeps)
                {
                    SoundPlayer failedSoundPlayer = new SoundPlayer(Properties.Resources.double_beep);
                    failedSoundPlayer.Play();
                }

                return;
            }

            if (result.PlayButtonSound && config.ButtonBeeps)
            {
                SoundPlayer soundPlayer = new SoundPlayer(Properties.Resources.single_beep);
                soundPlayer.Play();
            }

            currentMenu = result.NextMenu;
            currentMenu.GetDashboardControlHandleCallback = getDashboardControlHandle;
            currentMenu.Config = config;
            currentMenu.SaveConfigCallback = configSave;
            currentMenu.LabelChangeCallback = text =>
            {
                if (IsHandleCreated)
                {
                    Invoke(new MethodInvoker(() => lblMessage.Text = text));
                }
            };
            currentMenu.ForceMenuCallback = res => HandleButtonResult(res);

            if (IsHandleCreated)
            {
                Invoke(new MethodInvoker(() => lblMessage.Text = currentMenu.GetTextForInitialDisplay()));
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            currentMenu.OnTimer();
        }

        private void lblSelect_Click(object sender, EventArgs e)
        {
            HandleButtonResult(currentMenu.ButtonPressed(BaseMenu.ButtonTypes.Select));
        }

        private void lblUp_Click(object sender, EventArgs e)
        {
            HandleButtonResult(currentMenu.ButtonPressed(BaseMenu.ButtonTypes.Up));
        }

        private void lblLeft_Click(object sender, EventArgs e)
        {
            HandleButtonResult(currentMenu.ButtonPressed(BaseMenu.ButtonTypes.Left));
        }

        private void lblDown_Click(object sender, EventArgs e)
        {
            HandleButtonResult(currentMenu.ButtonPressed(BaseMenu.ButtonTypes.Down));
        }

        private void lblRight_Click(object sender, EventArgs e)
        {
            HandleButtonResult(currentMenu.ButtonPressed(BaseMenu.ButtonTypes.Right));
        }

        private void lblWeatherSnooze_Click(object sender, EventArgs e)
        {
            HandleButtonResult(currentMenu.ButtonPressed(BaseMenu.ButtonTypes.WeatherSnooze));
        }

        private void frmRadioInterface_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void frmRadioInterface_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void frmRadioInterface_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
