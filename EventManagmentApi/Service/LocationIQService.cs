using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using EventManagmentApi.Models;
using EventManagmentApi.Service.Interfaces;

namespace EventManagmentApi.Service
{
    public class LocationIQService : IGeocodingService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LocationIQService> _logger;

        public LocationIQService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<LocationIQService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
            _apiKey = _configuration["LocationIQ:ApiKey"];

            if (string.IsNullOrEmpty(_apiKey))
            {
                _logger.LogError("LocationIQ API key is missing from configuration");
                throw new ArgumentNullException("LocationIQ API key is missing from configuration");
            }

            _logger.LogInformation("LocationIQService initialized with API key");
        }

        public async Task<GeoLocation> GetCoordinatesAsync(string address)
        {
            _logger.LogInformation($"GetCoordinatesAsync called with address: {address}");

            if (string.IsNullOrWhiteSpace(address))
            {
                _logger.LogWarning("Address is empty or null");
                return null;
            }

            try
            {
                var encodedAddress = Uri.EscapeDataString(address);
                var requestUrl = $"https://us1.locationiq.com/v1/search.php?key={_apiKey}&q={encodedAddress}&format=json";

                _logger.LogInformation($"Calling LocationIQ API for address: {address}");

                // Use a more direct approach with HttpClient
                var response = await _httpClient.GetAsync(requestUrl);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"LocationIQ API returned error. Status: {response.StatusCode}, Content: {errorContent}");
                    return null;
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"LocationIQ API raw response: {responseContent}");

                var locationResults = System.Text.Json.JsonSerializer.Deserialize<LocationIQResponse[]>(responseContent);

                if (locationResults != null && locationResults.Length > 0)
                {
                    _logger.LogInformation($"Successfully geocoded: {address} to ({locationResults[0].Lat}, {locationResults[0].Lon})");

                    return new GeoLocation
                    {
                        Latitude = double.Parse(locationResults[0].Lat),
                        Longitude = double.Parse(locationResults[0].Lon),
                        DisplayName = locationResults[0].DisplayName
                    };
                }

                _logger.LogWarning($"No results returned from LocationIQ for address: {address}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error geocoding address {address}: {ex.Message}");
                _logger.LogError($"Stack trace: {ex.StackTrace}");
                return null;
            }
        }
    }
}