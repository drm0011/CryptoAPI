using CryptoAPI.Core.DTOs;
using CryptoAPI.Core.Interfaces;
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

        public async Task<PortfolioDto> GetPortfolioAsync(int userId)
        {
            var portfolio = await _portfolioRepo.GetPortfolioByUserIdAsync(userId);
            return new PortfolioDto
            {
                PortfolioItems = portfolio?.PortfolioItems.Select(pi => new PortfolioItemDto
                {
                    CoinId = pi.CoinId,
                    CoinName = pi.CoinName,
                    Amount = pi.Amount
                }).ToList() ?? new List<PortfolioItemDto>()
            };
        }

        public async Task AddPortfolioItemAsync(int userId, PortfolioItemDto portfolioItemDto)
        {
            if (portfolioItemDto == null)
            {
                return; //throw appropriate exception
            } 

            await _portfolioRepo.AddPortfolioItemAsync(userId, portfolioItemDto);
        }

        public async Task RemovePortfolioItemAsync(int userId, string coinId)
        {
            if (string.IsNullOrEmpty(coinId))
            {
                return; //throw appropriate exception
            }

            await _portfolioRepo.RemovePortfolioItemAsync(userId, coinId);
        }
    }
}
