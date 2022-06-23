using System;

namespace WXRadio.WXSynthesizer.RadioMenus
{
    public class AlertTypesMenu : BaseMenu
    {
        private AlertTypes _alertType;
        private enum AlertTypes
        {
            Sound,
            Voice,
            Display
        }

        protected override void OnConfigSet()
        {
            Enum.TryParse<AlertTypes>(config.AlertType, out _alertType);
        }

        public override ButtonResult ButtonPressed(ButtonTypes buttonType)
        {
            switch(buttonType)
            {
                case ButtonTypes.Up:
                    DecrementAlertType();
                    return new ButtonResult(true, this);
                case ButtonTypes.Down:
                    IncrementAlertType();
                    return new ButtonResult(true, this);
                case ButtonTypes.Menu:
                    return new ButtonResult(true, new MainMenu(MainMenu.Screens.AlertTypes));
                case ButtonTypes.Select:
                    config.AlertType = _alertType.ToString();
                    saveConfigCallback(config);
                    return new ButtonResult(true, new MainMenu(MainMenu.Screens.AlertTypes));
                default:
                    return new ButtonResult(false);
            }
        }

        public override string GetTextForInitialDisplay()
        {
            return _alertType.ToString().ToUpper();
        }

        private void IncrementAlertType()
        {
            int alertTypeIndex = (int)_alertType;
            alertTypeIndex++;
            if (alertTypeIndex >= Enum.GetValues(typeof(AlertTypes)).Length)
            {
                alertTypeIndex = 0;
            }

            _alertType = (AlertTypes)alertTypeIndex;
        }

        private void DecrementAlertType()
        {
            int alertTypeIndex = (int)_alertType;
            alertTypeIndex--;
            if (alertTypeIndex < 0)
            {
                alertTypeIndex = Enum.GetValues(typeof(AlertTypes)).Length - 1;
            }

            _alertType = (AlertTypes)alertTypeIndex;
        }
    }
}
