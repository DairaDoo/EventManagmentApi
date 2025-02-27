using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using EventManagmentApi.Helpers;
using EventManagmentApi.Models;
using Npgsql;


namespace EventManagmentApi.Data.Repositories
{
    public class EventRepository: IEventRepository
    {
        private readonly string _connectionString;

        // Injectamos dependencia del connectionString para establecer la conexión con la base de datos.
        public EventRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection Connection => new NpgsqlConnection(_connectionString);

        // Get All Events
        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            using var connection = Connection;
            return await connection.QueryAsync<Event>(SqlQueries.GetAllEvents);
        }

        // Get Event By Id
        public async Task<Event> GetEventByIdAsync(int id)
        {
            using var connection = Connection;
            return await connection.QueryFirstOrDefaultAsync<Event>(SqlQueries.GetEventById, new { Id = id });
        }

        // Create Event 
        public async Task<int> CreateEventAsync(Event evt)
        {
            using var connection = Connection;
            return await connection.ExecuteScalarAsync<int>(SqlQueries.CreateEvent, evt);
        }
        
        // Update Event
        public async Task<bool> UpdateEventAsync(Event evt)
        {
            using var connection = Connection;
            var affectedRows = await connection.ExecuteAsync(SqlQueries.UpdateEvent, evt);
            return affectedRows > 0;
        }

        // Delete Event
        public async Task<bool> DeleteEventAsync(int id)
        {
            using var connection = Connection;
            var affectedRows =  await connection.ExecuteAsync(SqlQueries.DeleteEvent, new {Id = id});
            return affectedRows > 0;
        }

    }
}
