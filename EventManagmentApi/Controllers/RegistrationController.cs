using EventManagmentApi.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EventManagmentApi.Models;

namespace EventManagmentApi.Controllers
{
    [Route("api/registration")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;

        public RegistrationController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        // Obtener todas las registraciones
        [HttpGet]
        public async Task<IActionResult> GetAllRegistrations()
        {
            var registrations = await _registrationService.GetAllRegistrationsAsync();
            return Ok(registrations);
        }

        // Obtener registración por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRegistrationById(int id)
        {
            var registration = await _registrationService.GetRegistrationByIdAsync(id);
            if (registration == null)
            {
                return NotFound();
            }
            return Ok(registration);
        }

        // Obtener registraciones por usuario
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetRegistrationsByUserId(int userId)
        {
            var registrations = await _registrationService.GetRegistrationsByUserIdAsync(userId);
            return Ok(registrations);
        }

        // Obtener registraciones por evento
        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetRegistrationsByEventId(int eventId)
        {
            var registrations = await _registrationService.GetRegistrationsByEventIdAsync(eventId);
            return Ok(registrations);
        }

        // Crear una nueva registración
        [HttpPost]
        public async Task<IActionResult> CreateRegistration([FromBody] Registration registration)
        {
            var registrationId = await _registrationService.CreateRegistrationAsync(registration);
            return CreatedAtAction(nameof(GetRegistrationById), new { id = registrationId }, registration);
        }

        // Eliminar una registración
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegistration(int id)
        {
            var result = await _registrationService.DeleteRegistrationAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        // Actualizar el estado de una registración
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateRegistrationStatus(int id, [FromBody] string status)
        {
            var result = await _registrationService.UpdateRegistrationStatusAsync(id, status);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }

}
