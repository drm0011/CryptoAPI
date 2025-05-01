using CryptoAPI.Core.DTOs;
using CryptoAPI.Core.Interfaces;
using CryptoAPI.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CryptoAPI.Controllers
{
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) => _authService = authService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User //use automapper
            {
                Username = userDto.Username,
                Password = userDto.Password
            };

            var token = await _authService.RegisterAsync(user);
            return Ok(new { token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User
            {
                Username = loginDto.Username,
                Password = loginDto.Password
            };

            var token = await _authService.LoginAsync(user);
            return Ok(new { token });
        }
    }
}
