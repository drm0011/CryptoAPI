//using AutoMapper;
//using CryptoAPI.DAL;
//using CryptoAPI.DAL.Entities;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Linq;
//using System.Threading.Tasks;

//namespace CryptoAPI.TestProject
//{
//    [TestClass]
//    public class PortfolioRepositoryIntegrationTests
//    {
//        private CryptoAPIContext _context;
//        private PortfolioRepository _repository;

//        [TestInitialize]
//        public void Initialize()
//        {
//            // Configure in-memory database
//            var options = new DbContextOptionsBuilder<CryptoAPIContext>()
//                .UseInMemoryDatabase(databaseName: $"CryptoDb_{Guid.NewGuid()}") 
//                .Options;

//            _context = new CryptoAPIContext(options);
//            _repository = new PortfolioRepository(_context, TestHelpers.CreateMapper());
//        }

//        [TestCleanup]
//        public void Cleanup()
//        {
//            _context.Dispose();
//        }

//        [TestMethod]
//        public async Task GetPortfolioByUserIdAsync_ReturnsNull_WhenPortfolioDoesNotExist() //change tests to work with current setup
//        {
//            var result = await _repository.GetPortfolioByUserIdAsync(999);

//            Assert.IsNull(result);
//        }

//        [TestMethod]
//        public async Task AddPortfolioItemAsync_CreatesNewPortfolio_WhenNoneExists()
//        {
//            var itemDto = new PortfolioItemDto
//            {
//                CoinId = "bitcoin",
//                CoinName = "Bitcoin",
//                Amount = 1.5m
//            };

//            await _repository.AddPortfolioItemAsync(1, itemDto);

//            var portfolio = await _context.Portfolio
//                .Include(p => p.PortfolioItems)
//                .FirstOrDefaultAsync(p => p.UserId == 1);

//            Assert.IsNotNull(portfolio);
//            Assert.AreEqual(1, portfolio.PortfolioItems.Count);
//            Assert.AreEqual("bitcoin", portfolio.PortfolioItems.First().CoinId);
//        }

//        [TestMethod]
//        public async Task AddPortfolioItemAsync_AddsToExistingPortfolio()
//        {
//            _context.Portfolio.Add(new Portfolio
//            {
//                UserId = 2,
//                PortfolioItems = new List<PortfolioItem>
//                {
//                    new PortfolioItem { CoinId = "ethereum", CoinName = "Ethereum", Amount = 2 }
//                }
//            });
//            await _context.SaveChangesAsync();

//            var newItem = new PortfolioItemDto
//            {
//                CoinId = "bitcoin",
//                CoinName = "Bitcoin",
//                Amount = 1
//            };

//            await _repository.AddPortfolioItemAsync(2, newItem);

//            var portfolio = await _context.Portfolio
//                .Include(p => p.PortfolioItems)
//                .FirstOrDefaultAsync(p => p.UserId == 2);

//            Assert.AreEqual(2, portfolio.PortfolioItems.Count);
//            Assert.IsTrue(portfolio.PortfolioItems.Any(i => i.CoinId == "bitcoin"));
//        }

//        [TestMethod]
//        public async Task RemovePortfolioItemAsync_RemovesItem_WhenExists()
//        {
//            _context.Portfolio.Add(new Portfolio
//            {
//                UserId = 3,
//                PortfolioItems = new List<PortfolioItem>
//                {
//                    new PortfolioItem { CoinId = "bitcoin", CoinName = "Bitcoin", Amount = 1 },
//                    new PortfolioItem { CoinId = "ethereum", CoinName = "Ethereum", Amount = 2 }
//                }
//            });
//            await _context.SaveChangesAsync();

//            await _repository.RemovePortfolioItemAsync(3, "bitcoin");

//            var portfolio = await _context.Portfolio
//                .Include(p => p.PortfolioItems)
//                .FirstOrDefaultAsync(p => p.UserId == 3);

//            Assert.AreEqual(1, portfolio.PortfolioItems.Count);
//            Assert.IsFalse(portfolio.PortfolioItems.Any(i => i.CoinId == "bitcoin"));
//        }

//        [TestMethod]
//        public async Task RemovePortfolioItemAsync_DoesNothing_WhenItemMissing()
//        {
//            _context.Portfolio.Add(new Portfolio
//            {
//                UserId = 4,
//                PortfolioItems = new List<PortfolioItem>
//                {
//                    new PortfolioItem { CoinId = "ethereum", CoinName = "Ethereum", Amount = 2 }
//                }
//            });
//            await _context.SaveChangesAsync();

//            await _repository.RemovePortfolioItemAsync(4, "bitcoin");

//            var portfolio = await _context.Portfolio
//                .Include(p => p.PortfolioItems)
//                .FirstOrDefaultAsync(p => p.UserId == 4);

//            Assert.AreEqual(1, portfolio.PortfolioItems.Count); 
//        }

//        [TestMethod]
//        public async Task GetPortfolioByUserIdAsync_ReturnsPortfolioWithItems()
//        {
//            _context.Portfolio.Add(new Portfolio
//            {
//                UserId = 5,
//                PortfolioItems = new List<PortfolioItem>
//                {
//                    new PortfolioItem { CoinId = "litecoin", CoinName = "Litecoin", Amount = 5 }
//                }
//            });
//            await _context.SaveChangesAsync();

//            var result = await _repository.GetPortfolioByUserIdAsync(5);

//            Assert.IsNotNull(result);
//            Assert.AreEqual(1, result.PortfolioItems.Count);
//            Assert.AreEqual("litecoin", result.PortfolioItems.First().CoinId);
//        }
//    }

//    public static class TestHelpers
//    {
//        public static IMapper CreateMapper()
//        {
//            var config = new MapperConfiguration(cfg =>
//            {
//                cfg.CreateMap<PortfolioItemDto, PortfolioItem>().ReverseMap();
//                cfg.CreateMap<Portfolio, PortfolioDto>();
//            });
//            return config.CreateMapper();
//        }
//    }
//}