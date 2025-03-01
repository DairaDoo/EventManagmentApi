using EventManagmentApi.Models;

namespace EventManagmentApi.Data.Interfaces
{
    // Metodos que usarán los Eventos
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllEventsAsync();
        Task<Event> GetEventByIdAsync(int eventId);
        Task<int> CreateEventAsync(Event newEvent);
        Task<bool> UpdateEventAsync(Event updatedEvent);
        Task<bool> DeleteEventAsync(int eventId);
    }
}
