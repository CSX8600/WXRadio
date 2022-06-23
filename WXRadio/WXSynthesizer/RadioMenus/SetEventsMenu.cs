using System;
using System.Collections.Generic;
using System.Linq;
using WXRadio.WeatherManager;
using WXRadio.WeatherManager.Extensions;
using WXRadio.WeatherManager.Product;

namespace WXRadio.WXSynthesizer.RadioMenus
{
    public class SetEventsMenu : BaseMenu
    {
        private Screens _screen = Screens.AllOn;
        private Dictionary<string, string> _productTypesByProductName = new Dictionary<string, string>();

        public SetEventsMenu() : base()
        {
            Type baseProductType = typeof(BaseProduct);
            foreach(Type type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(t => t != baseProductType && baseProductType.IsAssignableFrom(t)))
            {
                _productTypesByProductName.Add(type.Name.ToDisplayString(), type.FullName);
            }
        }

        public enum Screens
        {
            AllOn,
            AllOff,
            EditEvents
        }

        public override ButtonResult ButtonPressed(ButtonTypes buttonType)
        {
            switch(buttonType)
            {
                case ButtonTypes.Select:
                    switch(_screen)
                    {
                        case Screens.AllOn:
                            config.Events = new List<Event>();
                            foreach(string productType in _productTypesByProductName.Values)
                            {
                                config.Events.Add(new Event() { FullName = productType, Enabled = true });
                            }
                            saveConfigCallback(config);
                            return new ButtonResult(true, new MainMenu(MainMenu.Screens.Events));
                        case Screens.AllOff:
                            config.Events = new List<Event>();
                            foreach (string productType in _productTypesByProductName.Values)
                            {
                                config.Events.Add(new Event() { FullName = productType, Enabled = false });
                            }
                            saveConfigCallback(config);
                            return new ButtonResult(true, new MainMenu(MainMenu.Screens.Events));
                        case Screens.EditEvents:
                            return new ButtonResult(true, new EditEventsMenu(_productTypesByProductName));
                        default:
                            return new ButtonResult(false);
                    }
                case ButtonTypes.Up:
                    DecrementScreen();
                    return new ButtonResult(true, this);
                case ButtonTypes.Down:
                    IncrementScreen();
                    return new ButtonResult(true, this);
                case ButtonTypes.Menu:
                    return new ButtonResult(true, new MainMenu(MainMenu.Screens.Events));
                default:
                    return new ButtonResult(false);
            }
        }

        public override string GetTextForInitialDisplay()
        {
            return _screen.ToString().ToDisplayString().ToUpper();
        }

        private void IncrementScreen()
        {
            int screenIndex = (int)_screen;
            screenIndex++;

            if (screenIndex >= Enum.GetValues(typeof(Screens)).Length)
            {
                screenIndex = 0;
            }

            _screen = (Screens)screenIndex;
        }

        private void DecrementScreen()
        {
            int screenIndex = (int)_screen;
            screenIndex--;

            if (screenIndex < 0)
            {
                screenIndex = Enum.GetValues(typeof(Screens)).Length - 1;
            }

            _screen = (Screens)screenIndex;
        }

        private class EditEventsMenu : BaseMenu
        {
            private Dictionary<string, string> _productTypesByProductName = new Dictionary<string, string>();
            private string _currentProduct;
            private int _currentProductDisplayIndex = 0;
            public EditEventsMenu(Dictionary<string, string> productTypesByProductName, string selectedProduct = null)
            {
                _productTypesByProductName = productTypesByProductName;
                _currentProduct = selectedProduct ?? _productTypesByProductName.Keys.First();
            }

            public override ButtonResult ButtonPressed(ButtonTypes buttonType)
            {
                switch(buttonType)
                {
                    case ButtonTypes.Up:
                        DecrementProduct();
                        return new ButtonResult(true, this);
                    case ButtonTypes.Down:
                        IncrementProduct();
                        return new ButtonResult(true, this);
                    case ButtonTypes.Menu:
                        return new ButtonResult(true, new MainMenu(MainMenu.Screens.Events));
                    case ButtonTypes.Select:
                        return new ButtonResult(true, new EditEventMenu(_productTypesByProductName, _productTypesByProductName[_currentProduct]));
                    default:
                        return new ButtonResult(false);
                }
            }

            public override string GetTextForInitialDisplay()
            {
                int substringLength = _currentProduct.Length;
                if (substringLength > 14)
                {
                    substringLength = 14;
                }

                return _currentProduct.Substring(0, substringLength);
            }

            public override void OnTimer()
            {
                if (_currentProduct.Length > 14)
                {
                    if (_currentProductDisplayIndex < _currentProduct.Length - 1)
                    {
                        _currentProductDisplayIndex++;
                    }
                    else
                    {
                        _currentProductDisplayIndex = 0;
                    }

                    int substringLength = _currentProduct.Length - _currentProductDisplayIndex;
                    if (substringLength > 14)
                    {
                        substringLength = 14;
                    }

                    ChangeLabel(_currentProduct.Substring(_currentProductDisplayIndex, substringLength).PadRight(14, ' '));
                }
            }

            private void IncrementProduct()
            {
                List<string> productNames = _productTypesByProductName.Keys.ToList();
                int index = productNames.IndexOf(_currentProduct);
                index++;
                if (index >= productNames.Count)
                {
                    index = 0;
                }

                _currentProductDisplayIndex = 0;
                _currentProduct = productNames[index];
            }

            private void DecrementProduct()
            {
                List<string> productNames = _productTypesByProductName.Keys.ToList();
                int index = productNames.IndexOf(_currentProduct);
                index--;
                if (index < 0)
                {
                    index = productNames.Count - 1;
                }

                _currentProductDisplayIndex = 0;
                _currentProduct = productNames[index];
            }
        }

        private class EditEventMenu : BaseMenu
        {
            private Dictionary<string, string> _productEventsByProductName;
            private string _eventType;
            private Screens _screen;
            public EditEventMenu(Dictionary<string, string> productTypesByProductName, string eventType) : base()
            {
                _productEventsByProductName = productTypesByProductName;
                _eventType = eventType;
            }

            protected override void OnConfigSet()
            {
                Event configEvent = config.Events.SingleOrDefault(e => e.FullName == _eventType);
                _screen = (configEvent?.Enabled ?? false) ? Screens.On : Screens.Off;
            }

            public enum Screens
            {
                On,
                Off
            }

            public override ButtonResult ButtonPressed(ButtonTypes buttonType)
            {
                switch(buttonType)
                {
                    case ButtonTypes.Down:
                    case ButtonTypes.Up:
                        if (_screen == Screens.On) { _screen = Screens.Off; } else { _screen = Screens.On; }
                        return new ButtonResult(true, this);
                    case ButtonTypes.Menu:
                        return new ButtonResult(true, new EditEventsMenu(_productEventsByProductName, _productEventsByProductName.First(kvp => kvp.Value == _eventType).Key));
                    case ButtonTypes.Select:
                        Event configEvent = config.Events.SingleOrDefault(e => e.FullName == _eventType);
                        configEvent.Enabled = _screen == Screens.On;
                        saveConfigCallback(config);
                        return new ButtonResult(true, new EditEventsMenu(_productEventsByProductName, _productEventsByProductName.First(kvp => kvp.Value == _eventType).Key));
                    default:
                        return new ButtonResult(false);
                }
            }

            public override string GetTextForInitialDisplay()
            {
                return _screen == Screens.On ? "ON" : "OFF";
            }
        }
    }
}
