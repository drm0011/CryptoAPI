using CryptoAPI.BLL;
using CryptoAPI.Core.DTOs;
using CryptoAPI.Core.Interfaces;
using CryptoAPI.DAL.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoAPI.TestProject
{
    [TestClass]
    public class PortfolioServiceTests
    {
        private Mock<IPortfolioRepository> _mockPortfolioRepo;
        private PortfolioService _portfolioService;

        [TestInitialize]
        public void Setup()
        {
            _mockPortfolioRepo = new Mock<IPortfolioRepository>();
            _portfolioService = new PortfolioService(_mockPortfolioRepo.Object);
        }

        [TestMethod]
        public async Task GetPortfolioAsync_ReturnsPortfolio_WhenUserHasPortfolio()
        {
            var userId = 1;
            var portfolioDto = new PortfolioDto
            {
                PortfolioItems = new List<PortfolioItemDto>
                {
                    new PortfolioItemDto { CoinId = "bitcoin", CoinName = "Bitcoin", Amount = 0.5m },
                    new PortfolioItemDto { CoinId = "ethereum", CoinName = "Ethereum", Amount = 2m }
                }
            };

            _mockPortfolioRepo.Setup(x => x.GetPortfolioByUserIdAsync(userId))
                .ReturnsAsync(portfolioDto);

            var result = await _portfolioService.GetPortfolioAsync(userId);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.PortfolioItems.Count);
            Assert.AreEqual("bitcoin", result.PortfolioItems.First().CoinId);
            Assert.AreEqual(0.5m, result.PortfolioItems.First().Amount);
            _mockPortfolioRepo.Verify(x => x.GetPortfolioByUserIdAsync(userId), Times.Once);
        }

        [TestMethod]
        public async Task GetPortfolioAsync_ReturnsEmptyPortfolio_WhenUserHasNoPortfolio()
        {
            var userId = 1;
            var emptyPortfolio = new PortfolioDto
            {
                PortfolioItems = new List<PortfolioItemDto>()
            };

            _mockPortfolioRepo.Setup(x => x.GetPortfolioByUserIdAsync(userId))
                .ReturnsAsync(emptyPortfolio);

            var result = await _portfolioService.GetPortfolioAsync(userId);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.PortfolioItems.Count);
            _mockPortfolioRepo.Verify(x => x.GetPortfolioByUserIdAsync(userId), Times.Once);
        }

        [TestMethod]
        public async Task AddPortfolioItemAsync_CallsRepository_WithCorrectParameters()
        {
            var userId = 1;
            var portfolioItemDto = new PortfolioItemDto
            {
                CoinId = "bitcoin",
                CoinName = "Bitcoin",
                Amount = 0.5m
            };

            await _portfolioService.AddPortfolioItemAsync(userId, portfolioItemDto);

            _mockPortfolioRepo.Verify(x => x.AddPortfolioItemAsync(userId, portfolioItemDto), Times.Once);
        }

        [TestMethod]
        public async Task RemovePortfolioItemAsync_CallsRepository_WithCorrectParameters()
        {
            var userId = 1;
            var coinId = "bitcoin";

            await _portfolioService.RemovePortfolioItemAsync(userId, coinId);

            _mockPortfolioRepo.Verify(x => x.RemovePortfolioItemAsync(userId, coinId), Times.Once);
        }

        [TestMethod]
        public async Task AddPortfolioItemAsync_DoesNotThrow_WhenPortfolioItemDtoIsNull()
        {
            // validation happens in controller, service should handle null
            await _portfolioService.AddPortfolioItemAsync(1, null);
            // verify the repo wasnt called
            _mockPortfolioRepo.Verify(x => x.AddPortfolioItemAsync(It.IsAny<int>(), It.IsAny<PortfolioItemDto>()), Times.Never);
        }

        [TestMethod]
        public async Task RemovePortfolioItemAsync_DoesNotThrow_WhenCoinIdIsNullOrEmpty()
        {
            await _portfolioService.RemovePortfolioItemAsync(1, string.Empty);
            _mockPortfolioRepo.Verify(x => x.RemovePortfolioItemAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }
    }
}