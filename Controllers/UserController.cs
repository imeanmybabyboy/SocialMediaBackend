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
        public async Task<RestResponse> ApiSignInAsync()
        {
            var result = appService.SignInAsync(Request.Headers.Authorization!);
            return await result;
        }

        [HttpPost("signup")]
        public async Task<RestResponse> ApiSignUpAsync([FromForm] UserSignUpFormModel formModel)
        {
            var result = appService.SignUpAsync(formModel);
            return await result;
        }

        [HttpPut("profile/edit")]
        public async Task<RestResponse> ApiEditProfile([FromForm] UserEditProfileFormModel formModel)
        {
            var result = appService.EditProfileAsync(formModel);
            return await result;
        }

        [HttpGet("users/find/{*request}")]
        public async Task<RestResponse> ApiFindUser(string request)
        {
            var result = appService.FindUserAsync(request);
            return await result;
        }

        [HttpGet("profileById/{userId}")]
        public async Task<RestResponse> ApiGetUserProfileById(string userId)
        {
            var result = appService.GetUserProfileByIdAsync(userId);
            return await result;
        }
        
        [HttpGet("profileByLogin/{userLogin}")]
        public async Task<RestResponse> ApiGetUserProfileByLogin(string userLogin)
        {
            var result = appService.GetUserProfileByLoginAsync(userLogin);
            return await result;
        }
    }
}
