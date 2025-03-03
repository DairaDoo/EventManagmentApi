using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagmentApi.Models;
using EventManagmentApi.Models.DTOs;

namespace EventManagmentApi.Service.Interfaces
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> GetAllEventsAsync();
        Task<Event> GetEventByIdAsync(int id);
        Task<int> CreateEventAsync(Event newEvent);
        Task<int> CreateEventWithImageAsync(EventCreateDto eventDto);
        Task<bool> UpdateEventAsync(Event updatedEvent);
        Task<bool> UpdateEventWithImageAsync(int id, EventCreateDto eventDto);
        Task<bool> DeleteEventAsync(int id);

    }
}
