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
            var result = await appService.SignInAsync(Request.Headers.Authorization!);

            return result;
        }

        [HttpPost("signup")]
        public async Task<RestResponse> ApiSignUpAsync([FromForm] UserSignUpFormModel formModel)
        {
            var result = await appService.SignUpAsync(formModel);

            return result;
        }
    }
}
