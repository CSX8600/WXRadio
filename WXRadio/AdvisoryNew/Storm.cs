using System;
using System.Collections.Generic;
using System.Linq;
using WXRadio.WeatherManager.Utility;

namespace WXRadio.WeatherManager
{
    public class Storm
    {
        public event EventHandler<Storm> StormUpdated;
        public event EventHandler<Storm> StormDeleted;
        public List<Coordinate> StormTrack = new List<Coordinate>(5);

        public int ID { get; private set; }
        public Strengths Strength { get; private set; }
        public Coordinate Coordinate { get; private set; }

        public Storm(StormEventInfo stormEventInfo)
        {
            UpdateStorm(stormEventInfo);
        }

        internal void UpdateStorm(StormEventInfo stormEventInfo)
        {
            ID = stormEventInfo.ID;
            Strength = (Strengths)stormEventInfo.Strength;
            Coordinate = stormEventInfo.Coordinate;

            if (StormTrack.Count == 5)
            {
                StormTrack.RemoveAt(0);
            }

            StormTrack.Add(stormEventInfo.Coordinate);            

            StormUpdated?.Invoke(this, this);
        }

        internal void DeleteStorm()
        {
            StormDeleted?.Invoke(this, this);
        }

        public double GetAverageSpeed()
        {
            List<double> lengths = new List<double>();
            for(int i = StormTrack.Count - 1; i > 0; i--)
            {
                Coordinate coordinate1 = StormTrack.ElementAt(i);
                Coordinate coordinate2 = StormTrack.ElementAt(i - 1);

                lengths.Add(Math.Sqrt(Math.Pow(coordinate1.X - coordinate2.X, 2) + Math.Pow(coordinate1.Z - coordinate2.Z, 2)));
            }

            return lengths.Average();
        }

        // Returns motion in degrees in a counter-clockwise fashion.  0 = East
        public double GetCurrentMotion()
        {
            if (StormTrack.Count < 2)
            {
                return 0;
            }

            Coordinate coordinate1 = StormTrack.Last();
            Coordinate coordinate2 = StormTrack.ElementAt(StormTrack.Count - 2);

            double length = Math.Sqrt(Math.Pow(coordinate1.X - coordinate2.X, 2) + Math.Pow(coordinate1.Z - coordinate2.Z, 2));
            double oppositeLength = coordinate1.Z - coordinate2.Z;
            double angleInRadians = Math.Asin(oppositeLength / length);

            double angleInDegrees = angleInRadians * 180 / Math.PI;

            if (coordinate1.X - coordinate2.X < 0)
            {
                angleInDegrees = 180 - angleInDegrees;
            }

            while(angleInDegrees >= 360)
            {
                angleInDegrees -= 360;
            }

            while(angleInDegrees < 0)
            {
                angleInDegrees += 360;
            }

            return angleInDegrees;
        }

        public Coordinate GetPositionInFuture(int steps)
        {
            if (StormTrack.Count < 2)
            {
                return new Coordinate();
            }

            Coordinate lastCoordinate = StormTrack.Last();
            double compensatedSpeed = GetAverageSpeed() * steps;
            double motion = GetCurrentMotion() * Math.PI / 180;

            Coordinate coordinate = new Coordinate();
            coordinate.X = compensatedSpeed * Math.Cos(motion) + lastCoordinate.X;
            coordinate.Z = compensatedSpeed * Math.Sin(motion) + lastCoordinate.Z;

            return coordinate;
        }

        public enum Strengths
        {
            Rain = 0,
            Thunderstorm,
            SevereThunderstorm,
            SevereThunderstormWithHail,
            TornadoF0,
            TornadoF1,
            TornadoF2,
            TornadoF3,
            TornadoF4,
            TornadoF5,
        }
    }
}
