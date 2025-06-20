using AutoMapper;
using CryptoAPI.Core.Interfaces;
using CryptoAPI.DAL.Entities;
using CryptoAPI.Exceptions;
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

        public async Task CreatePortfolioAsync(int userId) 
        {
            var exists = await _context.Portfolio.AnyAsync(p => p.UserId == userId);
            if (!exists)
            {
                var newPortfolio = new Portfolio { UserId = userId };
                await _context.Portfolio.AddAsync(newPortfolio);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Core.Models.Portfolio> GetPortfolioByUserIdAsync(int userId) //null checks here instead of service? test exceptions in dal
        {
            var portfolioEntity = await _context.Portfolio
                .Include(p => p.PortfolioItems)
                .FirstOrDefaultAsync(p => p.UserId == userId);

            return _mapper.Map<Core.Models.Portfolio>(portfolioEntity);
        }

        public async Task AddPortfolioItemAsync(int userId, Core.Models.PortfolioItem portfolioItem) 
        {
            var portfolioEntity = await _context.Portfolio
                .Include(p => p.PortfolioItems)
                .FirstOrDefaultAsync(p => p.UserId == userId);

            //if (portfolioEntity == null)
            //{
            //    portfolioEntity = new Portfolio { UserId = userId };
            //    await _context.Portfolio.AddAsync(portfolioEntity);
            //}

            var portfolioItemEntity = _mapper.Map<PortfolioItem>(portfolioItem);
            portfolioEntity.PortfolioItems.Add(portfolioItemEntity); //may be null?
            await _context.SaveChangesAsync();
        }

        public async Task RemovePortfolioItemAsync(int userId, string coinId)
        {
            var portfolioEntity = await _context.Portfolio
                .Include(p => p.PortfolioItems)
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (portfolioEntity == null) throw new NotFoundException("Portfolio not found for user.");

            var portfolioItemEntity = portfolioEntity.PortfolioItems
                .FirstOrDefault(pi => pi.CoinId == coinId);

            if (portfolioItemEntity != null)
            {
                portfolioEntity.PortfolioItems.Remove(portfolioItemEntity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddOrUpdateNoteAsync(int userId, string coinId, string note, string mood)
        {
            var existing = await _context.PortfolioNotes
                .FirstOrDefaultAsync(n => n.UserId == userId && n.CoinId == coinId);

            if (existing != null)
            {
                existing.Note = note;
                existing.Mood = mood;
            }
            else
            {
                await _context.PortfolioNotes.AddAsync(new PortfolioNote
                {
                    UserId = userId,
                    CoinId = coinId,
                    Note = note,
                    Mood = mood
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<Core.Models.PortfolioNote>> GetNotesByUserAsync(int userId)
        {
            var notesEntity = await _context.PortfolioNotes
                .Where(n => n.UserId == userId)
                .ToListAsync();

            return _mapper.Map<List<Core.Models.PortfolioNote>>(notesEntity);
        }

    }
}