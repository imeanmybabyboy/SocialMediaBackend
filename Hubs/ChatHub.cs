using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SocialMediaBackend.Services.AppService;

namespace SocialMediaBackend.Hubs
{
    [Authorize]
    public class ChatHub(IAppService appService) : Hub
    {
        public async Task SendPrivateMessage(string targetUserId, string messageText)
        {
            var currentUserId = Context.UserIdentifier;
            if (string.IsNullOrEmpty(currentUserId)) return;

            var response = await appService.SendMessageAsync(targetUserId, messageText);

            if (response.Status.IsOk)
            {
                await Clients.User(targetUserId).SendAsync("ReceiveMessage", response.Data);

                await Clients.Caller.SendAsync("MessageSent", response.Data);
            }
        }
    }
}
