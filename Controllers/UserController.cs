using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMediaBackend.Models.Rest;
using SocialMediaBackend.Models.User;
using SocialMediaBackend.Services.AppService;

namespace SocialMediaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IAppService appService) : ControllerBase
    {
        [HttpPost("signin")]
        [AllowAnonymous]
        public async Task<RestResponse> ApiSignInAsync()
        {
            var result = appService.SignInAsync(Request.Headers.Authorization!);
            return await result;
        }

        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<RestResponse> ApiSignUpAsync([FromForm] UserSignUpFormModel formModel)
        {
            var result = appService.SignUpAsync(formModel);
            return await result;
        }

        [HttpPost("signout")]
        public RestResponse ApiSignOut()
        {
            return appService.SignOut();
        }

        [HttpPut("profile/edit")]
        public async Task<RestResponse> ApiEditProfileAsync([FromForm] UserEditProfileFormModel formModel)
        {
            var result = appService.EditProfileAsync(formModel);
            return await result;
        }

        [HttpGet("users/find/{*request}")]
        [AllowAnonymous]
        public async Task<RestResponse> ApiFindUserAsync(string request)
        {
            var result = appService.FindUserAsync(request);
            return await result;
        }

        [HttpGet("profileById/{userId}")]
        [AllowAnonymous]
        public async Task<RestResponse> ApiGetUserProfileByIdAsync(string userId)
        {
            var result = appService.GetUserProfileByIdAsync(userId);
            return await result;
        }
        
        [HttpGet("profileByLogin/{userLogin}")]
        [AllowAnonymous]
        public async Task<RestResponse> ApiGetUserProfileByLoginAsync(string userLogin)
        {
            var result = appService.GetUserProfileByLoginAsync(userLogin);
            return await result;
        }

        [HttpGet("likedPosts/{page?}")]
        public async Task<RestResponse> ApiGetLikedPostsAsync([FromRoute] int page = 1, [FromQuery] int pageSize = 5)
        {
            var result = appService.GetUserLikedPostsAsync(page, pageSize);
            return await result;
        }
        
        [HttpGet("savedPosts/{page?}")]
        public async Task<RestResponse> ApiGetSavedPostsAsync([FromRoute] int page = 1, [FromQuery] int pageSize = 5)
        {
            var result = appService.GetUserSavedPostsAsync(page, pageSize);
            return await result;
        }
    }
}
