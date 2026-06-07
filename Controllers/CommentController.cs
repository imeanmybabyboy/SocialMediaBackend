using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaBackend.Models.Comment;
using SocialMediaBackend.Models.Post;
using SocialMediaBackend.Models.Rest;
using SocialMediaBackend.Services.AppService;

namespace SocialMediaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController(IAppService appService) : ControllerBase
    {
        [HttpPost("add")]
        public async Task<RestResponse> ApiAddCommentAsync([FromForm] CommentAddFormModel formModel)
        {
            var result = appService.AddCommentAsync(formModel);
            return await result;
        }

        [HttpPost("toggleLike/{commentId}")]
        public async Task<RestResponse> ApiToggleCommentLikeAsync([FromRoute] string commentId)
        {
            var result = appService.ToggleCommentLikeAsync(commentId);
            return await result;
        }

        [HttpGet("{commentId}/likes")]
        [AllowAnonymous]
        public async Task<RestResponse> ApiGetUsersWhoLikedCommentAsync([FromRoute] string commentId)
        {
            var result = appService.GetUsersWhoLikedCommentAsync(commentId);
            return await result;
        }
    }
}
