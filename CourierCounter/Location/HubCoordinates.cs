namespace CourierCounter.Location
{
    public class HubCoordinates
    {
        public static readonly Dictionary<int, (double Lat, double Lng)> Locations = new()
    {
        { 1, (27.7172, 85.3240) }, // Kathmandu
        { 2, (27.6644, 85.3188) }, // Lalitpur
        { 3, (27.6710, 85.4298) }, // Bhaktapur
        { 4, (27.5500, 85.3000) }  // Outside Valley (Kirtipur)
    };
    }
}
