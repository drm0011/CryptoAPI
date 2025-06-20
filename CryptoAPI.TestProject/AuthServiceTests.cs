using CryptoAPI.BLL;
using CryptoAPI.Core.Interfaces;
using CryptoAPI.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace CryptoAPI.TestProject
{
    [TestClass]
    public class AuthServiceTests
    {
        private Mock<IUserRepository> _mockUserRepo;
        private Mock<IPortfolioRepository> _mockPortfolioRepo;
        private Mock<IConfiguration> _mockConfig;
        private AuthService _authService;

        [TestInitialize]
        public void Setup()
        {
            _mockUserRepo = new Mock<IUserRepository>();
            _mockPortfolioRepo = new Mock<IPortfolioRepository>();
            _mockConfig = new Mock<IConfiguration>();

            _mockConfig.Setup(c => c["Jwt:Key"]).Returns("supersecurekeythatistotallylongenough");
            _mockConfig.Setup(c => c["Jwt:Issuer"]).Returns("TestIssuer");
            _mockConfig.Setup(c => c["Jwt:Audience"]).Returns("TestAudience");

            _authService = new AuthService(
                _mockUserRepo.Object,
                _mockPortfolioRepo.Object,
                _mockConfig.Object
            );
        }

        [TestMethod]
        public async Task RegisterAsync_ShouldReturnToken_WhenNewUser()
        {
            var input = new User { Username = "newuser", Password = "password123" };
            var savedUser = new User { Id = 1, Username = "newuser", Password = "hashedpwd" };

            _mockUserRepo.SetupSequence(r => r.GetByUsernameAsync("newuser"))
                         .ReturnsAsync((User)null!) //check if user exists
                         .ReturnsAsync(savedUser); 

            _mockUserRepo.Setup(r => r.AddUserAsync(It.IsAny<User>()))
                         .Returns(Task.CompletedTask);

            _mockPortfolioRepo.Setup(p => p.CreatePortfolioAsync(1))
                              .Returns(Task.CompletedTask);

            var token = await _authService.RegisterAsync(input);

            Assert.IsFalse(string.IsNullOrEmpty(token));
        }


        [TestMethod]
        public async Task RegisterAsync_ShouldThrow_WhenUserAlreadyExists()
        {
            var input = new User { Username = "existing", Password = "irrelevant" };
            _mockUserRepo.Setup(r => r.GetByUsernameAsync("existing"))
                         .ReturnsAsync(new User { Username = "existing", Password = "hashed" });

            var ex = await Assert.ThrowsExceptionAsync<Exception>(() =>
                _authService.RegisterAsync(input));

            Assert.AreEqual("User already exists", ex.Message);
        }

        [TestMethod]
        public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreValid()
        {
            var input = new User { Username = "user", Password = "validpass" };
            var stored = new User
            {
                Id = 2,
                Username = "user",
                Password = BCrypt.Net.BCrypt.HashPassword("validpass")
            };

            _mockUserRepo.Setup(r => r.GetByUsernameAsync("user"))
                         .ReturnsAsync(stored);

            var token = await _authService.LoginAsync(input);

            Assert.IsFalse(string.IsNullOrEmpty(token));
        }

        [TestMethod]
        public async Task LoginAsync_ShouldThrow_WhenUsernameNotFound()
        {
            _mockUserRepo.Setup(r => r.GetByUsernameAsync("unknown"))
                         .ReturnsAsync((User)null!);

            var ex = await Assert.ThrowsExceptionAsync<Exception>(() =>
                _authService.LoginAsync(new User { Username = "unknown", Password = "pass" }));

            Assert.AreEqual("Invalid username or password", ex.Message);
        }

        [TestMethod]
        public async Task LoginAsync_ShouldThrow_WhenPasswordIncorrect()
        {
            var stored = new User
            {
                Id = 2,
                Username = "user",
                Password = BCrypt.Net.BCrypt.HashPassword("correctpass")
            };

            _mockUserRepo.Setup(r => r.GetByUsernameAsync("user"))
                         .ReturnsAsync(stored);

            var ex = await Assert.ThrowsExceptionAsync<Exception>(() =>
                _authService.LoginAsync(new User { Username = "user", Password = "wrongpass" }));

            Assert.AreEqual("Invalid username or password", ex.Message);
        }
    }
}
