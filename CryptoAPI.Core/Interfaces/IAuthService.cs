using CryptoAPI.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAPI.Core.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(UserDto userDto);
        Task<string> LoginAsync(LoginDto loginDto);
    }
}
