using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAPI.DAL.Entities
{
    public class PortfolioNote
    {
        public int Id { get; init; }
        public int UserId { get; set; }
        public string CoinId { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public string Mood { get; set; }
    }

}
