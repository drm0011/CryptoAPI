using CryptoAPI.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAPI.Core.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(User user); //change to domainmodel
        Task<string> LoginAsync(User user);
    }
}
