using System.Media;

namespace WXRadio.WXSynthesizer.RadioMenus
{
    public class AlertTest : BaseMenu
    {
        SoundPlayer soundPlayer = new SoundPlayer();
        public AlertTest() : base()
        {
            soundPlayer.Stream = Properties.Resources.alert;
            soundPlayer.PlayLooping();
        }

        public override ButtonResult ButtonPressed(ButtonTypes buttonType)
        {
            soundPlayer.Stop();
            return new ButtonResult(true, new MainMenu(MainMenu.Screens.AlertTest));
        }

        public override string GetTextForInitialDisplay()
        {
            return "TESTING";
        }
    }
}
