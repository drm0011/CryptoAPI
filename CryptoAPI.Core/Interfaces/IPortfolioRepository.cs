using CryptoAPI.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAPI.Core.Interfaces
{
    public interface IPortfolioRepository
    {
        Task<PortfolioDto> GetPortfolioByUserIdAsync(int userId);
        Task AddPortfolioItemAsync(int userId, PortfolioItemDto portfolioItemDto);
        Task RemovePortfolioItemAsync(int userId, string coinId);
    }
}
