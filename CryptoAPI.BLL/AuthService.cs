using CryptoAPI.Core.Interfaces;
using CryptoAPI.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAPI.BLL
{
    public class AuthService:IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository userRepo, IConfiguration config)
        {
            _userRepo = userRepo;
            _config = config;
        }

        public async Task<string> RegisterAsync(User userDto)
        {
            if (await _userRepo.GetByUsernameAsync(userDto.Username) != null)
                throw new Exception("User already exists");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            var user = new User { Username = userDto.Username, Password = passwordHash };

            await _userRepo.AddUserAsync(user);
            return GenerateJwtToken(user);
        }

        public async Task<string> LoginAsync(User loginDto)
        {
            var user = await _userRepo.GetByUsernameAsync(loginDto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
                throw new Exception("Invalid username or password");

            return GenerateJwtToken(user);
        }


        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                new List<Claim> { new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),},
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
