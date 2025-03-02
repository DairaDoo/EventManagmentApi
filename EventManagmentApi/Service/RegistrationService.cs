using EventManagmentApi.Data.Interfaces;
using EventManagmentApi.Service.Interfaces;
using EventManagmentApi.Models;

namespace EventManagmentApi.Service
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IRegistrationRepository _registrationRepository;

        // Injectamos dependencia del Registrationrepository
        public RegistrationService(IRegistrationRepository registrationRepository)
        {
            _registrationRepository = registrationRepository;
        }

        // Obtener todos los registros
        public async Task<IEnumerable<Registration>> GetAllRegistrationsAsync()
        {
            return await _registrationRepository.GetAllRegistrationsAsync();
        }


        // Obtener registro por ID
        public async Task<Registration> GetRegistrationByIdAsync(int id)
        {
            return await _registrationRepository.GetRegistrationByIdAsync(id);
        }

        // Obtener Registro por User ID
        public async Task<IEnumerable<Registration>> GetRegistrationsByUserIdAsync(int userId)
        {
            return await _registrationRepository.GetRegistrationsByUserIdAsync(userId);
        }

        // Obtener Registro Por Event ID
        public async Task<IEnumerable<Registration>> GetRegistrationsByEventIdAsync(int eventId)
        {
            return await _registrationRepository.GetRegistrationsByEventIdAsync(eventId);
        }

        // Crear Registro
        public async Task<int> CreateRegistrationAsync(Registration registration)
        {
            return await _registrationRepository.CreateRegistrationAsync(registration);
        }

        // Eliminar Registro
        public async Task<bool> DeleteRegistrationAsync(int id)
        {
            return await _registrationRepository.DeleteRegistrationAsync(id);
        }

        // Actualizar Registro.
        public async Task<bool> UpdateRegistrationStatusAsync(int id, string status)
        {
            return await _registrationRepository.UpdateRegistrationStatusAsync(id, status);
        }
    }

}
