using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAPI.Core.Models
{
    public class PortfolioItem
    {
        public string CoinId { get; set; }

        public string CoinName { get; set; }

        public decimal Amount { get; set; }
    }
}
