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
        public async Task<RestResponse> ApiGetPostsAsync(int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = appService.GetPostsAsync(page, pageSize);

            return await result;
        }

        [HttpPost("post/add")]
        public async Task<RestResponse> ApiAddPostAsync([FromForm] PostAddFormModel formModel)
        {
            var result = appService.AddPostAsync(formModel);

            return await result;
        }
    }
}
