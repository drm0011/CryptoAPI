using CryptoAPI.Core.Models;
using Microsoft.AspNetCore.SignalR;

namespace CryptoAPI
{
    public class CommentsHub:Hub
    {
        public async Task SendComment(int userId, string comment)
        {
            var portfolioComment = new PortfolioComment
            {
                UserId = userId,
                Comment = comment
            };

            //broadcast to all clients 
            await Clients.All.SendAsync("ReceiveComment", portfolioComment);
        }
    }
}
