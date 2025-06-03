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


        // GET /coininfo?id=bitcoin
        [HttpGet("coininfo")]
        public async Task<IActionResult> GetCoinInfo([FromQuery] string id)
        {
            var json = await _coinGeckoService.GetCoinInfoAsync(id);
            var data = JsonSerializer.Deserialize<JsonElement>(json);

            var result = new
            {
                id = data.GetProperty("id").GetString(),
                name = data.GetProperty("name").GetString(),
                symbol = data.GetProperty("symbol").GetString(),
                image = data.GetProperty("image").GetProperty("small").GetString(),
                price = data.GetProperty("market_data").GetProperty("current_price").GetProperty("usd").GetDecimal(),
                marketCap = data.GetProperty("market_data").GetProperty("market_cap").GetProperty("usd").GetDecimal(),
                change24h = data.GetProperty("market_data").GetProperty("price_change_percentage_24h").GetDecimal()
            };

            return Ok(result);
        }

        // GET /coincandles?id=bitcoin&vsCurrency=usd&days=7
        [HttpGet("coincandles")]
        public async Task<IActionResult> GetCoinChart(
            [FromQuery] string id,
            [FromQuery] string vsCurrency = "usd",
            [FromQuery] int days = 7)
        {
            var result = await _coinGeckoService.GetMarketChartAsync(id, vsCurrency, days);
            return Content(result, "application/json");
        }
    }
}

