using EventManagmentApi.Helpers;
using Npgsql;
using System.Data;
using EventManagmentApi.Models;
using EventManagmentApi.Data.Interfaces;
using EventManagmentApi.Helpers;
using Dapper;

namespace EventManagmentApi.Data.Repositories
{
    public class RegistrationRepository : IRegistrationRepository
    {
        private readonly string _connectionString;

        // Injectamos dependencia para establecer conexión con la base de datos
        public RegistrationRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection Connection => new NpgsqlConnection(_connectionString);

        // Obtener todos los Registros
        public async Task<IEnumerable<Registration>> GetAllRegistrationsAsync()
        {
            using var connection = Connection;
            return await connection.QueryAsync<Registration>(RegistrationSqlQueries.GetAllRegistrations);
        }

        // Obtener Registro por ID
        public async Task<Registration> GetRegistrationByIdAsync(int id)
        {
            using var connection = Connection;
            return await connection.QueryFirstOrDefaultAsync<Registration>(RegistrationSqlQueries.GetRegistrationById, new { Id = id });
        }

        // Obtener registro por el User ID
        public async Task<IEnumerable<Registration>> GetRegistrationsByUserIdAsync(int userId)
        {
            using var connection = Connection;
            return await connection.QueryAsync<Registration>(RegistrationSqlQueries.GetRegistrationsByUserId, new { UserId = userId });
        }

        // Obtener registro por el Event ID
        public async Task<IEnumerable<Registration>> GetRegistrationsByEventIdAsync(int eventId)
        {
            using var connection = Connection;
            return await connection.QueryAsync<Registration>(RegistrationSqlQueries.GetRegistrationsByEventId, new { EventId = eventId });
        }

        // Crear Registro
        public async Task<int> CreateRegistrationAsync(Registration registration)
        {
            using var connection = Connection;
            return await connection.ExecuteScalarAsync<int>(RegistrationSqlQueries.CreateRegistration, registration);
        }

        // Eliminar Registro
        public async Task<bool> DeleteRegistrationAsync(int id)
        {
            using var connection = Connection;
            var affectedRows = await connection.ExecuteAsync(RegistrationSqlQueries.DeleteRegistration, new { Id = id });
            return affectedRows > 0;
        }

        // Actualizar Registro
        public async Task<bool> UpdateRegistrationStatusAsync(int id, string status)
        {
            using var connection = Connection;
            var affectedRows = await connection.ExecuteAsync(RegistrationSqlQueries.UpdateRegistrationStatus, new { Id = id, Status = status });
            return affectedRows > 0;
        }
    }

}
