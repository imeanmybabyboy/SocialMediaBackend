using Microsoft.AspNetCore.Mvc;
using SocialMediaBackend.Models.Post;
using SocialMediaBackend.Models.Rest;
using SocialMediaBackend.Services.AppService;

namespace SocialMediaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController(IAppService appService) : ControllerBase
    {
        [HttpGet("posts/{page?}")]
        public async Task<RestResponse> ApiGetPostsAsync([FromRoute] int page = 1, [FromQuery] int pageSize = 5)
        {
            var result = appService.GetPostsAsync(page, pageSize);

            return await result;
        }

        [HttpGet("posts/private/{page?}")]
        public async Task<RestResponse> ApiGetPrivatePostsAsync([FromRoute] int page = 1, [FromQuery] int pageSize = 5)
        {
            var result = appService.GetPrivatePostsAsync(page, pageSize);
            return await result;
        }
    }
}
