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
        private readonly IPortfolioRepository _portfolioRepo; // does this belong here?
        private readonly IConfiguration _config;

        public AuthService(IUserRepository userRepo, IPortfolioRepository portfolioRepo, IConfiguration config)
        {
            _userRepo = userRepo;
            _portfolioRepo = portfolioRepo;
            _config = config;
        }

        public async Task<string> RegisterAsync(User userRegister) 
        {
            if (await _userRepo.GetByUsernameAsync(userRegister.Username) != null)
                throw new InvalidOperationException("User already exists");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(userRegister.Password);
            var user = new User { Username = userRegister.Username, Password = passwordHash };

            await _userRepo.AddUserAsync(user);

            var savedUser = await _userRepo.GetByUsernameAsync(user.Username);

            await _portfolioRepo.CreatePortfolioAsync(savedUser.Id);

            return GenerateJwtToken(savedUser);
        }

        public async Task<string> LoginAsync(User loginUser)
        {
            var user = await _userRepo.GetByUsernameAsync(loginUser.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginUser.Password, user.Password))
                throw new UnauthorizedAccessException("Invalid username or password");

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
