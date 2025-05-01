using CryptoAPI.Core.DTOs;
using CryptoAPI.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CryptoAPI.Controllers
{
    [ApiController]
    [Authorize]
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
            var userId = GetCurrentUserId();

            var portfolio = await _portfolioService.GetPortfolioAsync(userId.Value);
            return Ok(portfolio);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddPortfolioItem([FromBody] PortfolioItemDto portfolioItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            

            await _portfolioService.AddPortfolioItemAsync(userId.Value, portfolioItemDto);
            return Ok();
        }

        [HttpDelete("remove/{coinId}")]
        public async Task<IActionResult> RemovePortfolioItem(string coinId)
        {
            var userId = GetCurrentUserId();
            
            await _portfolioService.RemovePortfolioItemAsync(userId.Value, coinId);
            return Ok();
        }

        private int? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return null; //throw exception ?
            }
            return userId;
        }
    }
}
