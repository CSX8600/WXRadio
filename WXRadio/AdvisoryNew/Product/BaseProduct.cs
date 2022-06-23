using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WXRadio.WeatherManager.Utility;

namespace WXRadio.WeatherManager.Product
{
    public abstract class BaseProduct
    {
        public event EventHandler<IProductUpdateEventArgs> ProductUpdate;
        public event EventHandler<BaseProduct> ProductRemoved;

        private Guid _productGuid;
        public Guid ProductGuid
        {
            get
            {
                if (_productGuid.Equals(new Guid()))
                {
                    _productGuid = Guid.NewGuid();
                }

                return _productGuid;
            }
        }

        private HashSet<WeatherManagerConfiguration.Region> _regions = new HashSet<WeatherManagerConfiguration.Region>();
        public IReadOnlyCollection<WeatherManagerConfiguration.Region> GetAffectedConfigurationRegions()
        {
            return _regions;
        }

        protected BaseProduct() { }
        public virtual Coordinate[] GetPolygonCoordinates() { return new Coordinate[0]; }

        protected virtual void PreProductRemovedEvent() { }
        protected virtual void PostProductRemovedEvent() { }

        protected void InvokeProductUpdatedEvent(IProductUpdateEventArgs e)
        {
            ProductUpdate?.Invoke(this, e);
        }

        /// <summary>
        /// The text version of this product.  Normally seen online.
        /// </summary>
        public abstract string GetDetailedInformation();

        public void RemoveProduct()
        {
            PreProductRemovedEvent();
            ProductRemoved?.Invoke(this, this);
            PostProductRemovedEvent();
        }

        protected class DetailedProductBuilder
        {
            public bool RequestImmediateBroadcast { get; set; }
            public List<string> HeaderText { get; set; } = new List<string>();
            public List<Section> Sections { get; set; } = new List<Section>();
            public class Section
            {
                public string Header { get; set; }
                public List<string> Details { get; set; } = new List<string>();
                [Flags]
                public enum FormattingOptions
                {
                    None = 0,
                    IndentedStar = 1,
                    FormatFirstLineOnly = 2
                }
                public FormattingOptions FormattingOption { get; set; }

                public string Build()
                {
                    StringBuilder sectionBuilder = new StringBuilder();

                    if (!string.IsNullOrEmpty(Header))
                    {
                        sectionBuilder.AppendLine(Header.ToUpper());
                        sectionBuilder.AppendLine();
                    }

                    for (int i = 0; i < Details.Count; i++)
                    {
                        string detail = Details.ElementAt(i);

                        if (FormattingOption.HasFlag(FormattingOptions.IndentedStar) && (i == 0 || !FormattingOption.HasFlag(FormattingOptions.FormatFirstLineOnly)))
                        {
                            sectionBuilder.Append("*    ");
                        }

                        sectionBuilder.AppendLine(detail);
                    }

                    return sectionBuilder.ToString();
                }
            }

            public string Build()
            {
                StringBuilder builder = new StringBuilder();
                if (RequestImmediateBroadcast)
                {
                    builder.AppendLine("URGENT - IMMEDIATE BROADCAST REQUESTED");
                }

                foreach(string headerText in HeaderText)
                {
                    builder.AppendLine(headerText.ToUpper());
                }

                builder.AppendLine($"NATIONAL WEATHER SERVICE {WeatherManagerConfiguration.INSTANCE.StationName?.ToUpper()}");
                builder.AppendLine();

                foreach (Section section in Sections)
                {
                    builder.AppendLine(section.Build());
                    builder.AppendLine();
                }

                return builder.ToString();
            }
        }

        protected IEnumerable<string> GetAffectedRegions()
        {
            Coordinate[] polygon = GetPolygonCoordinates();
            if (polygon.Length != 4)
            {
                yield break;
            }

            foreach (WeatherManagerConfiguration.Region region in WeatherManagerConfiguration.INSTANCE.Regions)
            {
                bool regionAffected = false;

                int regionWidth = Math.Abs(region.X2 - region.X1);
                int regionHeight = Math.Abs(region.Y2 - region.Y1);

                float widthHalfUnit = regionWidth / 2F;
                float heightHalfUnit = regionHeight / 2F;

                // What part of the region is the polygon in?
                Coordinate[] northwestPolygon = new Coordinate[]
                {
                    new Coordinate(region.X1, region.Y1),
                    new Coordinate(region.X1 + widthHalfUnit, region.Y1),
                    new Coordinate(region.X1 + widthHalfUnit, region.Y1 + heightHalfUnit),
                    new Coordinate(region.X1, region.Y1 + heightHalfUnit)
                };

                bool doesIntersect = false;
                for(int i = 0; i < 4; i++)
                {
                    Coordinate firstCoordinate = northwestPolygon[i];
                    Coordinate secondCoordinate;
                    if (i == 3)
                    {
                        secondCoordinate = northwestPolygon[0];
                    }
                    else
                    {
                        secondCoordinate = northwestPolygon[i + 1];
                    }

                    for(int j = 0; j < 4; j++)
                    {
                        Coordinate polyCoord1 = polygon[j];
                        Coordinate polyCoord2;
                        if (j == 3)
                        {
                            polyCoord2 = polygon[0];
                        }
                        else
                        {
                            polyCoord2 = polygon[j + 1];
                        }

                        doesIntersect = doIntersect(firstCoordinate, secondCoordinate, polyCoord1, polyCoord2);

                        if (doesIntersect)
                        {
                            regionAffected = true;
                            break;
                        }
                    }

                    if (doesIntersect)
                    {
                        break;
                    }
                } 

                if (doesIntersect)
                {
                    yield return "Northwest " + region.Name + " region";
                }

                Coordinate[] northeastPolygon = new Coordinate[]
                {
                    new Coordinate(region.X1 + widthHalfUnit, region.Y1),
                    new Coordinate(region.X2, region.Y1),
                    new Coordinate(region.X2, region.Y1 + heightHalfUnit),
                    new Coordinate(region.X1 + widthHalfUnit, region.Y1 + heightHalfUnit)
                };

                doesIntersect = false;
                for (int i = 0; i < 4; i++)
                {
                    Coordinate firstCoordinate = northeastPolygon[i];
                    Coordinate secondCoordinate;
                    if (i == 3)
                    {
                        secondCoordinate = northeastPolygon[0];
                    }
                    else
                    {
                        secondCoordinate = northeastPolygon[i + 1];
                    }

                    for (int j = 0; j < 4; j++)
                    {
                        Coordinate polyCoord1 = polygon[j];
                        Coordinate polyCoord2;
                        if (j == 3)
                        {
                            polyCoord2 = polygon[0];
                        }
                        else
                        {
                            polyCoord2 = polygon[j + 1];
                        }

                        doesIntersect = doIntersect(firstCoordinate, secondCoordinate, polyCoord1, polyCoord2);

                        if (doesIntersect)
                        {
                            regionAffected = true;
                            break;
                        }
                    }

                    if (doesIntersect)
                    {
                        break;
                    }
                }

                if (doesIntersect)
                {
                    yield return "Northeast " + region.Name + " region";
                }

                Coordinate[] southwestPolygon = new Coordinate[]
                {
                    new Coordinate(region.X1, region.Y1 + heightHalfUnit),
                    new Coordinate(region.X1 + widthHalfUnit, region.Y1 + heightHalfUnit),
                    new Coordinate(region.X1 + widthHalfUnit, region.Y2),
                    new Coordinate(region.X1, region.Y2)
                };

                doesIntersect = false;
                for (int i = 0; i < 4; i++)
                {
                    Coordinate firstCoordinate = southwestPolygon[i];
                    Coordinate secondCoordinate;
                    if (i == 3)
                    {
                        secondCoordinate = southwestPolygon[0];
                    }
                    else
                    {
                        secondCoordinate = southwestPolygon[i + 1];
                    }

                    for (int j = 0; j < 4; j++)
                    {
                        Coordinate polyCoord1 = polygon[j];
                        Coordinate polyCoord2;
                        if (j == 3)
                        {
                            polyCoord2 = polygon[0];
                        }
                        else
                        {
                            polyCoord2 = polygon[j + 1];
                        }

                        doesIntersect = doIntersect(firstCoordinate, secondCoordinate, polyCoord1, polyCoord2);

                        if (doesIntersect)
                        {
                            regionAffected = true;
                            break;
                        }
                    }

                    if (doesIntersect)
                    {
                        break;
                    }
                }

                if (doesIntersect)
                {
                    yield return "Southwest " + region.Name + " region";
                }

                Coordinate[] southeastPolygon = new Coordinate[]
                {
                    new Coordinate(region.X1 + widthHalfUnit, region.Y1 + heightHalfUnit),
                    new Coordinate(region.X2, region.Y1 + heightHalfUnit),
                    new Coordinate(region.X2, region.Y2),
                    new Coordinate(region.X1 + widthHalfUnit, region.Y2)
                };

                doesIntersect = false;
                for (int i = 0; i < 4; i++)
                {
                    Coordinate firstCoordinate = southeastPolygon[i];
                    Coordinate secondCoordinate;
                    if (i == 3)
                    {
                        secondCoordinate = southeastPolygon[0];
                    }
                    else
                    {
                        secondCoordinate = southeastPolygon[i + 1];
                    }

                    for (int j = 0; j < 4; j++)
                    {
                        Coordinate polyCoord1 = polygon[j];
                        Coordinate polyCoord2;
                        if (j == 3)
                        {
                            polyCoord2 = polygon[0];
                        }
                        else
                        {
                            polyCoord2 = polygon[j + 1];
                        }

                        doesIntersect = doIntersect(firstCoordinate, secondCoordinate, polyCoord1, polyCoord2);

                        if (doesIntersect)
                        {
                            regionAffected = true;
                            break;
                        }
                    }

                    if (doesIntersect)
                    {
                        break;
                    }
                }

                if (doesIntersect)
                {
                    yield return "Southeast " + region.Name + " region";
                }

                if (regionAffected)
                {
                    _regions.Add(region);
                }
            }
        }

        protected Tuple<WeatherManagerConfiguration.City, double, double> GetReferenceCity(Coordinate coordinate)
        {
            Tuple<WeatherManagerConfiguration.City, double, double> closestCity = null;
            foreach (WeatherManagerConfiguration.City city in WeatherManagerConfiguration.INSTANCE.Cities)
            {
                double distance = Math.Sqrt(Math.Pow(city.X - coordinate.X, 2) + Math.Pow(city.Z + coordinate.Z, 2));
                if (closestCity == null || closestCity.Item2 > distance)
                {
                    double angle;
                    // Check for undefined
                    if (coordinate.X - city.X == 0)
                    {
                        if (coordinate.Z - city.Z > 0)
                        {
                            angle = 90;
                        }
                        else
                        {
                            angle = 270;
                        }
                    }
                    else
                    {
                        double slope = (city.Z - coordinate.Z) / (city.X - coordinate.X);
                        angle = Math.Atan(slope) * 180 / Math.PI;

                        if (coordinate.X - city.X < 0)
                        {
                            angle += 180;
                        }

                        if (angle >= 360)
                        {
                            angle -= 360;
                        }
                    }
                    closestCity = new Tuple<WeatherManagerConfiguration.City, double, double>(city, distance, angle);
                }
            }

            return closestCity;
        }

        protected IEnumerable<string> GetImpactedCities(Coordinate stormCoordinate, double stormAngle, double stormSpeed, int maxDistance, DateTime currentTime)
        {
            double coefficientA = Math.Sin(stormAngle * Math.PI / 180);
            double coefficientB = -1;
            double coefficientC = coefficientA * stormCoordinate.X - stormCoordinate.Z;

            double denominator = Math.Sqrt(Math.Pow(coefficientA, 2) + Math.Pow(coefficientB, 2));

            // Distance from point to line formula:
            // (|Ax1 + By1 + C|)/(SqRt(A^2 + B^2))
            foreach(WeatherManagerConfiguration.City city in WeatherManagerConfiguration.INSTANCE.Cities)
            {
                double distance = Math.Abs((coefficientA * city.X) + (coefficientB * city.Z) + coefficientC) / denominator;

                if (distance <= city.CriticalDistance)
                {
                    double distanceFromStormToCity = Math.Sqrt(Math.Pow(city.X - stormCoordinate.X, 2) + Math.Pow(city.Z - stormCoordinate.Z, 2));
                    double distanceToCriticalPoint = Math.Sqrt(Math.Pow(distanceFromStormToCity, 2) + Math.Pow(distance, 2));

                    if (distanceToCriticalPoint > maxDistance)
                    {
                        continue;
                    }

                    // How many seconds will it take for the storm to reach the critical point?
                    double time = distanceToCriticalPoint / stormSpeed;
                    DateTime impactTime = currentTime.AddSeconds(time);

                    yield return city.Name + " at " + impactTime.ToString("HH:mm tt");
                }
            }
        }

        #region Line Segment Intersection Magic
        // Given three colinear Coordinates p, q, r, the function checks if 
        // Coordinate q lies on line segment 'pr' 
        private bool onSegment(Coordinate p, Coordinate q, Coordinate r)
        {
            if (q.X <= Math.Max(p.X, r.X) && q.X >= Math.Min(p.X, r.X) &&
                q.Z <= Math.Max(p.Z, r.Z) && q.Z >= Math.Min(p.Z, r.Z))
                return true;

            return false;
        }

        // To find orientation of ordered triplet (p, q, r). 
        // The function returns following values 
        // 0 --> p, q and r are colinear 
        // 1 --> Clockwise 
        // 2 --> Counterclockwise 
        private int orientation(Coordinate p, Coordinate q, Coordinate r)
        {
            // See https://www.geeksforgeeks.org/orientation-3-ordered-Coordinates/ 
            // for details of below formula. 
            int val = (int)((q.Z - p.Z) * (r.X - q.X) -
                    (q.X - p.X) * (r.Z - q.Z));

            if (val == 0) return 0; // colinear 

            return (val > 0) ? 1 : 2; // clock or counterclock wise 
        }

        // The main function that returns true if line segment 'p1q1' 
        // and 'p2q2' intersect. 
        private bool doIntersect(Coordinate p1, Coordinate q1, Coordinate p2, Coordinate q2)
        {
            // Find the four orientations needed for general and 
            // special cases 
            int o1 = orientation(p1, q1, p2);
            int o2 = orientation(p1, q1, q2);
            int o3 = orientation(p2, q2, p1);
            int o4 = orientation(p2, q2, q1);

            // General case 
            if (o1 != o2 && o3 != o4)
                return true;

            // Special Cases 
            // p1, q1 and p2 are colinear and p2 lies on segment p1q1 
            if (o1 == 0 && onSegment(p1, p2, q1)) return true;

            // p1, q1 and q2 are colinear and q2 lies on segment p1q1 
            if (o2 == 0 && onSegment(p1, q2, q1)) return true;

            // p2, q2 and p1 are colinear and p1 lies on segment p2q2 
            if (o3 == 0 && onSegment(p2, p1, q2)) return true;

            // p2, q2 and q1 are colinear and q1 lies on segment p2q2 
            if (o4 == 0 && onSegment(p2, q1, q2)) return true;

            return false; // Doesn't fall in any of the above cases 
        }
        #endregion
    }
}
