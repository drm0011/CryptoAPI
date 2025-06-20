using CryptoAPI.Core.Interfaces;
using CryptoAPI.Core.Models;
using CryptoAPI.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoAPI.BLL
{
    public class PortfolioService : IPortfolioService
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

            return portfolio;
        }

        public Task AddPortfolioItemAsync(int userId, PortfolioItem portfolioItem)
        {
            if (portfolioItem == null)
                throw new NotFoundException("Item not found");

            return _portfolioRepo.AddPortfolioItemAsync(userId, portfolioItem);
        }

        public async Task RemovePortfolioItemAsync(int userId, string coinId)
        {
            if (string.IsNullOrEmpty(coinId))
                throw new NotFoundException("Coin not found");

            await _portfolioRepo.RemovePortfolioItemAsync(userId, coinId);
            await _portfolioRepo.RemoveNoteAsync(userId, coinId);
        }

        public async Task<List<PortfolioNote>> GetNotesByUserAsync(int userId)
        {
            var notes = await _portfolioRepo.GetNotesByUserAsync(userId);

            if (notes == null || notes.Count == 0)
                throw new NotFoundException("No notes found");

            return notes;
        }

        public Task AddOrUpdateNoteAsync(int userId, string coinId, string note, string mood)
        {
            return _portfolioRepo.AddOrUpdateNoteAsync(userId, coinId, note, mood);
        }

        public async Task<SentimentSummary> GetSentimentSummaryAsync(int userId)
        {
            var notes = await _portfolioRepo.GetNotesByUserAsync(userId);

            return new SentimentSummary
            {
                Bullish = notes.Count(n => n.Mood == "bullish"),
                Neutral = notes.Count(n => n.Mood == "neutral"),
                Bearish = notes.Count(n => n.Mood == "bearish")
            };
        }

        public async Task<List<CoinVolatility>> GetVolatilityForUserAsync(int userId, int days)
        {
            var portfolio = await _portfolioRepo.GetPortfolioByUserIdAsync(userId);

            if (portfolio == null || !portfolio.PortfolioItems.Any())
                throw new NotFoundException("Portfolio is empty or not found");

            var result = new List<CoinVolatility>();

            foreach (var coinId in portfolio.PortfolioItems.Select(p => p.CoinId))
            {
                var prices = await _portfolioRepo.GetCoinHistoricalPricesAsync(coinId, days);

                if (prices.Count < 2) continue;

                var avg = prices.Average();
                var stddev = Math.Sqrt(prices.Average(p => Math.Pow(p - avg, 2)));
                var percent = (stddev / avg) * 100;

                result.Add(new CoinVolatility
                {
                    CoinId = coinId,
                    VolatilityPercent = Math.Round(percent, 2)
                });
            }

            return result;
        }
    }
}
