using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAPI.DAL.Entities
{
    public class PortfolioItem
    {
        public int Id { get; init; }
        public int PortfolioId { get; set; }
        public Portfolio Portfolio { get; set; }
        public string CoinId { get; set; }
        public string CoinName { get; set; }
        [Precision(18,8)]
        public decimal Amount { get; set; }
    }
}
