using SocialMediaBackend.Models.Comment;
using SocialMediaBackend.Models.Post;
using SocialMediaBackend.Models.Rest;
using SocialMediaBackend.Models.User;

namespace SocialMediaBackend.Services.AppService
{
    public interface IAppService
    {
        public Task<RestResponse> GetPostsAsync(int page = 1, int pageSize = 5);
        public Task<RestResponse> GetPrivatePostsAsync(int page = 1, int pageSize = 5);
        public Task<RestResponse> GetAdditionalSignUpInfoAsync();
        public Task<RestResponse> SignInAsync(string authHeader);
        public RestResponse SignOut();
        public Task<RestResponse> SignUpAsync(UserSignUpFormModel formModel);
        public Task<RestResponse> AddPostAsync (PostAddFormModel formModel);
        public Task<RestResponse> EditProfileAsync(UserEditProfileFormModel formModel);
        public Task<RestResponse> FindUserAsync(string request);
        public Task<RestResponse> AddCommentAsync(CommentAddFormModel formModel);
        public Task<RestResponse> GetOwnPostsAsync(int page = 1, int pageSize = 5);
        public Task<RestResponse> TogglePostLikeAsync(string postId);
        //public Task<RestResponse> ToggleCommentLikeAsync(LikeFormModel formModel);
        public Task<RestResponse> GetUserProfileByIdAsync(string userId);
        public Task<RestResponse> GetUserProfileByLoginAsync(string userId);

    }
}
