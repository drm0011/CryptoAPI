using CryptoAPI.Core.DTOs;
using CryptoAPI.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CryptoAPI.Controllers
{
    [ApiController]
    public class PortfolioController : Controller
    {

        private readonly IPortfolioService _portfolioService;

        public PortfolioController(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        [HttpGet("portfolio")]
        public async Task<IActionResult> GetPortfolio()
        {
            //var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var portfolio = await _portfolioService.GetPortfolioAsync(1);
            return Ok(portfolio);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddPortfolioItem([FromBody] PortfolioItemDto portfolioItemDto)
        {
            //var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            await _portfolioService.AddPortfolioItemAsync(2, portfolioItemDto);
            return Ok();
        }

        [HttpDelete("remove/{coinId}")]
        public async Task<IActionResult> RemovePortfolioItem(string coinId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            await _portfolioService.RemovePortfolioItemAsync(userId, coinId);
            return Ok();
        }

    }
}
