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
        private readonly IGeocodingService _geocodingService;
        private readonly ILogger<EventService> _logger;

        public EventService(
           IEventRepository eventRepository,
           IPhotoService photoService,
           IGeocodingService geocodingService,
           ILogger<EventService> logger)
        {
            _eventRepository = eventRepository;
            _photoService = photoService;
            _geocodingService = geocodingService;
            _logger = logger;

            _logger.LogInformation("EventService initialized with GeocodingService");
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

            // Get coordinates if location is provided but coordinates aren't
            if (!string.IsNullOrWhiteSpace(newEvent.Location) &&
                (!newEvent.Latitude.HasValue || !newEvent.Longitude.HasValue))
            {
                await EnrichWithGeocodingDataAsync(newEvent);
            }

            return await _eventRepository.CreateEventAsync(newEvent);
        }

        public async Task<int> CreateEventWithImageAsync(EventCreateDto eventDto)
        {
            if (eventDto == null || string.IsNullOrWhiteSpace(eventDto.Name))
            {
                _logger.LogWarning("Event data is invalid");
                throw new ArgumentException("Event data is invalid.");
            }

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

            // Get coordinates if location is provided
            if (!string.IsNullOrWhiteSpace(newEvent.Location))
            {
                _logger.LogInformation($"Attempting to geocode location: {newEvent.Location}");
                await EnrichWithGeocodingDataAsync(newEvent);
            }

            // Subir imagen si existe
            if (eventDto.Image != null)
            {
                _logger.LogInformation("Uploading image for event");
                newEvent.imageUrl = await _photoService.UploadPhotoAsync(eventDto.Image);
            }

            _logger.LogInformation($"Creating event with geocoded data. Lat: {newEvent.Latitude}, Lng: {newEvent.Longitude}");
            return await _eventRepository.CreateEventAsync(newEvent);
        }

        public async Task<bool> UpdateEventAsync(Event updatedEvent)
        {
            if (updatedEvent == null || updatedEvent.Id <= 0)
                return false;

            // Get coordinates if location is provided but coordinates aren't
            if (!string.IsNullOrWhiteSpace(updatedEvent.Location) &&
                (!updatedEvent.Latitude.HasValue || !updatedEvent.Longitude.HasValue))
            {
                await EnrichWithGeocodingDataAsync(updatedEvent);
            }

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

            // Get coordinates if location is provided but coordinates aren't
            if (!string.IsNullOrWhiteSpace(existingEvent.Location) &&
                (!existingEvent.Latitude.HasValue || !existingEvent.Longitude.HasValue))
            {
                await EnrichWithGeocodingDataAsync(existingEvent);
            }

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

        // Helper method to get geocoding data
        private async Task EnrichWithGeocodingDataAsync(Event eventToEnrich)
        {
            _logger.LogInformation($"EnrichWithGeocodingDataAsync called for location: {eventToEnrich.Location}");

            try
            {
                // Always try to get coordinates if location is provided
                var geoLocation = await _geocodingService.GetCoordinatesAsync(eventToEnrich.Location);

                if (geoLocation != null)
                {
                    _logger.LogInformation($"Successfully geocoded {eventToEnrich.Location} to: {geoLocation.Latitude}, {geoLocation.Longitude}");
                    eventToEnrich.Latitude = geoLocation.Latitude;
                    eventToEnrich.Longitude = geoLocation.Longitude;
                }
                else
                {
                    _logger.LogWarning($"Could not geocode location: {eventToEnrich.Location}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting geocoding data for {eventToEnrich.Location}: {ex.Message}");
                _logger.LogError($"Stack trace: {ex.StackTrace}");
            }
        }
    }
}