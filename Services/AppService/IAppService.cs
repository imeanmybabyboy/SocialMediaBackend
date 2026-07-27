using SocialMediaBackend.Models.Chat;
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
        public RestResponse SignOutAsync();
        public Task<RestResponse> SignUpAsync(UserSignUpFormModel formModel);
        public Task<RestResponse> AddPostAsync(PostAddFormModel formModel);
        public Task<RestResponse> EditProfileAsync(UserEditProfileFormModel formModel);
        public Task<RestResponse> DeleteProfileAsync(UserDeleteFormModel formModel);
        public Task<RestResponse> FindUserAsync(string request);
        public Task<RestResponse> AddCommentAsync(CommentAddFormModel formModel);
        public Task<RestResponse> EditCommentAsync(CommentEditFormModel formModel);
        public Task<RestResponse> DeleteCommentAsync(string commentId);
        public Task<RestResponse> GetOwnPostsAsync(int page = 1, int pageSize = 5);
        public Task<RestResponse> GetUserPostsAsync(string userId, int page = 1, int pageSize = 5);
        public Task<RestResponse> TogglePostLikeAsync(string postId);
        public Task<RestResponse> ToggleCommentLikeAsync(string commentId);
        public Task<RestResponse> TogglePostSaveAsync(string postId);
        public Task<RestResponse> TogglePostShareAsync(string postId);
        public Task<RestResponse> GetUserProfileByIdAsync(string userId);
        public Task<RestResponse> GetUserProfileByLoginAsync(string userId);
        public Task<RestResponse> GetUserLikedPostsAsync(int page = 1, int pageSize = 5);
        public Task<RestResponse> GetUserSavedPostsAsync(int page = 1, int pageSize = 5);
        public Task<RestResponse> GetUsersWhoLikedPostAsync(string postId);
        public Task<RestResponse> GetUsersWhoLikedCommentAsync(string commentId);
        public Task<RestResponse> GetUsersWhoSavedPostAsync(string postId);
        public Task<RestResponse> GetUsersWhoSharedPostAsync(string postId);
        public Task<RestResponse> ToggleFollowAsync(string userId);
        public Task<RestResponse> GetFollowersAsync(string userId);
        public Task<RestResponse> GetFollowingAsync(string userId);
        public Task<RestResponse> GetCurrentUserAsync();
        public Task<RestResponse> EditPostAsync(PostEditFormModel formModel);
        public Task<RestResponse> DeletePostAsync(string postId);
        public Task<RestResponse> SendMessageAsync(SendMessageFormModel formModel);
        public Task<RestResponse> GetChatMessageAsync(string targetUserId, int page = 1, int pageSize = 20);
    }
}
