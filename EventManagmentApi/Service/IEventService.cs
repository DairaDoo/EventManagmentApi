using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagmentApi.Models;

namespace EventManagmentApi.Service
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> GetAllEventsAsync(); // Get all events
        Task<Event> GetEventByIdAsync(int eventId);  // Get event by ID
        Task<int> CreateEventAsync(Event newEvent);  // Create event
        Task<bool> UpdateEventAsync(Event updatedEvent); // Update event
        Task<bool> DeleteEventAsync(int eventId);  // Delete event
    }
}
