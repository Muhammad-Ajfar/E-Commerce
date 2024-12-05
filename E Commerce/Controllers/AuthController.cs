using E_Commerce.DTOs;
using E_Commerce.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="registerUserDTO">The registration details.</param>
        /// <returns>A response with the registered user details or an error message.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO registerUserDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _authService.RegisterUserAsync(registerUserDTO);

            if (!response.IsSuccess)
            {
                return BadRequest(response.Message);
            }

            return CreatedAtAction(nameof(Register), response.Data);
        }

        /// <summary>
        /// Logs in an existing user.
        /// </summary>
        /// <param name="loginUserDTO">The login details.</param>
        /// <returns>A JWT token or an error message.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO loginUserDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _authService.LoginUserAsync(loginUserDTO);

            if (!response.IsSuccess)
            {
                return Unauthorized(response.Message);
            }

            return Ok(new { Token = response.Data });
        }
    }
}
