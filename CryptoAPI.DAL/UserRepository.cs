using AutoMapper;
using CryptoAPI.Core.Interfaces;
using CryptoAPI.Core.Models;
using CryptoAPI.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CryptoAPI.DAL
{
    public class UserRepository : IUserRepository
    {
        private readonly CryptoAPIContext _context;
        private readonly IMapper _mapper;

        public UserRepository(CryptoAPIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Core.Models.User?> GetByUsernameAsync(string username)
        {
            var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            return _mapper.Map<Core.Models.User>(userEntity); 
        }

        public async Task AddUserAsync(Core.Models.User user)
        {
            var userEntity = _mapper.Map<DAL.Entities.User>(user);
            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();
        }
    }
}
