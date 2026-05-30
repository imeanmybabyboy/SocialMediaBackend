using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaBackend.Models.Post;
using SocialMediaBackend.Models.Rest;
using SocialMediaBackend.Services.AppService;

namespace SocialMediaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController(IAppService appService) : ControllerBase
    {
        [HttpPost("add")]
        public async Task<RestResponse> ApiAddPostAsync([FromForm] PostAddFormModel formModel)
        {
            var result = appService.AddPostAsync(formModel);
            return await result;
        }

        [HttpGet("getOwn/{page?}")]
        public async Task<RestResponse> ApiGetOwnPostsAsync([FromRoute] int page = 1, [FromQuery] int pageSize = 5)
        {
            var result = appService.GetOwnPostsAsync(page, pageSize);
            return await result;
        }
    }
}
