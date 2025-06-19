using AutoMapper;
using CryptoAPI.DTOs;
using CryptoAPI.Core.Interfaces;
using CryptoAPI.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CryptoAPI.Controllers
{
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _mapper.Map<User>(userDto);

            var token = await _authService.RegisterAsync(user);
            return Ok(new { token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _mapper.Map<User>(loginDto);

            var token = await _authService.LoginAsync(user);
            return Ok(new { token, message ="watchtower success", version="4.0.0" });
        }
    }
}
