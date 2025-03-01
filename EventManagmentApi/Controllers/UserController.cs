using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagmentApi.Models;
using EventManagmentApi.Service;
using EventManagmentApi.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace EventManagmentApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public UserController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest loginRequest)
        {
            var token = await _authService.AuthenticateAsync(loginRequest.Username, loginRequest.Password);
            if (token == null)
            {
                return Unauthorized(new { message = "Invalid Credentials" });
            }

            return Ok(new { Token = token });
        }


        // Obtener todos los usuarios
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch
            {
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        // Obtener usuario por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);

                if (user == null)
                    return NotFound(new { message = "Usuario no encontrado." });

                return Ok(user);
            }
            catch
            {
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        // Obtener usuario por Username
        [HttpGet("username/{username}")]
        public async Task<ActionResult<User>> GetUserByUsername(string username)
        {
            try
            {
                var user = await _userService.GetUserByUsernameAsync(username);
                if (user == null)
                    return NotFound(new { message = "Usuario no encontrado." });

                return Ok(user);
            }
            catch
            {
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        // Crear Usuario
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            if (user == null)
                return BadRequest(new { message = "Datos inválidos." });

            try
            {
                var userId = await _userService.CreateUserAsync(user);
                user.Id = userId;
                return CreatedAtAction(nameof(GetUserById), new { id = userId }, user);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Actualizar Usuario
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            if (user == null || id != user.Id)
                return BadRequest(new { message = "Datos inválidos." });

            try
            {
                var result = await _userService.UpdateUserAsync(user);
                if (!result)
                    return NotFound(new { message = "Usuario no encontrado." });

                return NoContent();
            }
            catch
            {
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        // Eliminar Usuario
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(id);
                if (!result)
                    return NotFound(new { message = "Usuario no encontrado." });

                return NoContent();
            }
            catch
            {
                return StatusCode(500, "Error interno del servidor.");
            }
        }
    }
}
