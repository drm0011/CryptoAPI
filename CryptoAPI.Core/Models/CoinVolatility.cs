using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAPI.Core.Models
{
    public class CoinVolatility
    {
        public string CoinId { get; set; } = string.Empty;
        public double VolatilityPercent { get; set; } 
    }
}
