using SocialMediaBackend.Models.Rest;

namespace SocialMediaBackend.Services.AppService
{
    public interface IAppService
    {
        public Task<RestResponse> GetPostsAsync(int page = 1, int pageSize = 10);
    }
}
