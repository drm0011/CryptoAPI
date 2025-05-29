using CryptoAPI.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CryptoAPI
{
    [Authorize]
    public class CommentsHub : Hub
    {
        public async Task SendComment(int userId, string coinId, string note)
        {
            var portfolioNote = new PortfolioNote
            {
                UserId = userId,
                CoinId = coinId,
                Note = note
            };

            //broadcast the note to all clients or limit this to groups based on coinId?
            await Clients.All.SendAsync("ReceiveComment", portfolioNote);
        }
    }
}
