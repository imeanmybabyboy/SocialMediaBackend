using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaBackend.Models.Chat;
using SocialMediaBackend.Models.Rest;
using SocialMediaBackend.Services.AppService;

namespace SocialMediaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController(IAppService appService) : ControllerBase
    {
        [HttpPost("send")]
        public async Task<RestResponse> ApiSendMessageAsync([FromForm] SendMessageFormModel formModel)
        {
            var result = appService.SendMessageAsync(formModel);
            return await result;
        }

        [HttpGet("{targetUserId}/messages")]
        public async Task<RestResponse> ApiGetMessageAsync([FromRoute] string targetUserId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var result = appService.GetChatMessageAsync(targetUserId, page, pageSize);
            return await result;
        }
    }
}
