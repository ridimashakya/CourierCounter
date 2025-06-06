namespace CourierCounter.Location
{
    public interface INominatimGeocodingService
    {
        Task<(double lat, double lng)?> GeocodeAddressAsync(string address);
        Task<string> ReverseGeocodeAsync(double latitude, double longitude);
    }
}
