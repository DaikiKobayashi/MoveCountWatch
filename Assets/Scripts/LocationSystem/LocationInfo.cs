namespace MoveCountWatch
{
    public class LocationInfo
    {
        public double Latitude { get; }
        public double Longitude { get; }

        public LocationInfo(string location)
        {
            var locations = location.Split(',');
            var latitude = double.Parse(locations[0]);
            var longitude = double.Parse(locations[1]);
        
            Latitude = latitude;
            Longitude = longitude;
        }

        public LocationInfo(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public bool Equals(LocationInfo other)
        {
            return Latitude.Equals(other.Latitude) && Longitude.Equals(other.Longitude);
        }
    }
}