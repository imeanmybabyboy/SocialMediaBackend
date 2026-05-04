using SocialMediaBackend.Models.Post;
using SocialMediaBackend.Models.Rest;
using SocialMediaBackend.Models.User;

namespace SocialMediaBackend.Services.AppService
{
    public interface IAppService
    {
        public Task<RestResponse> GetPostsAsync(int page = 1, int pageSize = 10);
        public Task<RestResponse> GetAdditionalSignUpInfoAsync();
        public Task<RestResponse> SignInAsync(string authHeader);
        public Task<RestResponse> SignUpAsync(UserSignUpFormModel formModel);
        public Task<RestResponse> AddPostAsync (PostAddFormModel formModel);
    }
}
