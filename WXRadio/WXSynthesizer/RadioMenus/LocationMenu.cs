using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WXRadio.WeatherManager;

namespace WXRadio.WXSynthesizer.RadioMenus
{
    public class LocationMenu : BaseMenu
    {
        private LocationTypes _locationType;
        private Screens _screen = Screens.LocationType;
        private Dictionary<string, string> _locationAbbreviationByLocationName;
        private string _currentLocation;
        public LocationMenu() : base()
        {
            _locationAbbreviationByLocationName = new Dictionary<string, string>();
            foreach(WeatherManagerConfiguration.Region region in WeatherManagerConfiguration.INSTANCE.Regions)
            {
                _locationAbbreviationByLocationName.Add(region.Name, region.Name.ToUpper().Replace("A", "").Replace("E", "").Replace("I", "").Replace("O", "").Replace("U", ""));
            }
        }

        protected override void OnConfigSet()
        {
            Enum.TryParse<LocationTypes>(config.Location.LocationType, out _locationType);
            if (_locationAbbreviationByLocationName.ContainsKey(config.Location.LocationName))
            {
                _currentLocation = config.Location.LocationName;
            }
            else
            {
                _currentLocation = _locationAbbreviationByLocationName.Keys.First();
            }
        }

        private enum LocationTypes
        {
            Any,
            Single
        }

        private enum Screens
        {
            LocationType,
            LocationSelect
        }

        public override ButtonResult ButtonPressed(ButtonTypes buttonType)
        {
            switch(buttonType)
            {
                case ButtonTypes.Down:
                case ButtonTypes.Up:
                    switch(_screen)
                    {
                        case Screens.LocationType:
                            if (_locationType == LocationTypes.Any)
                            {
                                _locationType = LocationTypes.Single;
                            }
                            else
                            {
                                _locationType = LocationTypes.Any;
                            }

                            return new ButtonResult(true, this);
                        case Screens.LocationSelect:
                            if(buttonType == ButtonTypes.Up)
                            {
                                DecrementLocation();
                            }
                            else
                            {
                                IncrementLocation();
                            }

                            return new ButtonResult(true, this);
                        default:
                            return new ButtonResult(false);
                    }
                case ButtonTypes.Menu:
                    if (_screen == Screens.LocationType)
                    {
                        return new ButtonResult(true, new MainMenu(MainMenu.Screens.Location));
                    }
                    else
                    {
                        _screen = Screens.LocationType;
                        return new ButtonResult(true, this);
                    }
                case ButtonTypes.Select:
                    if (_screen == Screens.LocationType)
                    {
                        if (_locationType == LocationTypes.Any)
                        {
                            config.Location.LocationType = _locationType.ToString();
                            saveConfigCallback(config);
                            return new ButtonResult(true, new MainMenu(MainMenu.Screens.Location));
                        }
                        else
                        {
                            _screen = Screens.LocationSelect;
                            return new ButtonResult(true, this);
                        }
                    }
                    else
                    {
                        config.Location.LocationType = _locationType.ToString();
                        config.Location.LocationName = _currentLocation;
                        saveConfigCallback(config);
                        return new ButtonResult(true, new MainMenu(MainMenu.Screens.Location));
                    }
                default:
                    return new ButtonResult(false);
            }
        }

        public override string GetTextForInitialDisplay()
        {
            switch(_screen)
            {
                case Screens.LocationType:
                    switch (_locationType)
                    {
                        case LocationTypes.Any:
                            return "ANY";
                        case LocationTypes.Single:
                            return "SINGLE";
                    }
                    break;
                case Screens.LocationSelect:
                    return _locationAbbreviationByLocationName[_currentLocation];
            }
            

            return "Err";
        }

        private void IncrementLocation()
        {
            List<string> keys = _locationAbbreviationByLocationName.Keys.ToList();
            int index = keys.IndexOf(_currentLocation);
            index++;
            if (index == keys.Count)
            {
                index = 0;
            }

            _currentLocation = keys[index];
        }

        private void DecrementLocation()
        {
            List<string> keys = _locationAbbreviationByLocationName.Keys.ToList();
            int index = keys.IndexOf(_currentLocation);
            index--;
            if (index == -1)
            {
                index = keys.Count - 1;
            }

            _currentLocation = keys[index];
        }
    }
}
