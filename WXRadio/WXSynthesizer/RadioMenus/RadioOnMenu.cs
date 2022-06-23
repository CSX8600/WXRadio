using System.Media;
using WXRadio.WeatherManager.Extensions;
using WXRadio.WeatherManager.Product;

namespace WXRadio.WXSynthesizer.RadioMenus
{
    public class RadioOnMenu : BaseMenu
    {
        private bool displayingAlert = false;
        private string alertText = "";
        private int alertTextCurrentIndex;
        public RadioOnMenu() : base()
        {
            Synthesizer.INSTANCE.RadioActivationRequired += RadioActivationRequired;
            Synthesizer.INSTANCE.Volume = 0;
        }

        private void RadioActivationRequired(object sender, IEmergency e)
        {
            BaseProduct product = e as BaseProduct;

            if (config.AlertType == "Sound")
            {
                new SoundPlayer(Properties.Resources.alert).PlayLooping();
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
            if (buttonType != ButtonTypes.WeatherSnooze)
            {
                return new ButtonResult(false);
            }

            if (displayingAlert)
            {
                displayingAlert = false;
                alertText = "";
                return new ButtonResult(true, this);
            }

            Synthesizer.INSTANCE.RadioActivationRequired -= RadioActivationRequired;
            return new ButtonResult(true, new RadioOffMenu());
        }

        public override void OnTimer()
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

        public override string GetTextForInitialDisplay()
        {
            return "WEATHER";
        }
    }
}
