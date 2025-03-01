using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagmentApi.Data.Interfaces;
using EventManagmentApi.Models;
using EventManagmentApi.Service.Interfaces;

namespace EventManagmentApi.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        // injectamos dependencia de IUserRepository
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Obtener todos los usuarios
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        // Obtener usuario por ID
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        // Obtener usuario por su username
        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _userRepository.GetUserByUsernameAsync(username);
        }

        // Crear un usuario
        public async Task<int> CreateUserAsync(User user)
        {
            var existingUserByUsername = await _userRepository.GetUserByUsernameAsync(user.Username);

            // validaciones de que el usuario no exista.
            if (existingUserByUsername != null)
            {
                throw new System.Exception("This username already exists.");
            }

            var existingUserByEmail = await _userRepository.GetUserByUsernameAsync(user.Email);
            if (existingUserByEmail != null) 
            {
                throw new System.Exception("This email is already in use.");
            }


            return await _userRepository.CreateUserAsync(user);
        }

        // Actualizar un usuario
        public async Task<bool> UpdateUserAsync(User user)
        {
            return await _userRepository.UpdateUserAsync(user);
        }

        // Eliminar un usuario
        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _userRepository.DeleteUserAsync(id);
        }

    }
}
