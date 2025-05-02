using CryptoAPI.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAPI.BLL
{
    public class CoinGeckoService:ICoinGeckoService // add exceptions to this class?
    {
        private readonly HttpClient _httpClient;
        public CoinGeckoService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<string> GetCoinMarketDataAsync(string vsCurrency, int perPage, int page)
        {
            var url = $"https://api.coingecko.com/api/v3/coins/markets?vs_currency={vsCurrency}&order=market_cap_desc&per_page={perPage}&page={page}&sparkline=false";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("User-Agent", "CryptoAPI/1.0");

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}: {response.ReasonPhrase}");
            }
        }
    }
}
