using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagmentApi.Data.Repositories;
using EventManagmentApi.Models;

namespace EventManagmentApi.Service
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await _eventRepository.GetAllEventsAsync();
        }

        public async Task<Event> GetEventByIdAsync(int id)
        {
            return await _eventRepository.GetEventByIdAsync(id);
        }

        public async Task<int> CreateEventAsync(Event newEvent)
        {
            if (newEvent == null || string.IsNullOrWhiteSpace(newEvent.Name))
                throw new ArgumentException("Event data is invalid.");

            return await _eventRepository.CreateEventAsync(newEvent);
        }

        public async Task<bool> UpdateEventAsync(Event updatedEvent)
        {
            if (updatedEvent == null || updatedEvent.Id <= 0)
                return false;

            return await _eventRepository.UpdateEventAsync(updatedEvent);
        }

        public async Task<bool> DeleteEventAsync(int id)
        {
            if (id <= 0)
                return false;

            return await _eventRepository.DeleteEventAsync(id);
        }
    }
}
