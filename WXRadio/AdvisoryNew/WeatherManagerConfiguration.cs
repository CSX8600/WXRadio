using System.Collections.Generic;
using System.Xml.Serialization;
using WXRadio.WeatherManager.Utility;

namespace WXRadio.WeatherManager
{
    public class WeatherManagerConfiguration
    {
        public static WeatherManagerConfiguration INSTANCE { get; internal set; }
        internal WeatherManagerConfiguration() { }
        public string StationName { get; set; } = "";
        public List<Region> Regions { get; set; } = new List<Region>();
        public List<City> Cities { get; set; } = new List<City>();
        public class Region
        {
            [XmlAttribute]
            public int X1 { get; set; }
            [XmlAttribute]
            public int Y1 { get; set; }
            [XmlAttribute]
            public int X2 { get; set; }
            [XmlAttribute]
            public int Y2 { get; set; }
            [XmlAttribute]
            public int Channel { get; set; }
            [XmlText]
            public string Name { get; set; }

            public IEnumerable<Coordinate> GetCoordinates()
            {
                yield return new Coordinate(X1, Y1);
                yield return new Coordinate(X1, Y2);
                yield return new Coordinate(X2, Y2);
                yield return new Coordinate(X2, Y1);
            }
        }
        public class City
        {
            [XmlAttribute]
            public int X { get; set; }
            [XmlAttribute]
            public int Z { get; set; }
            [XmlAttribute]
            public int CriticalDistance { get; set; }
            [XmlText]
            public string Name { get; set; }
        }
    }
}
