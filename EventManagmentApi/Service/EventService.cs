using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagmentApi.Data.Repositories;
using EventManagmentApi.Models;

namespace EventManagmentApi.Service
{
    public class EventService
    {
        private readonly IEventRepository _eventRepository;

        // Injectamos la dependencia IEventRepository para acceder a sus metodos.
        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        // Gell All Events
        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await _eventRepository.GetAllEventsAsync();
        }

        // Get Event By ID
        public async Task<Event> GetEventByIdAsync(int id)
        {
            return await _eventRepository.GetEventByIdAsync(id);
        }

        // Create Event 
        public async Task<int> CreateEventAsync(Event evt)
        {
            return await _eventRepository.CreateEventAsync(evt);
        }

        // Update Event
        public async Task<bool> UpdateEventAsync(Event evt)
        {
            return await _eventRepository.UpdateEventAsync(evt);
        }

        // Delete Event
        public async Task<bool> DeleteEventAsync(int id)
        {
            return await _eventRepository.DeleteEventAsync(id);
        }
    }
}
