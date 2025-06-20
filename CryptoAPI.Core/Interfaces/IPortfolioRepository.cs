using CryptoAPI.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAPI.Core.Interfaces
{
    public interface IPortfolioRepository
    {
        Task<Portfolio> GetPortfolioByUserIdAsync(int userId);
        Task AddPortfolioItemAsync(int userId, PortfolioItem portfolioItemDto);
        Task RemovePortfolioItemAsync(int userId, string coinId);
        Task CreatePortfolioAsync(int userId);
        Task AddOrUpdateNoteAsync(int userId, string coinId, string note, string mood);
        Task<List<PortfolioNote>> GetNotesByUserAsync(int userId);
    }
}
