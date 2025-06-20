
using CryptoAPI.BLL;
using CryptoAPI.Core.Interfaces;
using CryptoAPI.Core.Models;
using CryptoAPI.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoAPI.TestProject
{
    [TestClass]
    public class PortfolioServiceTests
    {
        private Mock<IPortfolioRepository> _mockRepo;
        private PortfolioService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IPortfolioRepository>();
            _service = new PortfolioService(_mockRepo.Object);
        }

        [TestMethod]
        public async Task GetPortfolioAsync_ReturnsPortfolio_WhenExists()
        {
            var userId = 1;
            var portfolio = new Portfolio
            {
                PortfolioItems = new List<PortfolioItem>
                {
                    new PortfolioItem { CoinId = "btc", CoinName = "Bitcoin" }
                }
            };

            _mockRepo.Setup(r => r.GetPortfolioByUserIdAsync(userId)).ReturnsAsync(portfolio);

            var result = await _service.GetPortfolioAsync(userId);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.PortfolioItems.Count);
            Assert.AreEqual("btc", result.PortfolioItems.First().CoinId);
        }

        [TestMethod]
        public async Task GetPortfolioAsync_Throws_WhenNotFound()
        {
            _mockRepo.Setup(r => r.GetPortfolioByUserIdAsync(It.IsAny<int>())).ReturnsAsync((Portfolio)null);

            await Assert.ThrowsExceptionAsync<NotFoundException>(() => _service.GetPortfolioAsync(1));
        }

        [TestMethod]
        public async Task AddPortfolioItemAsync_CallsRepo_WhenItemIsValid()
        {
            var item = new PortfolioItem { CoinId = "btc", CoinName = "Bitcoin" };

            await _service.AddPortfolioItemAsync(1, item);

            _mockRepo.Verify(r => r.AddPortfolioItemAsync(1, item), Times.Once);
        }

        [TestMethod]
        public async Task AddPortfolioItemAsync_Throws_WhenItemIsNull()
        {
            await Assert.ThrowsExceptionAsync<NotFoundException>(() => _service.AddPortfolioItemAsync(1, null));
        }

        [TestMethod]
        public async Task RemovePortfolioItemAsync_CallsRepo_WhenCoinIdIsValid()
        {
            await _service.RemovePortfolioItemAsync(1, "btc");

            _mockRepo.Verify(r => r.RemovePortfolioItemAsync(1, "btc"), Times.Once);
        }

        [TestMethod]
        public async Task RemovePortfolioItemAsync_Throws_WhenCoinIdIsNull()
        {
            await Assert.ThrowsExceptionAsync<NotFoundException>(() => _service.RemovePortfolioItemAsync(1, null));
        }

        [TestMethod]
        public async Task GetNotesByUserAsync_ReturnsNotes()
        {
            var notes = new List<PortfolioNote>
            {
                new PortfolioNote { CoinId = "btc", Note = "Note 1" }
            };

            _mockRepo.Setup(r => r.GetNotesByUserAsync(1)).ReturnsAsync(notes);

            var result = await _service.GetNotesByUserAsync(1);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Note 1", result.First().Note);
        }

        [TestMethod]
        public async Task AddOrUpdateNoteAsync_CallsRepo()
        {
            await _service.AddOrUpdateNoteAsync(1, "btc", "Note", "Neutral");

            _mockRepo.Verify(r => r.AddOrUpdateNoteAsync(1, "btc", "Note", "Neutral"), Times.Once);
        }
    }
}
