using AutoMapper;
using CryptoAPI.DAL;
using CryptoAPI.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAPI.TestProject
{
    [TestClass]
    public class UserRepositoryTests
    {
        private CryptoAPIContext _context;
        private UserRepository _repository;
        private IMapper _mapper;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<CryptoAPIContext>()
                .UseInMemoryDatabase(databaseName: $"CryptoDb_{Guid.NewGuid()}")
                .Options;

            _context = new CryptoAPIContext(options);

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();

            _repository = new UserRepository(_context, _mapper);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Dispose();
        }

        [TestMethod]
        public async Task GetByUserAsync_ReturnsUser_WhenExists()
        {
            var userEntity = new User {Username = "test", Password = "test" };
            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByUsernameAsync(userEntity.Username);

            Assert.IsNotNull(result);
            Assert.AreEqual("test", result.Username);
        }

        [TestMethod]
        public async Task AddUserAsync_PersistsUser()
        {
            var user = new Core.Models.User { Username = "newuser", Password = "hash" };

            await _repository.AddUserAsync(user);
            var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.Username == "newuser");

            Assert.IsNotNull(userInDb);
            Assert.AreEqual("newuser", userInDb.Username);
        }

    }
}
