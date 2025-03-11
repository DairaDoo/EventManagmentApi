using EventManagmentApi.Models;

namespace EventManagmentApi.Service.Interfaces
{
    public interface IGeocodingService
    {
        Task<GeoLocation> GetCoordinatesAsync(string adress);
    }
}
