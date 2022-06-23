namespace WXRadio.WXSynthesizer.RadioMenus
{
    public class ButtonBeepsMenu : BaseMenu
    {
        private bool _enabled;

        protected override void OnConfigSet()
        {
            _enabled = config.ButtonBeeps;
        }

        public override ButtonResult ButtonPressed(ButtonTypes buttonType)
        {
            switch(buttonType)
            {
                case ButtonTypes.Up:
                case ButtonTypes.Down:
                    _enabled = !_enabled;

                    return new ButtonResult(true, this);
                case ButtonTypes.Menu:
                    return new ButtonResult(true, new MainMenu(MainMenu.Screens.ButtonBeeps));
                case ButtonTypes.Select:
                    config.ButtonBeeps = _enabled;
                    saveConfigCallback(config);

                    return new ButtonResult(true, new MainMenu(MainMenu.Screens.ButtonBeeps));
                default:
                    return new ButtonResult(false);
            }
        }

        public override string GetTextForInitialDisplay()
        {
            return _enabled ? "ON" : "OFF";
        }
    }
}
