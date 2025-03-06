// Service/EventService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagmentApi.Data.Interfaces;
using EventManagmentApi.Models;
using EventManagmentApi.Models.DTOs;
using EventManagmentApi.Service.Interfaces;

namespace EventManagmentApi.Service
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IPhotoService _photoService;

        public EventService(IEventRepository eventRepository, IPhotoService photoService)
        {
            _eventRepository = eventRepository;
            _photoService = photoService;
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

        public async Task<int> CreateEventWithImageAsync(EventCreateDto eventDto)
        {
            if (eventDto == null || string.IsNullOrWhiteSpace(eventDto.Name))
                throw new ArgumentException("Event data is invalid.");

            var newEvent = new Event
            {
                Name = eventDto.Name,
                Description = eventDto.Description,
                Location = eventDto.Location,
                Date = eventDto.Date,
                Price = eventDto.Price,
                Longitude = eventDto.Longitude,
                Latitude = eventDto.Latitude
            };

            // Subir imagen si existe
            if (eventDto.Image != null)
            {
                newEvent.imageUrl = await _photoService.UploadPhotoAsync(eventDto.Image);
            }

            return await _eventRepository.CreateEventAsync(newEvent);
        }

        public async Task<bool> UpdateEventAsync(Event updatedEvent)
        {
            if (updatedEvent == null || updatedEvent.Id <= 0)
                return false;

            return await _eventRepository.UpdateEventAsync(updatedEvent);
        }

        public async Task<bool> UpdateEventWithImageAsync(int id, EventCreateDto eventDto)
        {
            var existingEvent = await _eventRepository.GetEventByIdAsync(id);

            if (existingEvent == null)
                return false;

            existingEvent.Name = eventDto.Name;
            existingEvent.Description = eventDto.Description;
            existingEvent.Location = eventDto.Location;
            existingEvent.Date = eventDto.Date;
            existingEvent.Price = eventDto.Price;
            existingEvent.Longitude = eventDto.Longitude;
            existingEvent.Latitude = eventDto.Latitude;

            // Subir imagen si existe
            if (eventDto.Image != null)
            {
                existingEvent.imageUrl = await _photoService.UploadPhotoAsync(eventDto.Image);
            }

            return await _eventRepository.UpdateEventAsync(existingEvent);
        }

        public async Task<bool> DeleteEventAsync(int id)
        {
            if (id <= 0)
                return false;

            return await _eventRepository.DeleteEventAsync(id);
        }
    }
}