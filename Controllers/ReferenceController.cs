using Microsoft.AspNetCore.Mvc;
using SocialMediaBackend.Models.Rest;
using SocialMediaBackend.Services.AppService;

namespace SocialMediaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReferenceController(IAppService appService) : ControllerBase
    {
        [HttpGet("races/")]
        public async Task<RestResponse> ApiGetRacesAsync()
        {
            return await appService.GetRacesAsync();
        }
    }
}
