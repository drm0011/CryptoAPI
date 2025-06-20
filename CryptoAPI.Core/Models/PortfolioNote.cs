using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAPI.Core.Models
{
    public class PortfolioNote
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CoinId { get; set; }
        public string Note { get; set; }
        public string Mood { get; set; }
    }
}
