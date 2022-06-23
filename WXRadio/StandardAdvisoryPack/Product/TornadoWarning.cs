using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WXRadio.WeatherManager;
using WXRadio.WeatherManager.Extensions;
using WXRadio.WeatherManager.Product;
using WXRadio.WeatherManager.Utility;

namespace StandardAdvisoryPack.Product
{
    public class TornadoWarning : BaseProduct, IEmergency, ICancellable, ISummarizable
    {
        private List<Coordinate> _polygon;
        private List<string> _affectedRegions;
        private List<string> _affectedCities;
        private DateTime expiryTime;
        private DateTime observationTime;
        private Tuple<WeatherManagerConfiguration.City, double, double> referenceCity;
        private double observedAngle;
        private double observedSpeed;
        public override Coordinate[] GetPolygonCoordinates()
        {
            if (IsCancelled)
            {
                return new Coordinate[0];
            }

            return _polygon.ToArray();
        }

        public TornadoWarning(Storm storm) : base()
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
            expiryTime = observationTime.AddSeconds(200);
            referenceCity = GetReferenceCity(storm.Coordinate);
            observedAngle = storm.GetCurrentMotion();
            observedSpeed = Math.Round(storm.GetAverageSpeed(), 1);
        }

        public override string GetDetailedInformation()
        {
            DetailedProductBuilder detailedProductBuilder = new DetailedProductBuilder();
            detailedProductBuilder.HeaderText.Add("TORNADO WARNING");

            detailedProductBuilder.Sections.Add(new DetailedProductBuilder.Section()
            {
                Details = new List<string>() { "The national weather service in " + WeatherManagerConfiguration.INSTANCE.StationName + " has issued a" }
            });

            DetailedProductBuilder.Section affectedAreas = new DetailedProductBuilder.Section()
            {
                Details = new List<string>() { "Tornado warning for..." },
                FormattingOption = DetailedProductBuilder.Section.FormattingOptions.IndentedStar | DetailedProductBuilder.Section.FormattingOptions.FormatFirstLineOnly
            };
            affectedAreas.Details.AddRange(_affectedRegions);
            detailedProductBuilder.Sections.Add(affectedAreas);

            detailedProductBuilder.Sections.Add(new DetailedProductBuilder.Section()
            {
                Details = new List<string>() { "Until " + expiryTime.ToString("h:mm tt") },
                FormattingOption = DetailedProductBuilder.Section.FormattingOptions.IndentedStar
            });

            detailedProductBuilder.Sections.Add(new DetailedProductBuilder.Section()
            {
                Details = new List<string>() { "At " + observationTime.ToString("h:mm tt") + "...Doppler Radar indicated a tornado " + Math.Round(referenceCity.Item2 / 1000, 1) + " kilometers " + Util.GetDirectionNameFromAngle(referenceCity.Item3) + " of " + referenceCity.Item1.Name + "...moving " + Util.GetDirectionNameFromAngle(observedAngle) + " at " + observedSpeed + " m/s." },
                FormattingOption = DetailedProductBuilder.Section.FormattingOptions.IndentedStar
            });

            detailedProductBuilder.Sections.Add(new DetailedProductBuilder.Section()
            {
                Details = new List<string>() { "Hazard...Tornado" },
                FormattingOption = DetailedProductBuilder.Section.FormattingOptions.IndentedStar
            });

            detailedProductBuilder.Sections.Add(new DetailedProductBuilder.Section()
            {
                Details = new List<string>() { "Source...Radar Indicated" },
                FormattingOption = DetailedProductBuilder.Section.FormattingOptions.IndentedStar
            });

            detailedProductBuilder.Sections.Add(new DetailedProductBuilder.Section()
            {
                Details = new List<string>() { "Impact...Flying debris will be dangerous to those caught without shelter.  Mobile homes will be damaged or destroyed.  Damage to roofs...windows and vehicles will occur.  Tree damage is likely." },
                FormattingOption = DetailedProductBuilder.Section.FormattingOptions.IndentedStar
            });

            if (_affectedCities.Any())
            {
                DetailedProductBuilder.Section affectedCities = new DetailedProductBuilder.Section()
                {
                    Details = new List<string>() { "The tornado will be near..." },
                    FormattingOption = DetailedProductBuilder.Section.FormattingOptions.IndentedStar | DetailedProductBuilder.Section.FormattingOptions.FormatFirstLineOnly
                };
                affectedCities.Details.AddRange(_affectedCities);
                detailedProductBuilder.Sections.Add(affectedCities);
            }

            detailedProductBuilder.Sections.Add(new DetailedProductBuilder.Section()
            {
                Header = "Precuationary/Preparedness Actions...",
                Details = new List<string>() { "Take cover now!  Move to an interior room on the lowest floor of a sturdy building.  Avoid windows.  If in a mobile home...a vehicle or outdoors...move to the closest substantial shelter and protect yourself from flying debris." }
            });

            return detailedProductBuilder.Build();
        }

        #region IEmergency
        public EmergencyNotificationTypes EmergencyNotificationType => EmergencyNotificationTypes.WarnPublicConsistently;

        private bool _isQueued = true;
        public void AcknowledgeImmediateBroadcastQueue()
        {
            _isQueued = false;
        }

        public bool QueueForImmediateBroadcast()
        {
            return _isQueued;
        }

        public string ImmediateBroadcastInformation()
        {
            string format = "The national weather service in {0} has issued a tornado warning for: {1} until {2}. .  At {3}, doppler radar confirmed a tornado {4} kilometers {5} of {6} and moving {7} at {8} meters per second.  ";

            if (_affectedCities.Count > 0)
            {
                format += "  Locations impacted include: {9}.  ";
            }

            format += "  Take cover now!  Move to an interior room on the lowest floor of a sturdy building.  Avoid windows.  If in a mobile home, a vehicle, or outdoors, move to the closest substantial shelter and protect yourself from flying debris.";

            return string.Format(format, WeatherManagerConfiguration.INSTANCE.StationName, _affectedRegions.ConcatToString(","), expiryTime.ToString("h:mm tt"), observationTime.ToString("h:mm tt"), Math.Round(referenceCity.Item2 / 1000, 2),
                Util.GetDirectionNameFromAngle(referenceCity.Item3), referenceCity.Item1.Name, Util.GetDirectionNameFromAngle(observedAngle), observedSpeed, _affectedCities.ConcatToString(","));
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
            return $"The tornado warning for " + _affectedRegions.ConcatToString(",") + " has been cancelled.";
        }
        #endregion

        #region ISummariable
        public string GetSummary()
        {
            if (IsCancelled)
            {
                return string.Empty;
            }

            string format = "A tornado warning remains in effect until {0} for the following regions: {1}.  At {2}, doppler radar confirmed a damaging tornado {3} kilomters {4} of {5} moving {6} at {7} meters per second.  ";

            if (_affectedCities.Any())
            {
                format += "The tornado will be near {8}.  ";
            }

            format += "To repeat, a tornado is on the ground.  Take cover now.";

            return string.Format(format, expiryTime.ToString("h:mm tt"), _affectedRegions.ConcatToString(","), observationTime.ToString("h:mm tt"), Math.Round(referenceCity.Item2 / 1000, 1),
                Util.GetDirectionNameFromAngle(referenceCity.Item3), referenceCity.Item1.Name, Util.GetDirectionNameFromAngle(observedAngle), Math.Round(observedSpeed, 1), _affectedCities.ConcatToString(","));
        }
        #endregion
    }
}
