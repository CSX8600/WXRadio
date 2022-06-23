using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WXRadio.WeatherManager.Utility
{
    public class Util
    {
        public static double RotateCW(double currentAngle)
        {
            double workingAngle = currentAngle;
            workingAngle -= 90;

            if (workingAngle < 0)
            {
                workingAngle += 360;
            }

            return workingAngle;
        }

        public static double RotateCCW(double currentAngle)
        {
            double workingAngle = currentAngle;
            workingAngle += 90;
            
            if (workingAngle >= 360)
            {
                workingAngle -= 360;
            }

            return workingAngle;
        }

        public static Coordinate GetCoordinateInDistance(Coordinate startingCoordinate, double angle, double length)
        {
            Coordinate newCoordinate = new Coordinate();
            newCoordinate.X = (length * Math.Cos(angle * Math.PI / 180)) + startingCoordinate.X;
            newCoordinate.Z = (length * Math.Sin(angle * Math.PI / 180)) + startingCoordinate.Z;

            return newCoordinate;
        }

        public static string GetDirectionNameFromAngle(double angle)
        {
            while (angle < 0)
            {
                angle += 360;
            }

            while (angle >= 360)
            {
                angle -= 360;
            }

            if (angle > 332.5 || angle < 22.5)
            {
                return "East";
            }

            if (angle >= 22.5 && angle <= 67.5)
            {
                return "Southeast";
            }

            if (angle > 67.5 && angle < 112.5)
            {
                return "South";
            }

            if (angle >= 112.5 && angle <= 157.5)
            {
                return "Southwest";
            }

            if (angle > 157.5 && angle < 202.5)
            {
                return "West";
            }

            if (angle >= 202.5 && angle <= 247.5)
            {
                return "Northwest";
            }

            if (angle > 247.5 && angle < 292.5)
            {
                return "North";
            }

            if (angle >= 292.5 && angle <= 332.5)
            {
                return "Northeast";
            }

            return "Unknown";
        }

        public static bool CoordinateIsInPolygon(Coordinate coordinate, IEnumerable<Coordinate> polygon)
        {
            bool result = false;
            int j = polygon.Count() - 1;
            for (int i = 0; i < polygon.Count(); i++)
            {
                if (polygon.ElementAt(i).Z < coordinate.Z && polygon.ElementAt(j).Z >= coordinate.Z || polygon.ElementAt(j).Z < coordinate.Z && polygon.ElementAt(i).Z >= coordinate.Z)
                {
                    if (polygon.ElementAt(i).X + (coordinate.Z - polygon.ElementAt(i).Z) / (polygon.ElementAt(j).Z - polygon.ElementAt(i).Z) * (polygon.ElementAt(j).X - polygon.ElementAt(i).X) < coordinate.X)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }
    }
}
