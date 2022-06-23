using System;

namespace WXRadio.WXSynthesizer.RadioMenus
{
    public class MainMenu : BaseMenu
    {
        private Screens _screen;
        public MainMenu(Screens screen)
        {
            _screen = screen;
        }

        public enum Screens
        {
            Location,
            AlertTypes,
            AlertTest,
            Channel,
            ButtonBeeps,
            Events,
            Voice,
            Device
        }
        public override ButtonResult ButtonPressed(ButtonTypes buttonType)
        {
            switch(buttonType)
            {
                case ButtonTypes.Menu:
                    return new ButtonResult(true, new RadioOffMenu());
                case ButtonTypes.Up:
                    return new ButtonResult(true, new MainMenu(GetPreviousScreen()));
                case ButtonTypes.Down:
                    return new ButtonResult(true, new MainMenu(GetNextScreen()));
                case ButtonTypes.Select:
                    return GetSelectResult();
                default:
                    return new ButtonResult(false);
            }
        }

        public override string GetTextForInitialDisplay()
        {
            switch(_screen)
            {
                case Screens.Location:
                    return "Set Location";
                case Screens.AlertTypes:
                    return "Alert Type";
                case Screens.AlertTest:
                    return "Alert Test";
                case Screens.Channel:
                    return "Set Channel";
                case Screens.ButtonBeeps:
                    return "Button Beeps";
                case Screens.Events:
                    return "Set Events";
                case Screens.Voice:
                    return "Set Voice";
                case Screens.Device:
                    return "Set Device";
                default:
                    return "Err";
            }
        }

        private Screens GetNextScreen()
        {
            Screens nextScreen;
            if ((int)_screen == Enum.GetValues(typeof(Screens)).Length - 1)
            {
                nextScreen = 0;
            }
            else
            {
                nextScreen = _screen + 1;
            }

            return nextScreen;
        }

        private Screens GetPreviousScreen()
        {
            Screens previousScreen;
            if (_screen == 0)
            {
                previousScreen = (Screens)Enum.GetValues(typeof(Screens)).Length - 1;
            }
            else
            {
                previousScreen = _screen - 1;
            }

            return previousScreen;
        }

        private ButtonResult GetSelectResult()
        {
            switch(_screen)
            {
                case Screens.Location:
                    return new ButtonResult(true, new LocationMenu());
                case Screens.AlertTypes:
                    return new ButtonResult(true, new AlertTypesMenu());
                case Screens.AlertTest:
                    return new ButtonResult(true, new AlertTest(), false);
                case Screens.Channel:
                    return new ButtonResult(true, new ChannelMenu());
                case Screens.ButtonBeeps:
                    return new ButtonResult(true, new ButtonBeepsMenu());
                case Screens.Events:
                    return new ButtonResult(true, new SetEventsMenu());
                case Screens.Voice:
                    return new ButtonResult(true, new VoiceMenu());
                case Screens.Device:
                    return new ButtonResult(true, new DeviceMenu());
                default:
                    return new ButtonResult(false);
            }
        }
    }
}
