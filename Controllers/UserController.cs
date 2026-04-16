using Microsoft.AspNetCore.Mvc;
using SocialMediaBackend.Models.Rest;
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
    }
}
