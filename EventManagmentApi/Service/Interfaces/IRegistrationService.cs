using EventManagmentApi.Models;

namespace EventManagmentApi.Service.Interfaces
{
    public interface IRegistrationService
    {
        Task<IEnumerable<Registration>> GetAllRegistrationsAsync();
        Task<Registration> GetRegistrationByIdAsync(int id);
        Task<IEnumerable<Registration>> GetRegistrationsByUserIdAsync(int userId);
        Task<IEnumerable<Registration>> GetRegistrationsByEventIdAsync(int eventId);
        Task<int> CreateRegistrationAsync(Registration registration);
        Task<bool> DeleteRegistrationAsync(int id);
        Task<bool> UpdateRegistrationStatusAsync(int id, string status);
    }

}
