using Microsoft.AspNetCore.Mvc;
using SocialMediaBackend.Models.Rest;
using SocialMediaBackend.Services.AppService;

namespace SocialMediaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReferenceController(IAppService appService) : ControllerBase
    {
        [HttpGet("additionalSignUpInfo/")]
        public async Task<RestResponse> ApiGetAdditionalSignUpInfoAsync()
        {
            return await appService.GetAdditionalSignUpInfoAsync();
        }
    }
}
