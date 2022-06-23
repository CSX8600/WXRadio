using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WXRadio.WeatherManager;
using WXRadio.WeatherManager.Extensions;
using WXRadio.WeatherManager.Product;
using WXRadio.WeatherManager.Utility;

namespace StandardAdvisoryPack.Product
{
    public class SevereThunderstormWatch : BaseProduct, IEmergency, ICancellable, ISummarizable
    {
        private List<Coordinate> _polygon;
        private List<string> _affectedRegions;
        private DateTime expiryTime;
        public override Coordinate[] GetPolygonCoordinates()
        {
            if (IsCancelled)
            {
                return new Coordinate[0];
            }

            return _polygon.ToArray();
        }

        internal SevereThunderstormWatch(Storm storm) : base()
        {
            Coordinate futureStormTrack = storm.GetPositionInFuture(700);

            // Create polygon
            double currentStormAngle = storm.GetCurrentMotion();
            _polygon = new List<Coordinate>();
            _polygon.Add(Util.GetCoordinateInDistance(storm.Coordinate, Util.RotateCW(currentStormAngle), 50));
            _polygon.Add(Util.GetCoordinateInDistance(futureStormTrack, Util.RotateCW(currentStormAngle), 300));
            _polygon.Add(Util.GetCoordinateInDistance(futureStormTrack, Util.RotateCCW(currentStormAngle), 300));
            _polygon.Add(Util.GetCoordinateInDistance(storm.Coordinate, Util.RotateCCW(currentStormAngle), 50));

            _affectedRegions = GetAffectedRegions().ToList();

            expiryTime = DateTime.Now.AddSeconds(700);
        }

        public override string GetDetailedInformation()
        {
            if (IsCancelled)
            {
                return string.Empty;
            }

            DetailedProductBuilder detailedProductBuilder = new DetailedProductBuilder();
            detailedProductBuilder.RequestImmediateBroadcast = true;
            detailedProductBuilder.HeaderText.Add("Severe Thunderstorm Watch");

            detailedProductBuilder.Sections.Add(new DetailedProductBuilder.Section()
            {
                Details = new List<string>()
                {
                    "The National Weather Service in " + WeatherManagerConfiguration.INSTANCE.StationName + " has issued a"
                }
            });

            // Calculate reference text
            List<string> referenceTextLines = new List<string>() { "Severe Thunderstorm Watch for..." };
            referenceTextLines.AddRange(_affectedRegions);
            referenceTextLines.Add("Until " + expiryTime.ToString("h:mm tt"));

            detailedProductBuilder.Sections.Add(new DetailedProductBuilder.Section()
            {
                FormattingOption = DetailedProductBuilder.Section.FormattingOptions.IndentedStar,
                Details = referenceTextLines
            });

            detailedProductBuilder.Sections.Add(new DetailedProductBuilder.Section()
            {
                Header = "Precautionary/Preparedness Actions...",
                Details = new List<string>()
                {
                    "Remember, a severe thunderstorm watch means that conditions are favorable for the development "
                    + "of severe weather, including large hail and damaging winds, in and close to the watch area.  While severe weather may not be imminent, "
                    + "persons should remain alert for rapidly changing weather conditions and listen for later statements and possible warnings.  Stay tuned "
                    + "to weather radio, commercial radio, and television outlets, or internet sources for the latest severe weather information."
                }
            });

            return detailedProductBuilder.Build();
        }

        #region IEmergency
        public EmergencyNotificationTypes EmergencyNotificationType => EmergencyNotificationTypes.General;

        public void AcknowledgeImmediateBroadcastQueue()
        {
            isQueuedForImmediateBroadcast = false;
        }

        public string ImmediateBroadcastInformation()
        {
            StringBuilder referenceBuilder = new StringBuilder();
            foreach(string reference in _affectedRegions)
            {
                if (referenceBuilder.Length > 0)
                {
                    referenceBuilder.Append(", ");
                }

                referenceBuilder.Append(reference);
            }

            string format = "The national weather service in {0} has issued A severe thunderstorm watch effective until {1}. .  This watch "
                + "includes the following areas: {2}.  Remember, a severe thunderstorm watch means that conditions are favorable for the development "
                + "of severe weather, including large hail and damaging winds, in and close to the watch area.  While severe weather may not be imminent, "
                + "persons should remain alert for rapidly changing weather conditions and listen for later statements and possible warnings.  Stay tuned "
                + "to weather radio, commercial radio, and television outlets, or internet sources for the latest severe weather information.";

            return string.Format(format, WeatherManagerConfiguration.INSTANCE.StationName, expiryTime.ToString("h:mm tt"), referenceBuilder.ToString());
        }

        private bool isQueuedForImmediateBroadcast = true;
        public bool QueueForImmediateBroadcast()
        {
            return isQueuedForImmediateBroadcast;
        }
        #endregion

        #region ICancellable
        private bool _isCancelled;
        public bool IsCancelled {
            get { return _isCancelled; }
            set { _isCancelled = value; InvokeProductUpdatedEvent(new EmptyProductUpdateEventArgs(this)); }
        }

        public string GetCancelSummary()
        {
            return string.Format("The severe thunderstorm watch for {0} has been cancelled.", _affectedRegions.ConcatToString(","));
        }
        #endregion

        #region ISummarizable
        public string GetSummary()
        {
            if (IsCancelled)
            {
                return string.Empty;
            }

            string format = "A severe thunderstorm watch remains in effect until {0} for the following regions: {1}.";

            return string.Format(format, expiryTime.ToString("h:mm tt"), _affectedRegions.ConcatToString(","));
        }
        #endregion
    }
}
