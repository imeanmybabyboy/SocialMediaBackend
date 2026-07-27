using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SocialMediaBackend.Models.Chat;
using SocialMediaBackend.Services.AppService;

namespace SocialMediaBackend.Hubs
{
    [Authorize]
    public class ChatHub(IAppService appService) : Hub
    {
        public async Task SendPrivateMessage(SendMessageFormModel formModel)
        {
            var currentUserId = Context.UserIdentifier;
            if (string.IsNullOrEmpty(currentUserId)) return;

            var response = await appService.SendMessageAsync(formModel);

            if (response.Status.IsOk)
            {
                await Clients.User(formModel.TargetUserId).SendAsync("ReceiveMessage", response.Data);

                await Clients.Caller.SendAsync("MessageSent", response.Data);
            }
        }
    }
}
