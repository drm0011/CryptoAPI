using CryptoAPI.Core.Interfaces;
using CryptoAPI.Core.Models;
using CryptoAPI.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAPI.BLL
{
    public class PortfolioService:IPortfolioService
    {
        private readonly IPortfolioRepository _portfolioRepo;

        public PortfolioService(IPortfolioRepository portfolioRepo)
        {
            _portfolioRepo = portfolioRepo;
        }

        public async Task<Portfolio> GetPortfolioAsync(int userId)
        {
            var portfolio = await _portfolioRepo.GetPortfolioByUserIdAsync(userId);

            if (portfolio == null)
                throw new NotFoundException("Portfolio not found");

            return new Portfolio
            {
                PortfolioItems = portfolio?.PortfolioItems.Select(pi => new PortfolioItem
                {
                    CoinId = pi.CoinId,
                    CoinName = pi.CoinName,
                    Amount = pi.Amount
                }).ToList() ?? new List<PortfolioItem>()
            };
        }

        public async Task AddPortfolioItemAsync(int userId, PortfolioItem portfolioItemDto)
        {
            if (portfolioItemDto == null)
                throw new NotFoundException("Item not found");

            await _portfolioRepo.AddPortfolioItemAsync(userId, portfolioItemDto);
        }

        public async Task RemovePortfolioItemAsync(int userId, string coinId)
        {
            if (string.IsNullOrEmpty(coinId))
                throw new NotFoundException("Coin not found");

            await _portfolioRepo.RemovePortfolioItemAsync(userId, coinId);
        }
    }
}
