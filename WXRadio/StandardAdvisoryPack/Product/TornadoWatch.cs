using System;
using System.Collections.Generic;
using System.Linq;
using WXRadio.WeatherManager;
using WXRadio.WeatherManager.Extensions;
using WXRadio.WeatherManager.Product;
using WXRadio.WeatherManager.Utility;

namespace StandardAdvisoryPack.Product
{
    public class TornadoWatch : BaseProduct, ICancellable, IEmergency, ISummarizable
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
        public TornadoWatch(Storm storm) : base()
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
            DetailedProductBuilder detailedProductBuilder = new DetailedProductBuilder()
            {
                RequestImmediateBroadcast = true,
                HeaderText = new List<string>()
                {
                    "TORNADO WATCH"
                }
            };

            detailedProductBuilder.Sections.Add(new DetailedProductBuilder.Section()
            {
                Details = new List<string>() { $"The national weather service in {WeatherManagerConfiguration.INSTANCE.StationName} has issued a..." }
            });

            detailedProductBuilder.Sections.Add(new DetailedProductBuilder.Section()
            {
                Details = new List<string>()
                {
                    "Tornado Watch for..."
                },
                FormattingOption = DetailedProductBuilder.Section.FormattingOptions.IndentedStar
            });
            detailedProductBuilder.Sections.Last().Details.AddRange(_affectedRegions);
            detailedProductBuilder.Sections.Last().Details.Add("Until " + expiryTime.ToString("h:mm tt"));

            detailedProductBuilder.Sections.Add(new DetailedProductBuilder.Section()
            {
                Details = new List<string>() { "Tornadoes...Large Hail...Thundertorm wind gusts to 70 MPH...and dangerous lightning are possible in these areas." }
            });

            detailedProductBuilder.Sections.Add(new DetailedProductBuilder.Section()
            {
                Header = "Precuationary/Preparedness...",
                Details = new List<string>()
                {
                    "Remember...a tornado watch means that conditions are favorable for tornadoes and severe thunderstorms in and close to the watch area.  Persons in these areas should be on the lookout for threatening weather conditions and listen for later statements and possible warnings."
                }
            });

            return detailedProductBuilder.Build();
        }

        #region IEmergency
        public EmergencyNotificationTypes EmergencyNotificationType => EmergencyNotificationTypes.WarnPublicOnce;

        private bool _isQueued = true;
        public void AcknowledgeImmediateBroadcastQueue()
        {
            _isQueued = false;
        }

        public string ImmediateBroadcastInformation()
        {
            string format = "The national weather service in {0} has issued a Tornado Watch, effective until {1}. . This watch includes the following areas: {2}.  "
                + "Remember, a tornado watch means that conditions are favorable for the development of severe weather including tornadoes, large hail, and damaging winds in and close "
                + "to the watch area.  While severe weather may not be imminent, persons should remain alert for rapidly changing weather conditions and listen for later statements "
                + "and possible warnings.  Stay tuned to weather radio, commercial radio, and television outlets, or internet sources for the latest severe weather information.";

            return string.Format(format, WeatherManagerConfiguration.INSTANCE.StationName, expiryTime.ToString("h:mm tt"), _affectedRegions.ConcatToString(","));
        }

        public bool QueueForImmediateBroadcast()
        {
            return _isQueued;
        }
        #endregion

        #region ICancellable
        private bool _isCancelled = false;
        public bool IsCancelled
        {
            get { return _isCancelled; }
            set { _isCancelled = value; InvokeProductUpdatedEvent(new EmptyProductUpdateEventArgs(this)); }
        }

        public string GetCancelSummary()
        {
            return $"The Tornado Watch for {_affectedRegions.ConcatToString(",")} has been cancelled.";
        }
        #endregion

        #region ISummariable
        public string GetSummary()
        {
            if (IsCancelled)
            {
                return string.Empty;
            }

            string format = "A tornado watch remains in effect until {0} for the following areas: {1}.";

            return string.Format(format, expiryTime.ToString("h:mm tt"), _affectedRegions.ConcatToString(","));
        }
        #endregion
    }
}
