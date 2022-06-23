using Microsoft.DirectX.DirectSound;
using System.Collections.Generic;

namespace WXRadio.WXSynthesizer.RadioMenus
{
    public class DeviceMenu : BaseMenu
    {
        private List<string> _installedDevices;
        private string _currentDevice;
        private int _currentDeviceCurrentIndex = 0;

        public DeviceMenu() : base()
        {
            _installedDevices = new List<string>();
            foreach(DeviceInformation deviceInformation in new DevicesCollection())
            {
                _installedDevices.Add(deviceInformation.Description);
            }
        }

        protected override void OnConfigSet()
        {
            _currentDevice = config.DeviceName;
        }

        public override ButtonResult ButtonPressed(ButtonTypes buttonType)
        {
            switch (buttonType)
            {
                case ButtonTypes.Up:
                    DecrementDevice();
                    return new ButtonResult(true, this);
                case ButtonTypes.Down:
                    IncrementDevice();
                    return new ButtonResult(true, this);
                case ButtonTypes.Menu:
                    return new ButtonResult(true, new MainMenu(MainMenu.Screens.Device));
                case ButtonTypes.Select:
                    config.DeviceName = _currentDevice;
                    saveConfigCallback(config);
                    Synthesizer.INSTANCE.Start(config);
                    return new ButtonResult(true, new MainMenu(MainMenu.Screens.Device));
                default:
                    return new ButtonResult(false);
            }
        }

        public override void OnTimer()
        {
            if (_currentDevice.Length <= 14)
            {
                _currentDeviceCurrentIndex = 0;
                return;
            }

            _currentDeviceCurrentIndex++;
            if (_currentDeviceCurrentIndex >= _currentDevice.Length)
            {
                _currentDeviceCurrentIndex = 0;
            }

            int substringLength = _currentDevice.Length - _currentDeviceCurrentIndex;
            if (substringLength > 14)
            {
                substringLength = 14;
            }

            ChangeLabel(_currentDevice.Substring(_currentDeviceCurrentIndex, substringLength).PadRight(14, ' '));
        }

        public override string GetTextForInitialDisplay()
        {
            int substringLength = _currentDevice.Length;
            if (substringLength > 14)
            {
                substringLength = 14;
            }

            return _currentDevice.Substring(0, substringLength);
        }

        private void IncrementDevice()
        {
            int index = _installedDevices.IndexOf(_currentDevice);
            index++;
            if (index >= _installedDevices.Count)
            {
                index = 0;
            }

            _currentDevice = _installedDevices[index];
        }

        private void DecrementDevice()
        {
            int index = _installedDevices.IndexOf(_currentDevice);
            index--;
            if (index < 0)
            {
                index = _installedDevices.Count - 1;
            }

            _currentDevice = _installedDevices[index];
        }
    }
}
