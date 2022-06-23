using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WXRadio.WeatherManager;
using WXRadio.WeatherManager.Extensions;

namespace WXRadio.WXSynthesizer.RadioMenus
{
    public class ChannelMenu : BaseMenu
    {
        int selectedChannel = -1;
        Dictionary<int, string> channelNamesByNumber = new Dictionary<int, string>();
        public ChannelMenu() : base()
        {
            foreach(WeatherManagerConfiguration.Region region in WeatherManagerConfiguration.INSTANCE.Regions)
            {
                string newName = region.Name.ToUpper()
                                            .Replace("A", "")
                                            .Replace("E", "")
                                            .Replace("I", "")
                                            .Replace("O", "")
                                            .Replace("U", "");

                if (channelNamesByNumber.ContainsKey(region.Channel))
                {
                    channelNamesByNumber[region.Channel] += "/";
                }
                else
                {
                    channelNamesByNumber[region.Channel] = string.Empty;
                }

                channelNamesByNumber[region.Channel] += newName;

                if (channelNamesByNumber[region.Channel].Length > 14)
                {
                    channelNamesByNumber[region.Channel] = channelNamesByNumber.GetOrDefault(region.Channel, string.Empty).Substring(0, 14);
                }
            }

            channelNamesByNumber[99] = "ALL";

            channelNamesByNumber = new Dictionary<int, string>(channelNamesByNumber.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
        }

        protected override void OnConfigSet()
        {
            selectedChannel = channelNamesByNumber.ContainsKey(config.CurrentChannel) ? config.CurrentChannel : channelNamesByNumber.Keys.First();
        }

        public override ButtonResult ButtonPressed(ButtonTypes buttonType)
        {
            switch(buttonType)
            {
                case ButtonTypes.Down:
                    IncreaseSelectedNumber();
                    return new ButtonResult(true, this);
                case ButtonTypes.Up:
                    DecreaseSelectedNumber();
                    return new ButtonResult(true, this);
                case ButtonTypes.Left:
                    return new ButtonResult(false);
                case ButtonTypes.Right:
                    return new ButtonResult(false);
                case ButtonTypes.Menu:
                    return new ButtonResult(true, new MainMenu(MainMenu.Screens.Channel));
                case ButtonTypes.Select:
                    config.CurrentChannel = selectedChannel;
                    saveConfigCallback(config);
                    Synthesizer.INSTANCE.Start(config);
                    return new ButtonResult(true, new MainMenu(MainMenu.Screens.Channel));
                default:
                    return new ButtonResult(false);
            }
        }

        public override string GetTextForInitialDisplay()
        {
            return selectedChannel + " - " + channelNamesByNumber[selectedChannel];
        }

        private void DecreaseSelectedNumber()
        {
            List<int> keys = channelNamesByNumber.Keys.ToList();
            int index = keys.IndexOf(selectedChannel);
            index--;
            if (index == -1)
            {
                index = keys.Count - 1;
            }

            selectedChannel = keys[index];
        }

        private void IncreaseSelectedNumber()
        {
            List<int> keys = channelNamesByNumber.Keys.ToList();
            int index = keys.IndexOf(selectedChannel);
            index++;
            if (index == keys.Count)
            {
                index = 0;
            }

            selectedChannel = keys[index];
        }
    }
}
