﻿using CryptoAPI.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAPI.Core.Interfaces
{
    public interface IPortfolioService
    {
        Task<Portfolio> GetPortfolioAsync(int userId);
        Task AddPortfolioItemAsync(int userId, PortfolioItem portfolioItemDto); //change to domain model
        Task RemovePortfolioItemAsync(int userId, string coinId);
        Task AddOrUpdateNoteAsync(int userId, string coinId, string note);
        Task<List<PortfolioNote>> GetNotesByUserAsync(int userId);
    }
}
