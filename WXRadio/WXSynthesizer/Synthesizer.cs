using Microsoft.DirectX.DirectSound;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WXRadio.WeatherManager;
using WXRadio.WeatherManager.Product;
using WXRadio.WeatherManager.Utility;

namespace WXRadio.WXSynthesizer
{
    internal class Synthesizer
    {
        private static Synthesizer _instance;
        public static Synthesizer INSTANCE
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Synthesizer();
                }

                return _instance;
            }
        }

        private Synthesizer() { }
        private Thread synthThread = null;
        private Config config;
        public int Volume { get; set; } = -10_000;

        public event EventHandler<IEmergency> RadioActivationRequired;

        public void ConsiderRestart(IEmergency emergency)
        {
            BaseProduct product = emergency as BaseProduct;
            
            // Is it in our channel?
            if (config.CurrentChannel != 99 && !product.GetAffectedConfigurationRegions().Any(r => r.Channel == config.CurrentChannel))
            {
                emergency.AcknowledgeImmediateBroadcastQueue();
                return;
            }

            Start(config);
        }

        public void Start(Config config)
        {
            Stop();

            synthThread = new Thread(new ParameterizedThreadStart(DoSpeak));
            synthThread.Start(config);
        }

        public void Stop()
        {
            if (synthThread != null && synthThread.IsAlive)
            {
                synthThread.Abort();
            }
        }

        private void DoSpeak(object configParam)
        {
            config = configParam as Config;

            if (string.IsNullOrEmpty(config.VoiceName) || string.IsNullOrEmpty(config.DeviceName))
            {
                return;
            }

            using (SpeechSynthesizer speechSynth = new SpeechSynthesizer())
            using (Device speaker = new Device())
            {
                speechSynth.SelectVoice(config.VoiceName);
                speaker.SetCooperativeLevel(config.CoopControl, CooperativeLevel.Priority);

                while(true)
                {
                    try
                    {
                        IReadOnlyCollection<BaseProduct> readonlyProducts = ProductManager.INSTANCE.GetProducts();

                        // Strip out products that don't fit the channel we're listening in on
                        IEnumerable<BaseProduct> products = readonlyProducts.Where(p => p.GetAffectedConfigurationRegions().Any(r => config.CurrentChannel == 99 || r.Channel == config.CurrentChannel));

                        // Find emergency queue first
                        while(products.Any(p => p is IEmergency && ((IEmergency)p).QueueForImmediateBroadcast()))
                        {
                            BaseProduct emergencyProduct = products.First(p => p is IEmergency && ((IEmergency)p).QueueForImmediateBroadcast());
                            IEmergency emergency = emergencyProduct as IEmergency;                            

                            using (FileStream stream = new FileStream("plugins\\eas intro.wav", FileMode.Open))
                            {
                                PlayStream(stream, speaker);
                            }

                            // Is this emergency in our location?
                            if ((config.Location.LocationType.Equals("Any", StringComparison.OrdinalIgnoreCase) || emergencyProduct.GetAffectedConfigurationRegions().Any(r => r.Name != config.Location.LocationName)) &&
                                config.Events.Any(e => e.Enabled && e.FullName == emergencyProduct.GetType().FullName))
                            {
                                RadioActivationRequired?.Invoke(this, emergency);
                            }

                            using (FileStream stream = new FileStream("plugins\\eas beep.wav", FileMode.Open))
                            {
                                PlayStream(stream, speaker);
                            }

                            SayText(emergency.ImmediateBroadcastInformation(), speechSynth, speaker);

                            using (FileStream stream = new FileStream("plugins\\eas outro.wav", FileMode.Open))
                            {
                                PlayStream(stream, speaker);
                            }
                            emergency.AcknowledgeImmediateBroadcastQueue();
                        }

                        if (products.Any())
                        {
                            SayText("During severe weather, we reduce our broadcasting so we can bring you the most important severe weather information", speechSynth, speaker);
                        }

                        List<ISummarizable> summarizables = products.Where(p => p is ISummarizable).Cast<ISummarizable>().ToList();
                        if (summarizables.Any())
                        {
                            SayText("The following is a summary of watches and warnings in effect.", speechSynth, speaker);
    
                            foreach (ISummarizable summarizable in summarizables)
                            {
                                SayText(summarizable.GetSummary(), speechSynth, speaker);
                            }
                        }

                        List<ICancellable> cancellables = products.Where(p => p is ICancellable && ((ICancellable)p).IsCancelled).Cast<ICancellable>().ToList();
                        foreach(ICancellable cancellable in cancellables)
                        {
                            SayText(cancellable.GetCancelSummary(), speechSynth, speaker);
                        }

                        if (!products.Any())
                        {
                            #region General Programming
                            #region Current conditions
                            SayText("And now for the current local observations.", speechSynth, speaker);
                            Thread.Sleep(500);

                            #region By City
                            foreach(WeatherManagerConfiguration.City city in WeatherManagerConfiguration.INSTANCE.Cities)
                            {
                                string cityText = "In " + city.Name + ", ";
                                List<Coordinate> cityCoordinates = new List<Coordinate>();
                                cityCoordinates.Add(new Coordinate(city.X, city.Z));
                                cityCoordinates.Add(new Coordinate(city.X + city.CriticalDistance, city.Z));
                                cityCoordinates.Add(new Coordinate(city.X + city.CriticalDistance, city.Z + city.CriticalDistance));
                                cityCoordinates.Add(new Coordinate(city.X, city.Z + city.CriticalDistance));

                                bool isRaining = StormManager.INSTANCE.GetStorms().Any(s => s.Strength == Storm.Strengths.Rain && Util.CoordinateIsInPolygon(s.Coordinate, cityCoordinates));
                                
                                if (isRaining)
                                {
                                    cityText += "it was raining.";
                                }
                                else
                                {
                                    cityText += "it was cleaar.";
                                }

                                SayText(cityText, speechSynth, speaker);
                            }
                            Thread.Sleep(500);
                            #endregion

                            #region Regional
                            SayText("Now a look at current regional conditions.", speechSynth, speaker);
                            Thread.Sleep(500);

                            Dictionary<string, int> rainCountPerRegion = new Dictionary<string, int>();
                            foreach (Storm storm in StormManager.INSTANCE.GetStorms().Where(s => s.Strength == Storm.Strengths.Rain))
                            {
                                foreach (WeatherManagerConfiguration.Region region in WeatherManagerConfiguration.INSTANCE.Regions)
                                {
                                    rainCountPerRegion[region.Name] = 0;

                                    if (Util.CoordinateIsInPolygon(storm.Coordinate, region.GetCoordinates()))
                                    {
                                        rainCountPerRegion[region.Name]++;
                                    }
                                }
                            }

                            foreach (KeyValuePair<string, int> countsPerRegion in rainCountPerRegion)
                            {
                                string regionText = "In " + countsPerRegion.Key + ", ";

                                if (countsPerRegion.Value == 0)
                                {
                                    regionText += "it was clear.";
                                }
                                else if (countsPerRegion.Value == 1)
                                {
                                    regionText += "isolated rain was falling.";
                                }
                                else if (countsPerRegion.Value == 2)
                                {
                                    regionText += "scattered rain was falling.";
                                }
                                else
                                {
                                    regionText = "rain was falling.";
                                }

                                SayText(regionText, speechSynth, speaker);
                            }
                            Thread.Sleep(500);
                            #endregion

                            #region Server-wide
                            int totalRainInServer = StormManager.INSTANCE.GetStorms().Where(s => s.Strength == Storm.Strengths.Rain).Count();

                            string generalAreaText = "Across the entire area, ";
                            if (totalRainInServer == 0)
                            {
                                generalAreaText += "it was clear.";
                            }
                            else if (totalRainInServer > 0 && totalRainInServer <= 3)
                            {
                                generalAreaText += "isolated rain was falling.";
                            }
                            else if (totalRainInServer > 3 && totalRainInServer <= 6)
                            {
                                generalAreaText += "scattered rain was falling.";
                            }
                            else
                            {
                                generalAreaText += "rain was falling.";
                            }

                            SayText(generalAreaText, speechSynth, speaker);
                            #endregion
                            #endregion

                            #region Lightning Safety Tips
                            if (new Random().Next(0, 10) == 5)
                            {
                                SayText("Here are some lightning safety tips from the National Weather Service: when outdoors, if you hear thunder, go inside and find "
                                            + "shelter until the threat has passed.  Listen to Weather Radio or other outlets for information.  Structures with steel shells are ideal "
                                            + "for thunderstorms.  Buildings constructed of wood or wool may not be suitable for thunderstorms.  Remember also, thunderstorms "
                                            + "can produce tornadoes.  To reduce the risk of life and property being damaged by lightning or tornadoes, follow the previously stated tips.  "
                                            + "There is no severe weather occuring in your area, and we thank you for listening to this informational bulletin.", speechSynth, speaker);
                            }
                            #endregion
                            #endregion
                        }

                        Thread.Sleep(1000);

                        SayText("You are listening to weather radio originating from the national weather service office at " + WeatherManagerConfiguration.INSTANCE.StationName, speechSynth, speaker);

                        Thread.Sleep(1000);

                        SayText("The current time is " + DateTime.Now.ToString("h:mm tt") + ".", speechSynth, speaker);

                        Thread.Sleep(3000);
                    }
                    catch(ThreadAbortException)
                    {
                        // This is fine
                    }
                }
            }
        }

        private void SayText(string text, SpeechSynthesizer speechSynth, Device device)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                speechSynth.SetOutputToWaveStream(stream);
                speechSynth.Speak(text);

                stream.Position = 0;
                PlayStream(stream, device);
            }
        }

        private void PlayStream(Stream stream, Device device)
        {
            using (Microsoft.DirectX.DirectSound.Buffer buffer = new Microsoft.DirectX.DirectSound.Buffer(stream, new BufferDescription() { GlobalFocus = true, ControlVolume = true }, device))
            {
                int startingVolume = Volume;
                buffer.SetCurrentPosition(0);
                buffer.Volume = Volume;
                buffer.Play(0, BufferPlayFlags.Default);

                while (buffer.Status.Playing)
                {
                    if (startingVolume != Volume)
                    {
                        startingVolume = Volume;
                        buffer.Volume = Volume;
                    }
                }
            }
        }

        private class DoSpeakParams
        {
            public string VoiceName { get; set; }
            public IntPtr CooperativeControl { get; set; }
        }
    }
}
