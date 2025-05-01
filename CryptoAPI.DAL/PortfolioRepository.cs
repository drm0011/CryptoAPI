using AutoMapper;
using CryptoAPI.Core.DTOs;
using CryptoAPI.Core.Interfaces;
using CryptoAPI.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoAPI.DAL
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly CryptoAPIContext _context;
        private readonly IMapper _mapper;

        public PortfolioRepository(CryptoAPIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PortfolioDto> GetPortfolioByUserIdAsync(int userId)
        {
            var portfolioEntity = await _context.Portfolio
                .Include(p => p.PortfolioItems)
                .FirstOrDefaultAsync(p => p.UserId == userId);

            return _mapper.Map<PortfolioDto>(portfolioEntity);
        }

        public async Task AddPortfolioItemAsync(int userId, PortfolioItemDto portfolioItemDto) //remove dto here and create domain model for portfolio
        {
            var portfolioEntity = await _context.Portfolio
                .Include(p => p.PortfolioItems)
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (portfolioEntity == null)
            {
                portfolioEntity = new Portfolio { UserId = userId };
                await _context.Portfolio.AddAsync(portfolioEntity);
            }

            var portfolioItemEntity = _mapper.Map<PortfolioItem>(portfolioItemDto);
            portfolioEntity.PortfolioItems.Add(portfolioItemEntity);
            await _context.SaveChangesAsync();
        }

        public async Task RemovePortfolioItemAsync(int userId, string coinId)
        {
            var portfolioEntity = await _context.Portfolio
                .Include(p => p.PortfolioItems)
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (portfolioEntity == null) return;

            var portfolioItemEntity = portfolioEntity.PortfolioItems
                .FirstOrDefault(pi => pi.CoinId == coinId);

            if (portfolioItemEntity != null)
            {
                portfolioEntity.PortfolioItems.Remove(portfolioItemEntity);
                await _context.SaveChangesAsync();
            }
        }
    }
}