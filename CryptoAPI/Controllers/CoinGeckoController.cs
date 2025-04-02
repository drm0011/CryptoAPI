using CryptoAPI.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    }
}
