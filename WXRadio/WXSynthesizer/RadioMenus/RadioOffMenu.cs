using System;
using System.Media;
using WXRadio.WeatherManager.Extensions;
using WXRadio.WeatherManager.Product;

namespace WXRadio.WXSynthesizer.RadioMenus
{
    public class RadioOffMenu : BaseMenu
    {
        private bool displayingAlert = false;
        private bool displayingError = false;
        private int errorTimeout = 0;
        private string alertText = "";
        private int alertTextCurrentIndex;
        public RadioOffMenu() : base()
        {
            Synthesizer.INSTANCE.Volume = -10_000;
            Synthesizer.INSTANCE.RadioActivationRequired += RadioActivationRequired;
        }

        private void RadioActivationRequired(object sender, WeatherManager.Product.IEmergency e)
        {
            BaseProduct product = e as BaseProduct;

            if (config.AlertType == "Sound")
            {
                new SoundPlayer(Properties.Resources.alert).PlayLooping();
            }
            else if (config.AlertType == "Voice")
            {
                forceMenuCallback(new ButtonResult(true, new RadioOnMenu(), false));
            }

            alertText = product.GetType().Name.ToDisplayString();
            if (alertText.Length <= 14)
            {
                ChangeLabel(alertText);
            }
            else
            {
                ChangeLabel(alertText.Substring(0, 14));
            }
            displayingAlert = true;
        }

        public override ButtonResult ButtonPressed(ButtonTypes buttonType)
        {
            switch(buttonType)
            {
                case ButtonTypes.Menu:
                    return new ButtonResult(true, new MainMenu(0));
                case ButtonTypes.WeatherSnooze:
                    if (string.IsNullOrEmpty(config.VoiceName))
                    {
                        displayingError = true;
                        errorTimeout = 0;
                        ChangeLabel("No Voice Sel");
                    }

                    if (string.IsNullOrEmpty(config.DeviceName))
                    {
                        displayingError = true;
                        errorTimeout = 0;
                        ChangeLabel("No Device Sel");
                    }

                    if (displayingError)
                    {
                        return new ButtonResult(false);
                    }

                    return new ButtonResult(true, new RadioOnMenu());
                default:
                    return new ButtonResult(false);
            }
        }

        public override string GetTextForInitialDisplay()
        {
            return DateTime.Now.ToString("hh:mm tt");
        }

        public override void OnTimer()
        {
            if (displayingAlert)
            {
                if (alertText.Length <= 14)
                {
                    return;
                }

                if (alertTextCurrentIndex < alertText.Length - 1)
                {
                    alertTextCurrentIndex++;
                }
                else
                {
                    alertTextCurrentIndex = 0;
                }

                int substringLength = alertText.Length - alertTextCurrentIndex;
                if (substringLength > 14)
                {
                    substringLength = 14;
                }

                ChangeLabel(alertText.Substring(alertTextCurrentIndex, substringLength).PadRight(14, ' '));
            }
            else if (displayingError)
            {
                errorTimeout++;
                if (errorTimeout > 6)
                {
                    displayingError = false;
                }
            }
            else
            {
                ChangeLabel(DateTime.Now.ToString("hh:mm tt"));
            }
        }
    }
}
