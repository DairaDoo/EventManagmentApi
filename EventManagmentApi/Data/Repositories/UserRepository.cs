using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using EventManagmentApi.Data.Interfaces;
using EventManagmentApi.Helpers;
using EventManagmentApi.Models;
using Npgsql;

// aquí se maneja la lógica de las operaciones de base de datos asociadas con los usuarios
namespace EventManagmentApi.Data.Repositories
{
    public class UserRepository: IUserRepository
    {

        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection Connection => new NpgsqlConnection(_connectionString);

        // Obtener todos los usuarios
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            using var connection = Connection;
            return await connection.QueryAsync<User>(UserSqlQueries.GetAllUsers);
        }

        // Obtener un usuario por ID
        public async Task<User> GetUserByIdAsync(int id)
        {
            using var connection = Connection;
            return await connection.QueryFirstOrDefaultAsync<User>(UserSqlQueries.GetUserById, new { Id = id });
        }

        // Obtener un usuario por Username
        public async Task<User> GetUserByUsernameAsync(string username)
        {
            using var connection = Connection;
            return await connection.QueryFirstOrDefaultAsync<User>(UserSqlQueries.GetUserByUsername, new { Username = username });
        }

        // Crear un nuevo usuario
        public async Task<int> CreateUserAsync(User user)
        {
            using var connection = Connection;
            return await connection.ExecuteScalarAsync<int>(UserSqlQueries.CreateUser, user);
        }

        // Actualizar un usuario
        public async Task<bool> UpdateUserAsync(User user)
        {
            using var connection = Connection;
            var affectedRows = await connection.ExecuteAsync(UserSqlQueries.UpdateUser, user);
            return affectedRows > 0;
        }

        // Eliminar un usuario
        public async Task<bool> DeleteUserAsync(int id)
        {
            using var connection = Connection;
            var affectedRows = await connection.ExecuteAsync(UserSqlQueries.DeleteUser, new {Id = id});
            return affectedRows > 0; // si afectedRows hiciera un cambio devolviera 1, si no 0 (false)
        }

    }
}
