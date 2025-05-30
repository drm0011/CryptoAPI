using CryptoAPI.Core.Interfaces;
using CryptoAPI.Core.Models;
using Microsoft.Extensions.Options;
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
        private readonly string _apiKey;
        public CoinGeckoService(HttpClient httpClient, IOptions<CoinGeckoOptions> options)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiKey = options.Value.ApiKey;
        }

        public async Task<string> GetCoinMarketDataAsync(string vsCurrency, int perPage, int page)
        {
            var url = $"https://api.coingecko.com/api/v3/coins/markets?vs_currency={vsCurrency}&order=market_cap_desc&per_page={perPage}&page={page}&sparkline=false";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("User-Agent", "CryptoAPI/1.0");
            request.Headers.Add("x-cg-demo-api-key", _apiKey);

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

        public async Task<string> GetCoinInfoAsync(string id)
        {
            var url = $"https://api.coingecko.com/api/v3/coins/{id}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("User-Agent", "CryptoAPI/1.0");
            request.Headers.Add("x-cg-demo-api-key", _apiKey);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            throw new HttpRequestException($"Failed to fetch coin info: {response.StatusCode} {response.ReasonPhrase}");
        }

        public async Task<string> GetMarketChartAsync(string id, string vsCurrency, int days)
        {
            var url = $"https://api.coingecko.com/api/v3/coins/{id}/market_chart?vs_currency={vsCurrency}&days={days}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("User-Agent", "CryptoAPI/1.0");
            request.Headers.Add("x-cg-demo-api-key", _apiKey);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            throw new HttpRequestException($"Failed to fetch market chart: {response.StatusCode} {response.ReasonPhrase}");
        }

    }
}
