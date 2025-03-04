using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using EventManagmentApi.Service.Interfaces;
using Microsoft.Extensions.Configuration;

namespace EventManagmentApi.Service
{
    public class MapboxGeolocationService : IMapboxGeolocationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _mapboxToken;
        private const string BaseUrl = "https://api.mapbox.com/geocoding/v5/mapbox.places/";

        public MapboxGeolocationService(IConfiguration configuration, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _mapboxToken = configuration["MapboxSettings:AccessToken"];
        }

        public async Task<(double Longitude, double Latitude)> GetCoordinatesAsync(string address)
        {
            var encodedAddress = Uri.EscapeDataString(address);
            var url = $"{BaseUrl}{encodedAddress}.json?access_token={_mapboxToken}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<MapboxGeocodeResponse>(content);

            if (result?.Features?.Length > 0)
            {
                var coordinates = result.Features[0].Center;
                return (coordinates[0], coordinates[1]);
            }

            throw new Exception("Could not find coordinates for the given address");
        }

        public async Task<string> GetFormattedAddressAsync(double longitude, double latitude)
        {
            var url = $"{BaseUrl}{longitude},{latitude}.json?access_token={_mapboxToken}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<MapboxGeocodeResponse>(content);

            return result?.Features?[0]?.PlaceName ?? "Unknown Location";
        }

        // Clases para deserializar la respuesta de Mapbox
        public class MapboxGeocodeResponse
        {
            public MapboxFeature[] Features { get; set; }
        }

        public class MapboxFeature
        {
            public double[] Center { get; set; }
            public string PlaceName { get; set; }
        }
    }
}