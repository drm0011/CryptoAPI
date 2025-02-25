using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAPI.Core.Interfaces
{
    public interface ICoinGeckoService
    {
        Task<string> GetCoinMarketDataAsync(string vsCurrency, int perPage, int page);
    }
}
