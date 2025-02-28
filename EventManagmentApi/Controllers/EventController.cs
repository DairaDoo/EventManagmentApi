using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventManagmentApi.Models;
using EventManagmentApi.Service;

namespace EventManagmentApi.Controllers
{
    [ApiController]
    [Route("api/events")] // ruta de la api
    public class EventController: ControllerBase
    {
        private readonly EventService _eventService;

        // Injectamos el EventService para acceder a la lógica de negocio.
        public EventController(EventService eventService)
        {
            _eventService = eventService;
        }

        // Get All Events
        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _eventService.GetAllEventsAsync();
            return Ok(events);
        }

        // Get Event By ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(int id)
        {
            var evt = await _eventService.GetEventByIdAsync(id); // Get Event
            if (evt == null)
            {
                return NotFound();
            }

            return Ok(evt);
        }

        // Create Event
        [HttpPost]
        public async Task<IActionResult> CreateEvent(Event evt)
        {
            var newEventId = await _eventService.CreateEventAsync(evt);
            return CreatedAtAction(nameof(GetEventById), new {id = newEventId}, evt);
        }

        [HttpPut("{id}")] 
        public async Task<IActionResult> UpdateEvent(int id, Event evt)
        {
            if (id != evt.Id) return BadRequest();
            var updatedEvent = await _eventService.UpdateEventAsync(evt);
            return updatedEvent ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var deletedEvent = await _eventService.DeleteEventAsync(id);
            return deletedEvent ? NoContent() : NotFound();
        }


    }
}
