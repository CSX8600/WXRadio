using WXRadio.WeatherManager.Utility;

namespace WXRadio.WeatherManager
{
    public class StormEventInfo
    {
        public string Event { get; set; }
        public int ID { get; set; }
        public int Strength { get; set; }
        public double X { get; set; }
        public double Z { get; set; }
        public Coordinate Coordinate
        {
            get
            {
                return new Coordinate(X, Z);
            }
        }
    }
}
