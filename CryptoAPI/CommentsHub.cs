using CryptoAPI.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CryptoAPI
{
    [Authorize]
    public class CommentsHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            if (userId != null)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{userId}");
            }
        }
        public async Task SendComment(string coinId, string note)
        {
            var userId = Context.UserIdentifier;
            if (userId == null) return; //exception throw here?

            await Clients.Group($"user-{userId}")
                .SendAsync("ReceiveComment", new { CoinId = coinId, Note = note });
        }
    }
}
