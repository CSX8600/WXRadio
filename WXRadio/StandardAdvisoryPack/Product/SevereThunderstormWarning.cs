using System;
using System.Collections.Generic;
using System.Linq;
using WXRadio.WeatherManager;
using WXRadio.WeatherManager.Extensions;
using WXRadio.WeatherManager.Product;
using WXRadio.WeatherManager.Utility;

namespace StandardAdvisoryPack.Product
{
    public class SevereThunderstormWarning : BaseProduct, ICancellable, IEmergency, ISummarizable
    {
        private List<Coordinate> _polygon;
        private List<string> _affectedRegions;
        private List<string> _affectedCities;
        private DateTime observationTime;
        private DateTime expiryTime;
        private double observationAngle;
        private double observationSpeed;
        private Tuple<WeatherManagerConfiguration.City, double, double> closestCity;
        public override Coordinate[] GetPolygonCoordinates()
        {
            if (IsCancelled)
            {
                return new Coordinate[0];
            }

            return _polygon.ToArray();
        }
        public SevereThunderstormWarning(Storm storm) : base()
        {
            Coordinate futureStormTrack = storm.GetPositionInFuture(200);

            // Create polygon
            double currentStormAngle = storm.GetCurrentMotion();
            _polygon = new List<Coordinate>();
            _polygon.Add(Util.GetCoordinateInDistance(storm.Coordinate, Util.RotateCW(currentStormAngle), 50));
            _polygon.Add(Util.GetCoordinateInDistance(futureStormTrack, Util.RotateCW(currentStormAngle), 150));
            _polygon.Add(Util.GetCoordinateInDistance(futureStormTrack, Util.RotateCCW(currentStormAngle), 150));
            _polygon.Add(Util.GetCoordinateInDistance(storm.Coordinate, Util.RotateCCW(currentStormAngle), 50));

            _affectedRegions = GetAffectedRegions().ToList();
            _affectedCities = GetImpactedCities(storm.Coordinate, storm.GetCurrentMotion(), storm.GetAverageSpeed(), 200, DateTime.Now).ToList();

            observationTime = DateTime.Now;
            observationSpeed = Math.Round(storm.GetAverageSpeed(), 1);
            observationAngle = storm.GetCurrentMotion();
            expiryTime = DateTime.Now.AddSeconds(200);
            closestCity = GetReferenceCity(storm.Coordinate);
        }

        public override string GetDetailedInformation()
        {
            DetailedProductBuilder detailedProductBuilder = new DetailedProductBuilder();
            detailedProductBuilder.RequestImmediateBroadcast = true;

            detailedProductBuilder.Sections.Add(new DetailedProductBuilder.Section()
            {
                Details = new List<string>()
                {
                    "The National Weather Service in " + WeatherManagerConfiguration.INSTANCE.StationName + " has issued a"
                }
            });

            detailedProductBuilder.Sections.Add(new DetailedProductBuilder.Section()
            {
                Details = new List<string>() { "Severe Thunderstorm Warning for..." },
                FormattingOption = DetailedProductBuilder.Section.FormattingOptions.IndentedStar | DetailedProductBuilder.Section.FormattingOptions.FormatFirstLineOnly
            });
            detailedProductBuilder.Sections.Last().Details.AddRange(_affectedRegions);

            detailedProductBuilder.Sections.Add(new DetailedProductBuilder.Section()
            {
                Details = new List<string>()
                {
                    "Until " + expiryTime.ToString("h:mm tt"),
                    "At " + observationTime.ToString("h:mm tt") + ", Doppler Radar Indicated a severe thunderstorm producing damaging winds and deadly cloud to ground lightning.  This storm was located " + Math.Round(closestCity.Item2 / 1000, 1) + " kilometers " + Util.GetDirectionNameFromAngle(closestCity.Item3) + " of " + closestCity.Item1.Name + "...moving " + Util.GetDirectionNameFromAngle(observationAngle) + " at " + observationSpeed + " m/s."
                },
                FormattingOption = DetailedProductBuilder.Section.FormattingOptions.IndentedStar
            });

            if (_affectedCities.Any())
            {
                detailedProductBuilder.Sections.Add(new DetailedProductBuilder.Section()
                {
                    Header = "This severe thunderstorm will be near...",
                    Details = _affectedCities
                });
            }

            detailedProductBuilder.Sections.Add(new DetailedProductBuilder.Section()
            {
                Header = "Precautionary/Preparedness actions...",
                Details = new List<string>() { "Tornadoes can develop quickly from severe thunderstorms.  Although a tornado is not immediately likely, if a tornado is spotted, act quickly and move to a place of safety inside a sturdy structure, such as a basement or a small interior room." }
            });

            return detailedProductBuilder.Build();
        }

        #region Emergency
        private bool _isQueued = true;
        public EmergencyNotificationTypes EmergencyNotificationType => EmergencyNotificationTypes.WarnPublicOnce;

        public void AcknowledgeImmediateBroadcastQueue()
        {
            _isQueued = false;
        }

        public string ImmediateBroadcastInformation()
        {
            string text = "The national weather service in " + WeatherManagerConfiguration.INSTANCE.StationName + " has issued a severe thunderstorm warning for " + _affectedRegions.ConcatToString(",") + " until " + expiryTime.ToString("h:mm tt") + ". .  At " + observationTime.ToString("h:mm tt") + ", doppler radar indicated a severe thunderstorm producing damaging winds and deadly cloud to ground lightning.  This storm was located "
                + Math.Round(closestCity.Item2 / 1000, 1) + " kilometers " + Util.GetDirectionNameFromAngle(closestCity.Item3) + " of " + closestCity.Item1.Name + ", moving " + Util.GetDirectionNameFromAngle(observationAngle) + " at " + Math.Round(observationSpeed, 1) + " meters per second.  ";

            if (_affectedCities.Any())
            {
                text += "This severe thunderstorm will be near " + _affectedCities.ConcatToString(",") + ".  ";
            }
            
            text += "Tornadoes can develop quickly from severe thunderstorms.  Although a tornado is not immediately likely, if a tornado is spotted, act quickly and move to a place of safety inside a sturdy structure, such as a basement or a small interior room.";
            return text;
        }

        public bool QueueForImmediateBroadcast()
        {
            return _isQueued;
        }
        #endregion

        #region Cancellable
        private bool _isCancelled;
        public bool IsCancelled
        {
            get { return _isCancelled; }
            set { _isCancelled = value; InvokeProductUpdatedEvent(new EmptyProductUpdateEventArgs(this)); }
        }

        public string GetCancelSummary()
        {
            return "The severe thunderstorm warning for " + _affectedRegions.ConcatToString(",") + " has been cancelled.";
        }
        #endregion

        #region ISummarizable
        public string GetSummary()
        {
            if (IsCancelled)
            {
                return string.Empty;
            }

            string format = "A severe thunderstorm warning remains in effect until {0} for: {1}. .  At {2}, doppler radar indicated a severe thunderstorm producing damaging winds and deadly cloud to ground lightning " +
                "{3} kilometers {4} of {5} moving {6} at {7} meters per second.  For your protection move to an interior room on the lowest floor of a building.";

            return string.Format(format, expiryTime.ToString("h:mm tt"), _affectedRegions.ConcatToString(","), observationTime.ToString("h:mm tt"), Math.Round(closestCity.Item2 / 1000, 1), Util.GetDirectionNameFromAngle(closestCity.Item3),
                closestCity.Item1.Name, Util.GetDirectionNameFromAngle(observationAngle), Math.Round(observationSpeed, 1));
        }
        #endregion
    }
}
