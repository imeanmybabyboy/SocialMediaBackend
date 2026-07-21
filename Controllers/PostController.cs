using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("getUserPosts/{userId}")]
        [AllowAnonymous]
        public async Task<RestResponse> ApiUserPostsAsync([FromRoute] string userId, [FromRoute] int page = 1, [FromQuery] int pageSize = 5)
        {
            var result = appService.GetUserPostsAsync(userId, page, pageSize);
            return await result;
        }


        [HttpPost("toggleLike/{postId}")]
        public async Task<RestResponse> ApiTogglePostLikeAsync([FromRoute] string postId)
        {
            var result = appService.TogglePostLikeAsync(postId);
            return await result;
        }

        [HttpPost("toggleSave/{postId}")]
        public async Task<RestResponse> ApiTogglePostSaveAsync([FromRoute] string postId)
        {
            var result = appService.TogglePostSaveAsync(postId);
            return await result;
        }

        [HttpPost("toggleShare/{postId}")]
        public async Task<RestResponse> ApiTogglePostShareAsync([FromRoute] string postId)
        {
            var result = appService.TogglePostShareAsync(postId);
            return await result;
        }

        [HttpGet("{postId}/likes")]
        [AllowAnonymous]
        public async Task<RestResponse> ApiGetUsersWhoLikedPostAsync([FromRoute] string postId)
        {
            var result = appService.GetUsersWhoLikedPostAsync(postId);
            return await result;
        }

        [HttpGet("{postId}/saves")]
        [AllowAnonymous]
        public async Task<RestResponse> ApiGetUsersWhoSavedPostAsync([FromRoute] string postId)
        {
            var result = appService.GetUsersWhoSavedPostAsync(postId);
            return await result;
        }

        [HttpGet("{postId}/shares")]
        [AllowAnonymous]
        public async Task<RestResponse> ApiGetUsersWhoSharedPostAsync([FromRoute] string postId)
        {
            var result = appService.GetUsersWhoSharedPostAsync(postId);
            return await result;
        }

        [HttpPut("edit")]
        public async Task<RestResponse> ApiEditPostAsync([FromForm] PostEditFormModel formModel)
        {
            var result = appService.EditPostAsync(formModel);
            return await result;
        }

        [HttpDelete("{postId}/delete")]
        public async Task<RestResponse> ApiDeletePostAsync([FromRoute] string postId)
        {
            var result = appService.DeletePostAsync(postId);
            return await result;
        }

    }
}
