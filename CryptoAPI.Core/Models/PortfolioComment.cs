using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAPI.Core.Models
{
    public class PortfolioComment
    {
        public int UserId { get; set; }
        public string Comment { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
