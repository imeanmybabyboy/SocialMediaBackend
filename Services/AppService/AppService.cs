using ASP_PV411.Services.Kdf;
using ASP_PV411.Services.Salt;
using SocialMediaBackend.Data;
using SocialMediaBackend.Data.Entities;
using SocialMediaBackend.Exceptions;
using SocialMediaBackend.Middleware;
using SocialMediaBackend.Models.Chat;
using SocialMediaBackend.Models.Comment;
using SocialMediaBackend.Models.Post;
using SocialMediaBackend.Models.Rest;
using SocialMediaBackend.Models.User;
using SocialMediaBackend.Services.BlobStorage;
using System.Text;
using System.Text.RegularExpressions;

namespace SocialMediaBackend.Services.AppService
{
    public class AppService(DataAccessor dataAccessor, IKdfService kdfService, ISaltService saltService, AvatarStorageService avatarStorageService, PostImageStorageService postStorageService, IHttpContextAccessor httpContextAccessor) : IAppService
    {
        private const string MissingAuthorizationHeaderError = "Missing Authorization header";
        private const string InvalidAuthorizationSchemeError = "Invalid Authorization scheme";
        private const string CredentialsError = "Invalid or empty Credentials";
        private const string AuthorizationFormatError = "Invalid Authorization format";
        private const string InvalidUserPasswordFormat = "Invalid user-pass format";
        private const string InvalidCredentialsError = "The login or password you entered is not valid. Please try again";
        private const string UserExistsError = "User with this login already exists";
        private const string InvalidBase64FormatError = "Invalid Base64 password format";
        private const string LoginPasswordError = "Password must be in 'login:password' format";
        private const string EmailExistsError = "The user with such email already exists";
        private const string UserNotFoundError = "User not found";
        private const string InvalidEmailFormatError = "Invalid email format";
        private const string OldPasswordRequiredError = "Old password is required to change password";
        private const string IncorrectPasswordError = "Password is incorrect";
        private const string PostIdEmptyError = "Field PostId cannot be empty";
        private const string CommentIdEmptyError = "Field CommentId cannot be empty";
        private const string BioEmptyError = "Field Bio cannot be empty";
        private const string PostNotFoundError = "Post not found";
        private const string NotPostOwnerError = "You can only edit your own posts";
        private const string NotCommentOwnerError = "You can only edit your own comments";
        private const string AlreadySignedOutError = "Already signed out";
        private const string ErrorWhileSigningUp = "An error occurred while signing up";
        private const string ErrorWhileGettingAdditionalInfo = "An error occurred while getting additional sign up info";
        private const string ErrorWhileRetrievingPosts = "An error occurred while retrieving posts";
        private const string ErrorWhileFindingUser = "An error occurred while finding a user";
        private const string ErrorWhileGettingUserInfo = "An error occurred while getting user info";
        private const string UnauthorizedActionError = "You must be logged in to perform an authorized action";
        private const string CommentNotFoundError = "Comment not found";
        private const string CannotFollowYourselfError = "You cannot follow yourself";
        private const string ChatIdEmptyError = "ChatId cannot be empty";

        /// <summary>
        /// Builds a ready-to-return RestResponse, filling separate for each response fields
        /// </summary>
        private static RestResponse buildResponse(
            RestStatus status,
            object? data,
            string resource,
            string method,
            string path,
            string dataType = "application/json",
            Dictionary<string, string>? links = null)
        {
            return new RestResponse
            {
                Status = status,
                Meta = new RestMeta
                {
                    Service = "SocialMediaBackend",
                    Resource = resource,
                    Method = method,
                    Path = path,
                    DataType = dataType,
                    ServerTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    Cache = 0,
                    Links = links ?? new Dictionary<string, string> { { "self", path } }
                },
                Data = data
            };

        }

        /// <summary>
        /// Builds the self/next/prev links for a paginated endpoint given a function that maps a page number to its path.
        /// </summary>
        private static Dictionary<string, string> buildPaginationLinks(int page, Func<int, string> pathForPage)
        {
            return new Dictionary<string, string>
            {
                { "self", pathForPage(page) },
                { "next", pathForPage(page + 1) },
                { "prev", page > 1 ? pathForPage(page - 1) : "" }
            };
        }

        public async Task<RestResponse> GetPostsAsync(int page = 1, int pageSize = 10)
        {
            RestStatus status = RestStatus.Ok;
            List<Models.Post.Post> result = [];

            try
            {
                string? currentUserId = sessionUserId();
                result = await dataAccessor.GetPostsAsync(currentUserId, page, pageSize);
            }
            catch (Exception)
            {
                status = new RestStatus
                {
                    IsOk = false,
                    Code = 500,
                    Phrase = ErrorWhileRetrievingPosts
                };
            }

            return buildResponse(status, result, "Post", "GET", $"/api/home/posts/{page}?pageSize={pageSize}",
                links: buildPaginationLinks(page, p => $"/api/home/posts/{p}?pageSize={pageSize}"));
        }

        public async Task<RestResponse> GetAdditionalSignUpInfoAsync()
        {
            RestStatus status = RestStatus.Ok;
            UserSignUpViewModel result = new();

            try
            {
                result.Races = await dataAccessor.GetRacesAsync();
                result.Interests = await dataAccessor.GetInterestsAsync();
            }
            catch (Exception)
            {
                status = new RestStatus
                {
                    IsOk = false,
                    Code = 500,
                    Phrase = ErrorWhileGettingAdditionalInfo
                };
            }

            return buildResponse(status, result, "AdditionalSignUpInfo", "GET", "/api/reference/additionalSignUpInfo");
        }

        public async Task<RestResponse> SignInAsync(string authHeader)
        {
            RestStatus status = RestStatus.Ok;
            Models.User.UserProfileViewModel? result = null;

            try
            {
                var user = await authenticateAsync(authHeader);

                AuthSessionMiddleware.SaveAuth(httpContextAccessor.HttpContext!, user);

                result = new()
                {
                    Id = user.Id,
                    Role = user.Role.Title,
                    Race = new Models.Race.Race
                    {
                        Id = user.Race.Id,
                        Name = user.Race.Name,
                        ThemeColorHex = user.Race.ThemeColorHex,
                    },
                    Login = user.Login,
                    Nickname = user.Nickname,
                    Bio = user.Bio,
                    Email = user.Email,
                    ImageUrl = user.ImageUrl,
                    LastLoginAt = user.LastLoginAt,
                    RegisteredAt = user.RegisteredAt,
                    DeletedAt = user.DeletedAt,
                    Interests = user.UserInterests
                        .Select(ui => new Models.Interest.Interest
                        {
                            Id = ui.Interest.Id,
                            Name = ui.Interest.Name,
                            Emoji = ui.Interest.Emoji,
                            Color = ui.Interest.Color
                        }).ToList()
                };
            }
            catch (AuthorizationHeaderException ex)
            {
                status = new RestStatus { IsOk = false, Code = 400, Phrase = ex.Message };
            }
            catch (AuthorizationSchemeException ex)
            {
                status = new RestStatus { IsOk = false, Code = 400, Phrase = ex.Message };
            }
            catch (AuthorizationFormatException ex)
            {
                status = new RestStatus { IsOk = false, Code = 400, Phrase = ex.Message };
            }
            catch (UsernamePasswordFormatException ex)
            {
                status = new RestStatus { IsOk = false, Code = 400, Phrase = ex.Message };
            }
            catch (UserException ex)
            {
                status = new RestStatus { IsOk = false, Code = 401, Phrase = ex.Message };
            }
            catch (Exception ex)
            {
                status = new RestStatus { IsOk = false, Code = 500, Phrase = ex.Message };
            }

            return buildResponse(status, result, "User", "POST", "/api/user/signin");
        }

        public RestResponse SignOutAsync()
        {
            RestStatus status = RestStatus.Ok;

            try
            {
                if (string.IsNullOrWhiteSpace(sessionUserId()))
                {
                    throw new Exception(AlreadySignedOutError);
                }

                AuthSessionMiddleware.Logout(httpContextAccessor.HttpContext!);
            }
            catch (Exception ex)
            {
                status = new RestStatus { IsOk = false, Code = 400, Phrase = ex.Message };
            }

            return buildResponse(status, null, "User", "POST", "/api/user/signout");
        }

        private async Task<User> authenticateAsync(string authHeader)
        {
            if (string.IsNullOrEmpty(authHeader))
            {
                throw new AuthorizationHeaderException(MissingAuthorizationHeaderError);
            }

            string scheme = "Basic ";

            if (!authHeader.StartsWith(scheme))
                throw new AuthorizationSchemeException(InvalidAuthorizationSchemeError);

            string basicCredentials = authHeader[scheme.Length..];

            if (basicCredentials.Length <= 3)
                throw new CredentialsException(CredentialsError);

            string userPass;

            try
            {
                userPass = Encoding.UTF8.GetString(Convert.FromBase64String(basicCredentials));
            }
            catch (Exception)
            {
                throw new AuthorizationFormatException(AuthorizationFormatError);
            }

            string[] parts = userPass.Split(':', 2);

            if (parts.Length != 2)
                throw new UsernamePasswordFormatException(InvalidUserPasswordFormat);

            string login = parts[0].ToLower().Trim();
            string password = parts[1];

            var user = await dataAccessor.GetUserByLoginAsync(login);

            string passwordHash = user != null ? kdfService.Dk(password, user.Salt)
                : kdfService.Dk(password, "dummy-salt-32-chars-long-xxxx");

            if (user == null || passwordHash != user.PasswordHash)
                throw new CredentialsException(InvalidCredentialsError);

            return user;
        }

        private string? sessionUserId() => httpContextAccessor.HttpContext!.User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value;

        public async Task<RestResponse> SignUpAsync(UserSignUpFormModel formModel)
        {
            RestStatus status = RestStatus.Ok;
            UserProfileViewModel? result = null;

            try
            {
                bool userByLoginExists = await dataAccessor.GetUserByLoginAsync(formModel.Login) != null;

                if (userByLoginExists)
                    throw new UserException(UserExistsError);

                bool userByEmailExists = await dataAccessor.GetUserByEmailAsync(formModel.Email) != null;

                if (userByEmailExists)
                    throw new UserException(EmailExistsError);

                string decoded;

                try
                {
                    decoded = Encoding.UTF8.GetString(Convert.FromBase64String(formModel.Base64Password));
                }
                catch
                {
                    throw new InvalidBase64FormatException(InvalidBase64FormatError);
                }

                string[] parts = decoded.Split(':', 2);

                if (parts.Length != 2)
                    throw new LoginPasswordException(LoginPasswordError);

                string userPassword = parts[1];
                string salt = saltService.GetSalt();
                var race = await dataAccessor.GetRaceByIdAsync(formModel.RaceId);
                var role = await dataAccessor.GetUserRoleAsync();

                Guid userId = Guid.NewGuid();
                string? imageUrl = null;

                if (formModel.Avatar != null)
                    imageUrl = await avatarStorageService.UploadImageAsync(formModel.Avatar, userId);


                var user = new Data.Entities.User
                {
                    Id = userId,
                    RaceId = race?.Id,
                    RoleId = role?.Id,
                    Login = formModel.Login,
                    Nickname = formModel.Nickname,
                    Email = formModel.Email,
                    ImageUrl = imageUrl,
                    Salt = salt,
                    PasswordHash = kdfService.Dk(userPassword, salt),
                    RegisteredAt = DateTime.UtcNow,
                };

                try
                {
                    await dataAccessor.AddUserAsync(user);
                }
                catch
                {
                    if (imageUrl != null)
                        await avatarStorageService.DeleteImageAsync(imageUrl);
                }

                if (formModel.Interests != null && formModel.Interests.Length != 0)
                {
                    var interests = await dataAccessor.GetInterestByIdAsync(formModel.Interests);

                    var userInterests = interests.Select(interest => new UserInterest
                    {
                        UserId = user.Id,
                        InterestId = interest.Id
                    }).ToList();

                    await dataAccessor.AddUserInterestsAsync(userInterests);
                }

                user = await dataAccessor.GetUserByLoginAsync(user.Login);

                AuthSessionMiddleware.SaveAuth(httpContextAccessor.HttpContext!, user!);

                result = new UserProfileViewModel
                {
                    Id = user!.Id,
                    Race = new Models.Race.Race
                    {
                        Id = race!.Id,
                        Name = race.Name,
                        ThemeColorHex = race.ThemeColorHex
                    },
                    Role = role!.Title,
                    Login = user.Login,
                    Nickname = user.Nickname,
                    Email = user.Email,
                    Bio = user.Bio,
                    ImageUrl = user.ImageUrl,
                    LastLoginAt = user.LastLoginAt,
                    RegisteredAt = user.RegisteredAt,
                    Interests = user.UserInterests
                        .Select(ui => new Models.Interest.Interest
                        {
                            Id = ui.Interest.Id,
                            Name = ui.Interest.Name,
                            Emoji = ui.Interest.Emoji,
                            Color = ui.Interest.Color
                        }).ToList()
                };
            }
            catch (Exception)
            {
                status = new RestStatus { IsOk = false, Code = 400, Phrase = ErrorWhileSigningUp };
            }

            return buildResponse(status, result, "User", "POST", "/api/user/signup");
        }

        public async Task<RestResponse> AddPostAsync(PostAddFormModel formModel)
        {
            RestStatus status = RestStatus.Ok;
            Models.Post.Post? result = null;

            try
            {
                Guid postId = Guid.NewGuid();
                string? imageUrl = null;

                if (formModel.PostImage != null)
                    imageUrl = await postStorageService.UploadImageAsync(formModel.PostImage, postId);

                var userId = sessionUserId();
                if (string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(UnauthorizedActionError);

                var user = await dataAccessor.GetUserByIdAsync(userId);

                if (user == null)
                    throw new UserException(UserNotFoundError);

                var post = new Data.Entities.Post
                {
                    Id = postId,
                    UserId = user.Id,
                    RaceId = user.RaceId,
                    Title = formModel.Title,
                    ImageUrl = imageUrl,
                    Bio = formModel.Bio,
                    LikesQnt = 0,
                    SharesQnt = 0,
                    CreatedAt = DateTime.UtcNow,
                    IsPrivate = formModel.IsPrivate
                };

                try
                {
                    await dataAccessor.AddPostAsync(post);
                }
                catch
                {
                    if (imageUrl != null)
                        await postStorageService.DeleteImageAsync(imageUrl);
                    throw;
                }

                if (formModel.Interests != null && formModel.Interests.Length != 0)
                {
                    var interests = await dataAccessor.GetInterestByIdAsync(formModel.Interests);

                    var postInterests = interests
                        .Select(i => new PostInterest
                        {
                            PostId = postId,
                            InterestId = i.Id,
                        }).ToList();

                    await dataAccessor.AddPostInterestsAsync(postInterests);
                }

                post = await dataAccessor.GetPostByIdAsync(post.Id.ToString());

                result = new Models.Post.Post
                {
                    Id = post!.Id,
                    UserId = post.UserId,
                    Race = new Models.Race.Race
                    {
                        Id = post.Race!.Id,
                        Name = post.Race.Name
                    },
                    Title = post.Title,
                    ImageUrl = post.ImageUrl,
                    Bio = post.Bio,
                    LikesQnt = post.Likes.Count,
                    SharesQnt = post.SharesQnt,
                    CreatedAt = post.CreatedAt,
                    DeletedAt = post.DeletedAt,
                    IsPrivate = post.IsPrivate,
                    Comments = post.Comments.Select(c => new Models.Comment.CommentViewModel
                    {
                        Id = c.Id,
                        UserId = c.UserId,
                        PostId = c.PostId,
                        Bio = c.Bio,
                        LikesQnt = c.LikesQnt,
                        CreatedAt = c.CreatedAt,
                        DeletedAt = c.DeletedAt,
                        IsEdited = c.IsEdited,
                        EditedAt = c.EditedAt,
                    }).ToList(),
                    Interests = post.PostsInterests
                    .Select(pi => new Models.Interest.Interest
                    {
                        Id = pi.Interest.Id,
                        Name = pi.Interest.Name,
                        Emoji = pi.Interest.Emoji,
                        Color = pi.Interest.Color,
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                status = new RestStatus { IsOk = false, Code = 400, Phrase = ex.Message };
            }

            return buildResponse(status, result, "Post", "POST", "/api/post/add");
        }

        public async Task<RestResponse> EditProfileAsync(UserEditProfileFormModel formModel)
        {
            RestStatus status = RestStatus.Ok;
            UserProfileViewModel? result = null;

            try
            {
                var userId = sessionUserId();
                if (string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(UnauthorizedActionError);

                var user = await dataAccessor.GetUserByIdAsync(userId);

                if (user == null)
                    throw new UserException(UserNotFoundError);

                if (formModel.Email != null)
                {
                    var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

                    if (!emailRegex.IsMatch(formModel.Email))
                        throw new EmailException(InvalidEmailFormatError);

                    bool emailExists = await dataAccessor.GetUserByEmailAsync(formModel.Email) != null;
                    if (emailExists && formModel.Email != user.Email)
                        throw new EmailException(EmailExistsError);
                }

                if (formModel.Login != null)
                {
                    bool loginExists = await dataAccessor.GetUserByLoginAsync(formModel.Login) != null;
                    if (loginExists && formModel.Login != user.Login)
                        throw new UserException(UserExistsError);
                }

                if (formModel.Avatar != null)
                {
                    if (user.ImageUrl != null)
                        await avatarStorageService.DeleteImageAsync(user.ImageUrl);

                    user.ImageUrl = await avatarStorageService.UploadImageAsync(formModel.Avatar, user.Id);
                }

                if (formModel.Base64Password != null)
                {
                    if (formModel.OldBase64Password == null)
                        throw new PasswordException(OldPasswordRequiredError);

                    string decodedOld;
                    try
                    {
                        decodedOld = Encoding.UTF8.GetString(Convert.FromBase64String(formModel.OldBase64Password));
                    }
                    catch
                    {
                        throw new InvalidBase64FormatException(InvalidBase64FormatError);
                    }
                    string[] oldParts = decodedOld.Split(':', 2);
                    if (oldParts.Length != 2)
                        throw new LoginPasswordException(LoginPasswordError);
                    string oldPassword = oldParts[1];
                    string oldPasswordHash = kdfService.Dk(oldPassword, user.Salt);
                    if (oldPasswordHash != user.PasswordHash)
                        throw new PasswordException(IncorrectPasswordError);


                    string decoded;
                    try
                    {
                        decoded = Encoding.UTF8.GetString(Convert.FromBase64String(formModel.Base64Password));
                    }
                    catch
                    {
                        throw new InvalidBase64FormatException(InvalidBase64FormatError);
                    }
                    string[] parts = decoded.Split(':', 2);
                    if (parts.Length != 2)
                        throw new LoginPasswordException(LoginPasswordError);
                    string newPassword = parts[1];
                    string salt = saltService.GetSalt();
                    user.Salt = salt;
                    user.PasswordHash = kdfService.Dk(newPassword, salt);
                }

                user.Login = formModel.Login ?? user.Login;
                user.Nickname = formModel.Nickname ?? user.Nickname;
                user.Bio = formModel.Bio ?? user.Bio;
                user.Email = formModel.Email ?? user.Email;

                if (formModel.Interests != null)
                {
                    await dataAccessor.DeleteUserInterestsAsync(user.Id.ToString());

                    if (formModel.Interests.Length != 0)
                    {
                        var interests = await dataAccessor.GetInterestByIdAsync(formModel.Interests);
                        var userInterests = interests.Select(i => new UserInterest
                        {
                            UserId = user.Id,
                            InterestId = i.Id
                        }).ToList();

                        await dataAccessor.AddUserInterestsAsync(userInterests);
                    }
                }
                await dataAccessor.UpdateUserAsync(user);

                result = await dataAccessor.GetUserProfileByIdAsync(user.Id.ToString(), sessionUserId());
            }
            catch (Exception ex)
            {
                status = new RestStatus { IsOk = false, Code = 400, Phrase = ex.Message };
            }

            return buildResponse(status, result, "User", "PUT", "/api/user/edit");
        }

        public async Task<RestResponse> FindUserAsync(string request)
        {
            RestStatus status = RestStatus.Ok;
            List<UserProfileViewModel> result = [];

            try
            {
                result = await dataAccessor.FindUsersByLoginOrUsername(request, sessionUserId());
            }
            catch (Exception)
            {
                status = new RestStatus { IsOk = false, Code = 500, Phrase = ErrorWhileFindingUser };
            }

            return buildResponse(status, result, "User", "GET", $"/api/users/find?login={request}");
        }

        public async Task<RestResponse> AddCommentAsync(CommentAddFormModel formModel)
        {
            RestStatus status = RestStatus.Ok;
            Models.Comment.CommentViewModel? result = null;

            try
            {
                if (string.IsNullOrWhiteSpace(formModel.PostId))
                    throw new Exception(PostIdEmptyError);

                if (string.IsNullOrWhiteSpace(formModel.Bio))
                    throw new Exception(BioEmptyError);

                var userId = sessionUserId();
                if (string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(UnauthorizedActionError);

                var user = await dataAccessor.GetUserByIdAsync(userId);
                if (user is null)
                    throw new UserException(UserNotFoundError);

                var post = await dataAccessor.GetPostByIdAsync(formModel.PostId);
                if (post is null)
                    throw new PostException(PostNotFoundError);

                var comment = new Data.Entities.Comment
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.Parse(userId),
                    PostId = Guid.Parse(formModel.PostId),
                    Bio = formModel.Bio,
                    LikesQnt = 0,
                    CreatedAt = DateTime.UtcNow,
                    IsEdited = false,
                };

                await dataAccessor.AddCommentAsync(comment);

                result = new Models.Comment.CommentViewModel
                {
                    Id = comment.Id,
                    UserId = comment.UserId,
                    PostId = comment.PostId,
                    Bio = comment.Bio,
                    LikesQnt = comment.LikesQnt,
                    CreatedAt = comment.CreatedAt,
                    DeletedAt = comment.DeletedAt,
                    IsEdited = comment.IsEdited,
                    EditedAt = comment.EditedAt,
                };
            }
            catch (Exception ex)
            {
                status = new RestStatus { IsOk = false, Code = 400, Phrase = ex.Message };
            }

            return buildResponse(status, result, "Comment", "POST", $"/api/comment/add");
        }

        public async Task<RestResponse> GetOwnPostsAsync(int page = 1, int pageSize = 5)
        {
            RestStatus status = RestStatus.Ok;
            List<Models.Post.Post> result = [];
            var userId = sessionUserId();
            if (string.IsNullOrWhiteSpace(userId))
                throw new UnauthorizedAccessException(UnauthorizedActionError);

            try
            {
                var user = await dataAccessor.GetUserByIdAsync(userId) ?? throw new UserException(UserNotFoundError);
                result = await dataAccessor.GetUserPostsAsync(userId, userId, page, pageSize);
            }
            catch (Exception ex)
            {
                status = new RestStatus { IsOk = false, Code = 400, Phrase = ex.Message };
            }

            return buildResponse(status, result, "Post", "GET", $"/api/post/getOwn/{page}?pageSize={pageSize}",
                links: buildPaginationLinks(page, p => $"/api/post/getOwn/{userId}/{p}?pageSize={pageSize}"));
        }

        public async Task<RestResponse> GetUserPostsAsync(string userId, int page = 1, int pageSize = 5)
        {
            RestStatus status = RestStatus.Ok;
            List<Models.Post.Post> result = [];

            try
            {
                var user = await dataAccessor.GetUserByIdAsync(userId) ?? throw new UserException(UserNotFoundError);
                result = await dataAccessor.GetUserPostsAsync(userId, sessionUserId(), page, pageSize);
            }
            catch (UserException ex)
            {
                status = new RestStatus { IsOk = false, Code = 404, Phrase = ex.Message };
            }
            catch (Exception ex)
            {
                status = new RestStatus { IsOk = false, Code = 400, Phrase = ex.Message };
            }

            return buildResponse(status, result, "Post", "GET", $"/api/post/getByUser/{userId}/{page}?pageSize={pageSize}", 
                links: buildPaginationLinks(page, p => $"/api/post/getByUser/{userId}/{p}?pageSize={pageSize}"));
        }

        public async Task<RestResponse> GetPrivatePostsAsync(int page = 1, int pageSize = 5)
        {
            RestStatus status = RestStatus.Ok;
            List<Models.Post.Post> result = [];

            try
            {
                string? currentUserId = sessionUserId();
                result = await dataAccessor.GetPrivatePostsAsync(currentUserId, page, pageSize);
            }
            catch (Exception)
            {
                status = new RestStatus
                {
                    IsOk = false,
                    Code = 500,
                    Phrase = ErrorWhileRetrievingPosts
                };
            }

            return buildResponse(status, result, "Post", "GET", $"/api/home/posts/private/{page}?pageSize={pageSize}",
                links: buildPaginationLinks(page, p => $"/api/home/posts/private/{p}?pageSize={pageSize}"));
        }

        public async Task<RestResponse> TogglePostLikeAsync(string postId)
        {
            RestStatus status = RestStatus.Ok;
            object? result = null;

            try
            {
                if (string.IsNullOrWhiteSpace(postId))
                    throw new Exception(PostIdEmptyError);

                var userId = sessionUserId();
                if (string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(UnauthorizedActionError);

                var user = await dataAccessor.GetUserByIdAsync(userId);
                if (user is null)
                    throw new UserException(UserNotFoundError);

                var post = await dataAccessor.GetPostByIdAsync(postId);
                if (post is null)
                    throw new PostException(PostNotFoundError);

                bool alreadyLiked = await dataAccessor.PostLikeExistsAsync(userId, postId);

                if (alreadyLiked)
                {
                    await dataAccessor.RemovePostLikeAsync(userId, postId);
                    result = new { isLiking = false };
                }
                else
                {
                    await dataAccessor.AddPostLikeAsync(new Data.Entities.PostLike
                    {
                        UserId = Guid.Parse(userId),
                        PostId = Guid.Parse(postId),
                        CreatedAt = DateTime.UtcNow
                    });

                    result = new { isLiking = true };
                }
            }
            catch (Exception ex)
            {
                status = new RestStatus { IsOk = false, Code = 400, Phrase = ex.Message };
            }

            return buildResponse(status, result, "PostLike", "POST", $"/api/post/like");
        }

        public async Task<RestResponse> ToggleCommentLikeAsync(string commentId)
        {
            RestStatus status = RestStatus.Ok;
            object? result = null;

            try
            {
                if (string.IsNullOrWhiteSpace(commentId))
                    throw new Exception(CommentIdEmptyError);

                var userId = sessionUserId();
                if (string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(UnauthorizedActionError);

                var user = await dataAccessor.GetUserByIdAsync(userId);
                if (user is null)
                    throw new UserException(UserNotFoundError);

                var post = await dataAccessor.GetCommentByIdAsync(commentId);
                if (post is null)
                    throw new PostException(CommentNotFoundError);

                bool alreadyLiked = await dataAccessor.CommentLikeExistsAsync(userId, commentId);

                if (alreadyLiked)
                {
                    await dataAccessor.RemoveCommentLikeAsync(userId, commentId);
                    result = new { isLiking = false };
                }
                else
                {
                    await dataAccessor.AddCommentLikeAsync(new Data.Entities.CommentLike
                    {
                        UserId = Guid.Parse(userId),
                        CommentId = Guid.Parse(commentId),
                        CreatedAt = DateTime.UtcNow
                    });
                    result = new { isLiking = false };
                }
            }
            catch (Exception ex)
            {
                status = new RestStatus { IsOk = false, Code = 400, Phrase = ex.Message };
            }

            return buildResponse(status, result, "CommentLike", "POST", $"/api/comment/like");
        }

        public async Task<RestResponse> TogglePostSaveAsync(string postId)
        {
            RestStatus status = RestStatus.Ok;
            object? result = null;

            try
            {
                if (string.IsNullOrWhiteSpace(postId))
                    throw new Exception(PostIdEmptyError);

                var userId = sessionUserId();
                if (string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(UnauthorizedActionError);

                var user = await dataAccessor.GetUserByIdAsync(userId);
                if (user is null)
                    throw new UserException(UserNotFoundError);

                var post = await dataAccessor.GetPostByIdAsync(postId);
                if (post is null)
                    throw new PostException(PostNotFoundError);

                bool alreadysaved = await dataAccessor.PostSaveExistsAsync(userId, postId);

                if (alreadysaved)
                {
                    await dataAccessor.RemovePostSaveAsync(userId, postId);
                    result = new { isSaving = false };

                }
                else
                {
                    await dataAccessor.AddPostSaveAsync(new Data.Entities.PostSave
                    {
                        UserId = Guid.Parse(userId),
                        PostId = Guid.Parse(postId),
                        CreatedAt = DateTime.UtcNow
                    });
                    result = new { isSaving = true };
                }

            }
            catch (Exception ex)
            {
                status = new RestStatus { IsOk = false, Code = 400, Phrase = ex.Message };
            }

            return buildResponse(status, result, "PostSave", "POST", $"/api/post/save");
        }

        public async Task<RestResponse> TogglePostShareAsync(string postId)
        {
            RestStatus status = RestStatus.Ok;
            object? result = null;

            try
            {
                if (string.IsNullOrWhiteSpace(postId))
                    throw new Exception(PostIdEmptyError);

                var userId = sessionUserId();
                if (string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(UnauthorizedActionError);

                var user = await dataAccessor.GetUserByIdAsync(userId);
                if (user is null)
                    throw new UserException(UserNotFoundError);

                var post = await dataAccessor.GetPostByIdAsync(postId);
                if (post is null)
                    throw new PostException(PostNotFoundError);

                bool alreadyShared = await dataAccessor.PostShareExistsAsync(userId, postId);

                if (alreadyShared)
                {
                    await dataAccessor.RemovePostShareAsync(userId, postId);
                    result = new { isSharing = false };
                }
                else
                {
                    await dataAccessor.AddPostShareAsync(new Data.Entities.PostShare
                    {
                        UserId = Guid.Parse(userId),
                        PostId = Guid.Parse(postId),
                        CreatedAt = DateTime.UtcNow
                    });
                    result = new { isSharing = false };
                }
            }
            catch (Exception ex)
            {
                status = new RestStatus { IsOk = false, Code = 400, Phrase = ex.Message };
            }

            return buildResponse(status, result, "PostShare", "POST", $"/api/post/share");
        }

        public async Task<RestResponse> GetUserProfileByIdAsync(string userId)
        {
            RestStatus status = RestStatus.Ok;
            UserProfileViewModel? result = null;

            try
            {
                result = await dataAccessor.GetUserProfileByIdAsync(userId, sessionUserId());
                if (result is null)
                {
                    throw new UserException(UserNotFoundError);
                }
            }
            catch (UserException ex)
            {
                status = new RestStatus
                {
                    IsOk = false,
                    Code = 404,
                    Phrase = ex.Message
                };
            }
            catch (Exception)
            {
                status = new RestStatus
                {
                    IsOk = false,
                    Code = 500,
                    Phrase = ErrorWhileGettingUserInfo
                };
            }

            return buildResponse(status, result, "User", "GET", $"/api/user/profile/{userId}");
        }

        public async Task<RestResponse> GetUserProfileByLoginAsync(string userLogin)
        {
            RestStatus status = RestStatus.Ok;
            UserProfileViewModel? result = null;

            try
            {
                result = await dataAccessor.GetUserProfileByLoginAsync(userLogin, sessionUserId());
                if (result is null)
                {
                    throw new UserException(UserNotFoundError);
                }
            }
            catch (UserException ex)
            {
                status = new RestStatus
                {
                    IsOk = false,
                    Code = 404,
                    Phrase = ex.Message
                };
            }
            catch (Exception)
            {
                status = new RestStatus
                {
                    IsOk = false,
                    Code = 500,
                    Phrase = ErrorWhileGettingUserInfo
                };
            }

            return buildResponse(status, result, "User", "GET", $"/api/user/profile/{userLogin}");
        }

        public async Task<RestResponse> GetUserLikedPostsAsync(int page = 1, int pageSize = 5)
        {
            RestStatus status = RestStatus.Ok;
            List<Models.Post.Post> result = [];

            try
            {
                var userId = sessionUserId();
                if (string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(UnauthorizedActionError);

                result = await dataAccessor.GetUserLikedPostsAsync(userId, page, pageSize);
            }
            catch (Exception)
            {
                status = new RestStatus
                {
                    IsOk = false,
                    Code = 500,
                    Phrase = ErrorWhileRetrievingPosts
                };
            }

            return buildResponse(status, result, "Post", "GET", $"/api/user/likedPosts/{page}?pageSize={pageSize}",
                links: buildPaginationLinks(page, p => $"/api/user/likedPosts/{p}?pageSize={pageSize}"));
        }

        public async Task<RestResponse> GetUserSavedPostsAsync(int page = 1, int pageSize = 5)
        {
            RestStatus status = RestStatus.Ok;
            List<Models.Post.Post> result = [];

            try
            {
                var userId = sessionUserId();
                if (string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(UnauthorizedActionError);

                result = await dataAccessor.GetUserSavedPostsAsync(userId, page, pageSize);
            }
            catch (Exception)
            {
                status = new RestStatus
                {
                    IsOk = false,
                    Code = 500,
                    Phrase = ErrorWhileRetrievingPosts
                };
            }

            return buildResponse(status, result, "Post", "GET", $"/api/user/savedPosts/{page}?pageSize={pageSize}",
                links: buildPaginationLinks(page, p => $"/api/user/savedPosts/{p}?pageSize={pageSize}"));

        }

        public async Task<RestResponse> GetUsersWhoLikedPostAsync(string postId)
        {
            RestStatus status = RestStatus.Ok;
            List<Models.User.UserProfileViewModel> result = [];

            try
            {
                if (string.IsNullOrWhiteSpace(postId))
                    throw new PostException(PostIdEmptyError);

                var post = await dataAccessor.GetPostByIdAsync(postId);
                if (post is null)
                    throw new PostException(PostNotFoundError);

                result = await dataAccessor.GetUsersWhoLikedPostAsync(postId, sessionUserId());
            }
            catch (Exception ex)
            {
                status = new RestStatus { IsOk = false, Code = 500, Phrase = ex.Message };
            }

            return buildResponse(status, result, "User", "GET", $"/api/post/{postId}/likes");
        }

        public async Task<RestResponse> GetUsersWhoLikedCommentAsync(string commentId)
        {
            RestStatus status = RestStatus.Ok;
            List<Models.User.UserProfileViewModel> result = [];

            try
            {
                if (string.IsNullOrWhiteSpace(commentId))
                    throw new CommentException(CommentIdEmptyError);

                var comment = await dataAccessor.GetCommentByIdAsync(commentId);
                if (comment is null)
                    throw new CommentException(CommentNotFoundError);

                result = await dataAccessor.GetUsersWhoLikedCommentAsync(commentId, sessionUserId());
            }
            catch (Exception ex)
            {
                status = new RestStatus { IsOk = false, Code = 500, Phrase = ex.Message };
            }

            return buildResponse(status, result, "User", "GET", $"/api/comment/{commentId}/likes");
        }

        public async Task<RestResponse> GetUsersWhoSavedPostAsync(string postId)
        {
            RestStatus status = RestStatus.Ok;
            List<Models.User.UserProfileViewModel> result = [];

            try
            {
                if (string.IsNullOrWhiteSpace(postId))
                    throw new PostException(PostIdEmptyError);

                var post = await dataAccessor.GetPostByIdAsync(postId);
                if (post is null)
                    throw new PostException(PostNotFoundError);

                result = await dataAccessor.GetUsersWhoSavedPostAsync(postId, sessionUserId());
            }
            catch (Exception ex)
            {
                status = new RestStatus { IsOk = false, Code = 500, Phrase = ex.Message };
            }

            return buildResponse(status, result, "User", "GET", $"/api/post/{postId}/saves");
        }

        public async Task<RestResponse> GetUsersWhoSharedPostAsync(string postId)
        {
            RestStatus status = RestStatus.Ok;
            List<Models.User.UserProfileViewModel> result = [];

            try
            {
                if (string.IsNullOrWhiteSpace(postId))
                    throw new PostException(PostIdEmptyError);

                var post = await dataAccessor.GetPostByIdAsync(postId);
                if (post is null)
                    throw new PostException(PostNotFoundError);

                result = await dataAccessor.GetUsersWhoSharedPostAsync(postId, sessionUserId());
            }
            catch (Exception ex)
            {
                status = new RestStatus { IsOk = false, Code = 500, Phrase = ex.Message };
            }

            return buildResponse(status, result, "User", "GET", $"/api/post/{postId}/shares");
        }

        public async Task<RestResponse> ToggleFollowAsync(string targetUserId)
        {
            RestStatus status = RestStatus.Ok;
            object? result = null;

            try
            {
                var currentUserId = sessionUserId();
                if (string.IsNullOrWhiteSpace(currentUserId))
                    throw new UnauthorizedAccessException(UnauthorizedActionError);

                if (currentUserId == targetUserId)
                    throw new UserException(CannotFollowYourselfError);

                var targetUser = await dataAccessor.GetUserByIdAsync(targetUserId);
                if (targetUser is null)
                    throw new UserException(UserNotFoundError);

                bool alreadyFollowing = await dataAccessor.FollowExistsAsync(currentUserId, targetUserId);
                if (alreadyFollowing)
                {
                    await dataAccessor.RemoveFollowAsync(currentUserId, targetUserId);
                    result = new { isFollowing = false };
                }
                else
                {
                    await dataAccessor.AddFollowAsync(new Data.Entities.UserFollow
                    {
                        FollowerId = Guid.Parse(currentUserId),
                        FollowingId = Guid.Parse(targetUserId),
                        FollowedAt = DateTime.UtcNow
                    });
                    result = new { isFollowing = true };
                }

            }
            catch (Exception ex)
            {
                status = new RestStatus { IsOk = false, Code = 400, Phrase = ex.Message };
            }

            return buildResponse(status, result, "User", "POST", $"/api/user/{targetUserId}/follow");
        }

        public async Task<RestResponse> GetFollowersAsync(string userId)
        {
            RestStatus status = RestStatus.Ok;
            List<Models.User.UserProfileViewModel> result = [];

            try
            {
                var user = await dataAccessor.GetUserByIdAsync(userId);
                if (user is null) throw new UserException(UserNotFoundError);

                result = await dataAccessor.GetFollowersAsync(userId, sessionUserId());

            }
            catch (Exception ex)
            {
                status = new RestStatus { IsOk = false, Code = 500, Phrase = ex.Message };
            }

            return buildResponse(status, result, "User", "GET", $"/api/user/{userId}/followers");
        }

        public async Task<RestResponse> GetFollowingAsync(string userId)
        {
            RestStatus status = RestStatus.Ok;
            List<Models.User.UserProfileViewModel> result = [];

            try
            {
                var user = await dataAccessor.GetUserByIdAsync(userId);
                if (user is null) throw new UserException(UserNotFoundError);

                result = await dataAccessor.GetFollowingAsync(userId, sessionUserId());
            }
            catch (Exception ex)
            {
                status = new RestStatus { IsOk = false, Code = 500, Phrase = ex.Message };
            }

            return buildResponse(status, result, "User", "GET", $"/api/user/{userId}/following");
        }

        public async Task<RestResponse> DeleteProfileAsync(UserDeleteFormModel formModel)
        {
            RestStatus status = RestStatus.Ok;

            try
            {
                var userId = sessionUserId();
                if (string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(UnauthorizedActionError);

                if (string.IsNullOrWhiteSpace(formModel.Base64Password))
                    throw new PasswordException(IncorrectPasswordError);

                string userPass;

                try
                {
                    userPass = Encoding.UTF8.GetString(Convert.FromBase64String(formModel.Base64Password));
                }
                catch (Exception)
                {
                    throw new Exception(InvalidBase64FormatError);
                }

                string[] parts = userPass.Split(':', 2);

                if (parts.Length != 2)
                    throw new UsernamePasswordFormatException(InvalidUserPasswordFormat);

                string login = parts[0].ToLower().Trim();
                string password = parts[1];

                var user = await dataAccessor.GetUserByLoginAsync(login);

                string passwordHash = user != null ? kdfService.Dk(password, user.Salt)
                    : kdfService.Dk(password, "dummy-salt-32-chars-long-xxxx");

                if (user is null || passwordHash != user.PasswordHash)
                    throw new CredentialsException(IncorrectPasswordError);

                if (user.Id.ToString() != userId)
                    throw new CredentialsException(IncorrectPasswordError);

                user.Email = null!;
                user.ImageUrl = null!;
                user.LastLoginAt = null;
                user.Bio = null;
                user.DeletedAt = DateTime.UtcNow;

                await dataAccessor.UpdateUserAsync(user);

                AuthSessionMiddleware.Logout(httpContextAccessor.HttpContext!);
            }
            catch (AuthorizationFormatException ex)
            {
                status = new RestStatus { IsOk = false, Code = 400, Phrase = ex.Message };
            }
            catch (UsernamePasswordFormatException ex)
            {
                status = new RestStatus { IsOk = false, Code = 400, Phrase = ex.Message };
            }
            catch (CredentialsException ex)
            {
                status = new RestStatus { IsOk = false, Code = 401, Phrase = ex.Message };
            }
            catch (UnauthorizedAccessException ex)
            {
                status = new RestStatus { IsOk = false, Code = 401, Phrase = ex.Message };
            }
            catch (Exception ex)
            {
                status = new RestStatus { IsOk = false, Code = 500, Phrase = ex.ToString() };
            }

            return buildResponse(status, null, "User", "DELETE", "/api/user/delete");
        }

        public async Task<RestResponse> GetCurrentUserAsync()
        {
            RestStatus status = RestStatus.Ok;
            UserProfileViewModel? result = null;

            var userId = sessionUserId();
            try
            {
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    result = await dataAccessor.GetUserProfileByIdAsync(userId);
                    if (result is null)
                    {
                        throw new UserException(UserNotFoundError);
                    }
                }
            }
            catch (UserException ex)
            {
                status = new RestStatus
                {
                    IsOk = false,
                    Code = 404,
                    Phrase = ex.Message
                };
            }
            catch (Exception)
            {
                status = new RestStatus
                {
                    IsOk = false,
                    Code = 500,
                    Phrase = ErrorWhileGettingUserInfo
                };
            }

            return buildResponse(status, result, "User", "GET", $"/api/user/getCurrentUser/");
        }

        public async Task<RestResponse> EditPostAsync(PostEditFormModel formModel)
        {
            RestStatus status = RestStatus.Ok;
            Models.Post.Post? result = null;

            try
            {
                if (string.IsNullOrWhiteSpace(formModel.PostId))
                    throw new Exception(PostIdEmptyError);

                var userId = sessionUserId();
                if (string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(UnauthorizedActionError);

                var post = await dataAccessor.GetPostByIdAsync(formModel.PostId);
                if (post is null)
                    throw new PostException(PostNotFoundError);

                if (post.UserId.ToString() != userId)
                    throw new UnauthorizedAccessException(NotPostOwnerError);

                if (formModel.PostImage != null)
                {
                    if (post.ImageUrl != null)
                        await postStorageService.DeleteImageAsync(post.ImageUrl);

                    post.ImageUrl = await postStorageService.UploadImageAsync(formModel.PostImage, post.Id);
                }

                post.Title = formModel.Title ?? post.Title;
                post.Bio = formModel.Bio ?? post.Bio;
                post.IsPrivate = formModel.IsPrivate ?? post.IsPrivate;

                if (formModel.Interests != null)
                {
                    await dataAccessor.DeletePostInterestsAsync(post.Id.ToString());

                    if (formModel.Interests.Length != 0)
                    {
                        var interests = await dataAccessor.GetInterestByIdAsync(formModel.Interests);

                        var postInterests = interests
                            .Select(i => new PostInterest
                            {
                                PostId = post.Id,
                                InterestId = i.Id,
                            }).ToList();

                        await dataAccessor.AddPostInterestsAsync(postInterests);
                    }
                }
                await dataAccessor.UpdatePostAsync(post);
                post = await dataAccessor.GetPostByIdAsync(post.Id.ToString());
                result = new Models.Post.Post
                {
                    Id = post!.Id,
                    UserId = post.UserId,
                    Race = new Models.Race.Race
                    {
                        Id = post.Race!.Id,
                        Name = post.Race.Name
                    },
                    Title = post.Title,
                    ImageUrl = post.ImageUrl,
                    Bio = post.Bio,
                    LikesQnt = post.Likes.Count,
                    SharesQnt = post.SharesQnt,
                    CreatedAt = post.CreatedAt,
                    DeletedAt = post.DeletedAt,
                    IsPrivate = post.IsPrivate,
                    Comments = post.Comments.Select(c => new Models.Comment.CommentViewModel
                    {
                        Id = c.Id,
                        UserId = c.UserId,
                        PostId = c.PostId,
                        Bio = c.Bio,
                        LikesQnt = c.LikesQnt,
                        CreatedAt = c.CreatedAt,
                        DeletedAt = c.DeletedAt,
                        IsEdited = c.IsEdited,
                        EditedAt = c.EditedAt,
                    }).ToList(),
                    Interests = post.PostsInterests
                    .Select(pi => new Models.Interest.Interest
                    {
                        Id = pi.Interest.Id,
                        Name = pi.Interest.Name,
                        Emoji = pi.Interest.Emoji,
                        Color = pi.Interest.Color,
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                status = new RestStatus { IsOk = false, Code = 400, Phrase = ex.Message };
            }

            return buildResponse(status, result, "Post", "PUT", $"/api/post/edit/{formModel.PostId}");
        }

        public async Task<RestResponse> DeletePostAsync(string postId)
        {
            RestStatus status = RestStatus.Ok;

            try
            {
                if (string.IsNullOrWhiteSpace(postId))
                    throw new Exception(PostIdEmptyError);

                var userId = sessionUserId();
                if (string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(UnauthorizedActionError);

                var post = await dataAccessor.GetPostByIdAsync(postId);
                if (post is null)
                    throw new PostException(PostNotFoundError);

                if (post.UserId.ToString() != userId)
                    throw new UnauthorizedAccessException(NotPostOwnerError);

                if (post.ImageUrl != null)
                {
                    await postStorageService.DeleteImageAsync(post.ImageUrl);
                    post.ImageUrl = null;
                }

                post.DeletedAt = DateTime.UtcNow;

                await dataAccessor.UpdatePostAsync(post);
            }
            catch (Exception ex)
            {
                status = new RestStatus { IsOk = false, Code = 400, Phrase = ex.Message };
            }

            return buildResponse(status, null, "Post", "DELETE", $"/api/post/{postId}/delete");
        }

        public async Task<RestResponse> EditCommentAsync(CommentEditFormModel formModel)
        {
            RestStatus status = RestStatus.Ok;
            Models.Comment.CommentViewModel? result = null;

            try
            {
                if (string.IsNullOrWhiteSpace(formModel.CommentId))
                    throw new Exception(CommentIdEmptyError);

                var userId = sessionUserId();
                if (string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(UnauthorizedActionError);

                var comment = await dataAccessor.GetCommentByIdAsync(formModel.CommentId);
                if (comment is null)
                    throw new CommentException(CommentNotFoundError);

                if (comment.UserId.ToString() != userId)
                    throw new UnauthorizedAccessException(NotCommentOwnerError);

                if (string.IsNullOrWhiteSpace(formModel.Bio))
                    throw new Exception(BioEmptyError);

                comment.Bio = formModel.Bio;
                comment.IsEdited = true;
                comment.EditedAt = DateTime.UtcNow;

                await dataAccessor.UpdateCommentAsync(comment);
                comment = await dataAccessor.GetCommentByIdAsync(comment.Id.ToString());

                if (comment is null)
                    throw new Exception(CommentNotFoundError);

                result = new CommentViewModel
                {
                    Id = comment.Id,
                    UserId = comment.UserId,
                    PostId = comment.PostId,
                    Bio = comment.Bio,
                    LikesQnt = comment.LikesQnt,
                    CreatedAt = comment.CreatedAt,
                    DeletedAt = comment.DeletedAt,
                    IsEdited = comment.IsEdited,
                    EditedAt = comment.EditedAt,
                };
            }
            catch (Exception ex)
            {
                status = new RestStatus { IsOk = false, Code = 400, Phrase = ex.Message };
            }

            return buildResponse(status, result, "Comment", "PUT", $"/api/comment/edit/{formModel.CommentId}");
        }

        public async Task<RestResponse> DeleteCommentAsync(string commentId)
        {
            RestStatus status = RestStatus.Ok;

            try
            {
                if (string.IsNullOrWhiteSpace(commentId))
                    throw new Exception(CommentIdEmptyError);

                var userId = sessionUserId();
                if (string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(UnauthorizedActionError);

                var comment = await dataAccessor.GetCommentByIdAsync(commentId);
                if (comment is null)
                    throw new CommentException(CommentNotFoundError);

                if (comment.UserId.ToString() != userId)
                    throw new UnauthorizedAccessException(NotCommentOwnerError);

                comment.DeletedAt = DateTime.UtcNow;

                await dataAccessor.UpdateCommentAsync(comment);
            }
            catch (Exception ex)
            {
                status = new RestStatus { IsOk = false, Code = 400, Phrase = ex.Message };
            }

            return buildResponse(status, null, "Comment", "DELETE", $"/api/comment/{commentId}/delete");
        }

        public async Task<RestResponse> SendMessageAsync(SendMessageFormModel formModel)
        {
            RestStatus status = RestStatus.Ok;
            object? result = null;

            try
            {
                var senderId = sessionUserId();
                if (string.IsNullOrWhiteSpace(senderId))
                    throw new UnauthorizedAccessException("UnauthorizedActionError");

                if (senderId == formModel.TargetUserId)
                    throw new Exception("You cannot send messages to yourself");

                if (string.IsNullOrWhiteSpace(formModel.Text))
                    throw new Exception("Message text cannot be empty");

                var chat = await dataAccessor.GetOrCreateChatAsync(Guid.Parse(senderId), Guid.Parse(formModel.TargetUserId));

                var message = new Message
                {
                    Id = Guid.NewGuid(),
                    ChatId = chat.Id,
                    SenderId = Guid.Parse(senderId),
                    Text = formModel.Text,
                    CreatedAt = DateTime.UtcNow
                };

                await dataAccessor.AddMessageAsync(message);

                result = new
                {
                    MessageId = message.Id,
                    message.ChatId,
                    message.SenderId,
                    message.Text,
                    message.CreatedAt
                };
            }
            catch (Exception ex)
            {
                status = new RestStatus { IsOk = false, Code = 400, Phrase = ex.Message };
            }

            return buildResponse(status, result, "Message", "POST", "/api/chat/send");
        }

        public async Task<RestResponse> GetChatMessageAsync(string targetUserId, int page = 1, int pageSize = 20)
        {
            RestStatus status = RestStatus.Ok;
            object? result = null;

            try
            {
                var currentUserId = sessionUserId();
                if (string.IsNullOrWhiteSpace(currentUserId))
                    throw new UnauthorizedAccessException("UnauthorizedActionError");

                result = await dataAccessor.GetChatMessagesAsync(Guid.Parse(currentUserId), Guid.Parse(targetUserId), page, pageSize);
            }
            catch (Exception ex)
            {
                status = new RestStatus { IsOk = false, Code = 400, Phrase = ex.Message };
            }

            return buildResponse(status, result, "Message", "GET", $"/api/chat/{targetUserId}/messages");
        }

        public async Task<RestResponse> GetUserChatsAsync()
        {
            RestStatus status = RestStatus.Ok;
            object? result = null;

            try
            {
                var currentUserId = sessionUserId();
                if (string.IsNullOrWhiteSpace(currentUserId))
                    throw new UnauthorizedAccessException("UnauthorizedActionError");

                result = await dataAccessor.GetUserChatsAsync(Guid.Parse(currentUserId));
            }
            catch (Exception ex)
            {
                status = new RestStatus { IsOk = false, Code = 400, Phrase = ex.Message };
            }

            return buildResponse(status, result, "Chats", "GET", "/api/chat/list", "application/json (array)");
        }

        public async Task<RestResponse> DeleteChatAsync(string chatId)
        {
            RestStatus status = RestStatus.Ok;

            try
            {
                if (string.IsNullOrWhiteSpace(chatId))
                    throw new Exception(ChatIdEmptyError);

                var userId = sessionUserId();
                if (string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(UnauthorizedActionError);

                await dataAccessor.DeleteChatAsync(Guid.Parse(chatId), Guid.Parse(userId));
            }
            catch (Exception ex)
            {
                status = new RestStatus { IsOk = false, Code = 400, Phrase = ex.Message };
            }

            return buildResponse(status, null, "Chat", "DELETE", $"/api/chat/{chatId}");
        }
    }
}