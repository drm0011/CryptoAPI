using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAPI.Core.Models
{
    public class MarketChartResult
    {
        //coin price history = [timestamp, price]
        public List<List<decimal>> Prices { get; set; } = new();
    }
}
