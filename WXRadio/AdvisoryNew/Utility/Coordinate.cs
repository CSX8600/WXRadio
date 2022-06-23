namespace WXRadio.WeatherManager.Utility
{
    public struct Coordinate
    {
        public double X { get; set; }
        public double Z { get; set; }

        public Coordinate(double x, double z)
        {
            X = x;
            Z = z;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Coordinate))
            {
                return false;
            }

            if (obj == null)
            {
                return false;
            }

            Coordinate coordinate = (Coordinate)obj;
            return coordinate.X == X && coordinate.Z == Z;
        }

        public override int GetHashCode()
        {
            return string.Format("{0}-{1}", X, Z).GetHashCode();
        }
    }
}
