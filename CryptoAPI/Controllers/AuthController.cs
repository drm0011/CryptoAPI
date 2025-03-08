using CryptoAPI.Core.DTOs;
using CryptoAPI.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CryptoAPI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) => _authService = authService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = await _authService.RegisterAsync(userDto);
            return Ok(new { token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = await _authService.LoginAsync(loginDto);
            return Ok(new { token });
        }
    }
}
