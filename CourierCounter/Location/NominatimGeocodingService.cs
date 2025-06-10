using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace CourierCounter.Location
{
    public class NominatimGeocodingService : INominatimGeocodingService
    {
        private readonly HttpClient _httpClient;

        public NominatimGeocodingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(double lat, double lng)?> GeocodeAddressAsync(string address)
        {
            var url = $"https://nominatim.openstreetmap.org/search?format=json&q={Uri.EscapeDataString(address)}";

            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("CourierCounterApp/1.0");
            var response = await _httpClient.GetStringAsync(url);
            var results = JsonSerializer.Deserialize<List<NominatimResult>>(response);

            if (results != null && results.Any())
            {
                var first = results.First();
                return (double.Parse(first.lat), double.Parse(first.lon));
            }

            return null;
        }

        private class NominatimResult
        {
            public string lat { get; set; }
            public string lon { get; set; }
        }

        public async Task<string> ReverseGeocodeAsync(double latitude, double longitude)
        {
            var url = $"https://nominatim.openstreetmap.org/reverse?format=json&lat={latitude}&lon={longitude}&addressdetails=1&accept-language=en&zoom=18";

            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("CourierCounterApp/1.0");

            try
            {
                var response = await _httpClient.GetStringAsync(url);
                var result = JsonSerializer.Deserialize<ReverseGeocodeResult>(response);

                if (result?.address != null)
                {
                    var addressParts = new List<string>();

                    if (!string.IsNullOrWhiteSpace(result.address.road))
                        addressParts.Add(result.address.road);

                    if (!string.IsNullOrWhiteSpace(result.address.suburb))
                        addressParts.Add(result.address.suburb);

                    if (!string.IsNullOrWhiteSpace(result.address.neighbourhood))
                        addressParts.Add(result.address.neighbourhood);

                    if (!string.IsNullOrWhiteSpace(result.address.city_district))
                        addressParts.Add(result.address.city_district);
                        
                    return string.Join(", ", addressParts);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Reverse geocoding failed: {ex.Message}");
            }

            return $"{latitude}, {longitude}";
        }

        private class ReverseGeocodeResult
        {
            [JsonPropertyName("display_name")]
            public string display_name { get; set; }

            public Address address { get; set; }
        }

        private class Address
        {
            public string road { get; set; }
            public string suburb { get; set; }
            public string neighbourhood { get; set; }

            [JsonPropertyName("city_district")]
            public string city_district { get; set; }

            public string house_number { get; set; }
            public string city { get; set; }
            public string town { get; set; }
            public string village { get; set; }
            public string county { get; set; }
            public string country { get; set; }
        }
    }
}
