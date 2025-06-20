using CryptoAPI.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CryptoAPI.Controllers
{
    [ApiController]
    [Authorize]
    public class CoinGeckoController : Controller
    {
        private readonly ICoinGeckoService _coinGeckoService;

        public CoinGeckoController(ICoinGeckoService coinGeckoService)
        {
            _coinGeckoService = coinGeckoService;
        }

        [HttpGet("market")]
        public async Task<IActionResult> GetMarketData([FromQuery] string vsCurrency = "usd", [FromQuery] int perPage = 10, [FromQuery] int page = 1)
        {
            var result = await _coinGeckoService.GetCoinMarketDataAsync(vsCurrency, perPage, page);
            return Ok(result);
        }


        // GET /coininfo?id=bitcoin&vsCurrency=usd
        [HttpGet("coininfo")]
        public async Task<IActionResult> GetCoinInfo([FromQuery] string id, [FromQuery] string vsCurrency)
        {
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(vsCurrency))
                return BadRequest("Both 'id' and 'vsCurrency' query parameters are required.");

            var json = await _coinGeckoService.GetCoinInfoAsync(id, vsCurrency);
            var data = JsonSerializer.Deserialize<JsonElement>(json);

            var marketData = data.GetProperty("market_data");

            // extract the requested currency values
            if (!marketData.GetProperty("current_price").TryGetProperty(vsCurrency, out var priceElem) ||
                !marketData.GetProperty("market_cap").TryGetProperty(vsCurrency, out var marketCapElem))
            {
                return BadRequest($"Currency '{vsCurrency}' not available for coin '{id}'.");
            }

            var result = new
            {
                id = data.GetProperty("id").GetString(),
                name = data.GetProperty("name").GetString(),
                symbol = data.GetProperty("symbol").GetString(),
                image = data.GetProperty("image").GetProperty("small").GetString(),
                price = priceElem.GetDecimal(),
                marketCap = marketCapElem.GetDecimal(),
                change24h = marketData.GetProperty("price_change_percentage_24h").GetDecimal()
            };

            return Ok(result);
        }

        // GET /coincandles?id=bitcoin&vsCurrency=usd&days=7
        [HttpGet("coincandles")]
        public async Task<IActionResult> GetCoinChart(
            [FromQuery] string id,
            [FromQuery] string vsCurrency,
            [FromQuery] int days = 30)
        {
            var result = await _coinGeckoService.GetMarketChartAsync(id, vsCurrency, days);
            return Content(result, "application/json");
        }
    }
}

