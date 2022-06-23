using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace WXRadio.WXSynthesizer.RadioMenus
{
    public class VoiceMenu : BaseMenu
    {
        private List<string> _installedVoices;
        private string _currentVoice;
        private int _currentVoiceCurrentIndex = 0;

        public VoiceMenu() : base()
        {
            _installedVoices = new List<string>();
            SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();
            foreach(InstalledVoice installedVoice in speechSynthesizer.GetInstalledVoices())
            {
                _installedVoices.Add(installedVoice.VoiceInfo.Name);
            }
        }

        protected override void OnConfigSet()
        {
            _currentVoice = config.VoiceName;
        }

        public override ButtonResult ButtonPressed(ButtonTypes buttonType)
        {
            switch(buttonType)
            {
                case ButtonTypes.Up:
                    DecrementVoice();
                    return new ButtonResult(true, this);
                case ButtonTypes.Down:
                    IncrementVoice();
                    return new ButtonResult(true, this);
                case ButtonTypes.Menu:
                    return new ButtonResult(true, new MainMenu(MainMenu.Screens.Voice));
                case ButtonTypes.Select:
                    config.VoiceName = _currentVoice;
                    saveConfigCallback(config);
                    Synthesizer.INSTANCE.Start(config);
                    return new ButtonResult(true, new MainMenu(MainMenu.Screens.Voice));
                default:
                    return new ButtonResult(false);
            }
        }

        public override void OnTimer()
        {
            if (_currentVoice.Length <= 14)
            {
                _currentVoiceCurrentIndex = 0;
                return;
            }

            _currentVoiceCurrentIndex++;
            if (_currentVoiceCurrentIndex >= _currentVoice.Length)
            {
                _currentVoiceCurrentIndex = 0;
            }

            int substringLength = _currentVoice.Length - _currentVoiceCurrentIndex;
            if (substringLength > 14)
            {
                substringLength = 14;
            }

            ChangeLabel(_currentVoice.Substring(_currentVoiceCurrentIndex, substringLength).PadRight(14, ' '));
        }

        public override string GetTextForInitialDisplay()
        {
            int substringLength = _currentVoice.Length;
            if (substringLength > 14)
            {
                substringLength = 14;
            }

            return _currentVoice.Substring(0, substringLength);
        }

        private void IncrementVoice()
        {
            int index = _installedVoices.IndexOf(_currentVoice);
            index++;
            if (index >= _installedVoices.Count)
            {
                index = 0;
            }

            _currentVoice = _installedVoices[index];
        }

        private void DecrementVoice()
        {
            int index = _installedVoices.IndexOf(_currentVoice);
            index--;
            if (index < 0)
            {
                index = _installedVoices.Count - 1;
            }

            _currentVoice = _installedVoices[index];
        }
    }
}
