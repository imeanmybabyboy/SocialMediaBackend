using Microsoft.AspNetCore.Mvc;
using SocialMediaBackend.Models.Rest;
using SocialMediaBackend.Services.AppService;

namespace SocialMediaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController(IAppService appService) : ControllerBase
    {
        [HttpGet("posts/{page?}")]
        public async Task<RestResponse> ApiGetPostsAsync(int page = 1, [FromQuery] int pageSize = 10)
        {
            return await appService.GetPostsAsync(page, pageSize);
        }
    }
}
