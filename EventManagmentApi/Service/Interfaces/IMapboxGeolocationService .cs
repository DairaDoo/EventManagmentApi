namespace EventManagmentApi.Service.Interfaces
{
    public interface IMapboxGeolocationService
    {
        Task<(double Longitude, double Latitude)> GetCoordinatesAsync(string address);
        Task<string> GetFormattedAddressAsync(double longitude, double latitude);
    }
}
