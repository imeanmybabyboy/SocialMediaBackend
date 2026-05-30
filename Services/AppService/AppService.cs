using ASP_PV411.Services.Kdf;
using ASP_PV411.Services.Salt;
using Microsoft.AspNetCore.Authorization;
using SocialMediaBackend.Data;
using SocialMediaBackend.Data.Entities;
using SocialMediaBackend.Exceptions;
using SocialMediaBackend.Middleware;
using SocialMediaBackend.Models.Comment;
using SocialMediaBackend.Models.Post;
using SocialMediaBackend.Models.Race;
using SocialMediaBackend.Models.Rest;
using SocialMediaBackend.Models.User;
using SocialMediaBackend.Services.AppService;
using SocialMediaBackend.Services.BlobStorage;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        private const string BioEmptyError = "Field Bio cannot be empty";
        private const string PostNotFoundError = "Post not found";
        private const string AlreadySignedInError = "Already signed in";
        private const string AlreadySignedOutError = "Already signed out";
        private const string ErrorWhileSigningUp = "An error occurred while signing up";
        private const string ErrorWhileGettingAdditionalInfo = "An error occurred while getting additional sign up info";
        private const string ErrorWhileRetrievingPosts = "An error occurred while retrieving posts";
        private const string ErrorWhileFindingUser = "An error occurred while finding a user";
        private const string ErrorWhileGettingUserInfo = "An error occurred while getting user info";
        private const string UnauthorizedActionError = "You must be logged in to perform an authorized action";

        public async Task<RestResponse> GetPostsAsync(int page = 1, int pageSize = 10)
        {
            RestStatus status = RestStatus.Ok;
            List<Models.Post.Post> result = [];

            try
            {
                result = await dataAccessor.GetPostsAsync(page, pageSize);
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

            var meta = new RestMeta
            {
                Service = "SocialMediaBackend",
                Resource = "Posts",
                Method = "GET",
                Path = $"/api/home/posts/{page}?pageSize={pageSize}",
                DataType = "application/json (object)",
                ServerTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Cache = 0,
                Links = new Dictionary<string, string>
                {
                    { "self", $"/api/home/posts/{page}?pageSize={pageSize}" },
                    { "next", $"/api/home/posts/{page + 1}?pageSize={pageSize}" },
                    { "prev", page > 1 ? $"/api/home/posts/{page - 1}?pageSize={pageSize}" : "" }
                }
            };

            return new RestResponse
            {
                Status = status,
                Meta = meta,
                Data = result
            };
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

            var meta = new RestMeta
            {
                Service = "SocialMediaBackend",
                Resource = "AdditionalSignUpInfo",
                Method = "GET",
                Path = "/api/reference/additionalSignUpInfo",
                DataType = "application/json (object)",
                ServerTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Cache = 0,
                Links = new Dictionary<string, string>
                {
                    { "self", $"/api/reference/additionalSignUpInfo" }
                }
            };

            return new RestResponse
            {
                Status = status,
                Meta = meta,
                Data = result
            };
        }

        public async Task<RestResponse> SignInAsync(string authHeader)
        {
            RestStatus status = RestStatus.Ok;
            Models.User.UserProfileViewModel? result = null;

            try
            {
                if (!string.IsNullOrWhiteSpace(sessionUserId()))
                {
                    throw new Exception(AlreadySignedInError);
                }

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

            var meta = new RestMeta
            {
                Service = "SocialMediaBackend",
                Resource = "User",
                Method = "POST",
                Path = "/api/user/signin",
                DataType = "application/json (object)",
                ServerTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Cache = 0,
                Links = new Dictionary<string, string>
                {
                    { "self", "/api/user/signin" }
                }
            };

            return new RestResponse
            {
                Status = status,
                Meta = meta,
                Data = result
            };
        }

        public RestResponse SignOut()
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

            var meta = new RestMeta
            {
                Service = "SocialMediaBackend",
                Resource = "User",
                Method = "POST",
                Path = "/api/user/signout",
                DataType = "application/json (object)",
                ServerTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Cache = 0,
                Links = new Dictionary<string, string> { { "self", "/api/user/signout" } }
            };

            return new RestResponse { Status = status, Meta = meta, Data = null };
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
            Models.User.UserProfileViewModel? result = null;

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

            var meta = new RestMeta
            {
                Service = "SocialMediaBackend",
                Resource = "User",
                Method = "POST",
                Path = "/api/user/signup",
                DataType = "application/json (object)",
                ServerTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Cache = 0,
                Links = new Dictionary<string, string> { { "self", "/api/user/signup" } }
            };

            return new RestResponse
            {
                Status = status,
                Meta = meta,
                Data = result
            };
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
                    CreatedAt = DateTime.UtcNow
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
                    LikesQnt = post.LikesQnt,
                    SharesQnt = post.SharesQnt,
                    CreatedAt = post.CreatedAt,
                    DeletedAt = post.DeletedAt,
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

            var meta = new RestMeta
            {
                Service = "SocialMediaBackend",
                Resource = "Post",
                Method = "POST",
                Path = "/api/post/add",
                DataType = "application/json (object)",
                ServerTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Cache = 0,
                Links = new Dictionary<string, string> { { "self", "/api/post/add" } }
            };

            return new RestResponse
            {
                Status = status,
                Meta = meta,
                Data = result
            };
        }

        public async Task<RestResponse> EditProfileAsync(UserEditProfileFormModel formModel)
        {
            RestStatus status = RestStatus.Ok;
            Models.User.UserProfileViewModel? result = null;

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

                user = await dataAccessor.GetUserByIdAsync(user.Id.ToString());

                result = new UserProfileViewModel
                {
                    Id = user!.Id,
                    Role = user.Role.Title,
                    Race = new Models.Race.Race
                    {
                        Id = user.Race!.Id,
                        Name = user.Race.Name,
                        ThemeColorHex = user.Race.ThemeColorHex
                    },
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
            catch (Exception ex)
            {
                status = new RestStatus { IsOk = false, Code = 400, Phrase = ex.Message };
            }

            var meta = new RestMeta
            {
                Service = "SocialMediaBackend",
                Resource = "User",
                Method = "PUT",
                Path = "/api/user/edit",
                DataType = "application/json (object)",
                ServerTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Cache = 0,
                Links = new Dictionary<string, string> { { "self", "/api/user/edit" } }
            };

            return new RestResponse
            {
                Status = status,
                Meta = meta,
                Data = result
            };
        }

        public async Task<RestResponse> FindUserAsync(string request)
        {
            RestStatus status = RestStatus.Ok;
            List<UserProfileViewModel> result = [];

            try
            {
                var users = await dataAccessor.FindUserByLoginOrUsername(request);

                result = users.Select(user => new UserProfileViewModel
                {
                    Id = user.Id,
                    Race = new Models.Race.Race
                    {
                        Id = user.Race.Id,
                        Name = user.Race.Name,
                        ThemeColorHex = user.Race.ThemeColorHex,
                    },
                    Login = user.Login,
                    Nickname = user.Nickname,
                    Bio = user.Bio,
                    ImageUrl = user.ImageUrl,
                    LastLoginAt = user.LastLoginAt,
                    Interests = user.UserInterests
                        .Select(ui => new Models.Interest.Interest
                        {
                            Id = ui.Interest.Id,
                            Name = ui.Interest.Name,
                            Emoji = ui.Interest.Emoji,
                            Color = ui.Interest.Color
                        }).ToList()
                }).ToList();
            }
            catch (Exception)
            {
                status = new RestStatus { IsOk = false, Code = 500, Phrase = ErrorWhileFindingUser };
            }

            var meta = new RestMeta
            {
                Service = "SocialMediaBackend",
                Resource = "User",
                Method = "GET",
                Path = $"/api/users/find?login={request}",
                DataType = "application/json (object)",
                ServerTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Cache = 0,
                Links = new Dictionary<string, string>
                {
                    { "self", $"/api/users/find?login={request}" }
                }
            };

            return new RestResponse
            {
                Status = status,
                Meta = meta,
                Data = result
            };
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

            var meta = new RestMeta
            {
                Service = "SocialMediaBackend",
                Resource = "Comment",
                Method = "POST",
                Path = "/api/comment/add",
                DataType = "application/json (object)",
                ServerTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Cache = 0,
                Links = new Dictionary<string, string> { { "self", "/api/comment/add" } }
            };

            return new RestResponse
            {
                Status = status,
                Meta = meta,
                Data = result,
            };
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
                result = await dataAccessor.GetUsersPostsAsync(userId, page, pageSize);
            }
            catch (Exception ex)
            {
                status = new RestStatus { IsOk = false, Code = 400, Phrase = ex.Message };
            }

            var meta = new RestMeta
            {
                Service = "SocialMediaBackend",
                Resource = "Post",
                Method = "GET",
                Path = $"/api/post/getOwn/{page}?pageSize={pageSize}",
                DataType = "application/json (object)",
                ServerTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Cache = 0,
                Links = new Dictionary<string, string> {
                    { "self", $"/api/post/getOwn/{userId}/{page}?pageSize={pageSize}" },
                    { "next", $"/api/post/getOwn/{userId}/{page + 1}?pageSize={pageSize}" },
                    { "prev", page > 1 ? $"/api/post/getOwn/{userId}/{page - 1}?pageSize={pageSize}" : "" }

                }
            };

            return new RestResponse
            {
                Status = status,
                Meta = meta,
                Data = result
            };
        }

        public async Task<RestResponse> GetPrivatePostsAsync(int page = 1, int pageSize = 5)
        {
            RestStatus status = RestStatus.Ok;
            List<Models.Post.Post> result = [];

            try
            {
                result = await dataAccessor.GetPrivatePostsAsync(page, pageSize);
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

            var meta = new RestMeta
            {
                Service = "SocialMediaBackend",
                Resource = "Posts",
                Method = "GET",
                Path = $"/api/home/posts/private/{page}?pageSize={pageSize}",
                DataType = "application/json (object)",
                ServerTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Cache = 0,
                Links = new Dictionary<string, string>
                {
                    { "self", $"/api/home/posts/private/{page}?pageSize={pageSize}" },
                    { "next", $"/api/home/posts/private/{page + 1}?pageSize={pageSize}" },
                    { "prev", page > 1 ? $"/api/home/posts/private/{page - 1}?pageSize={pageSize}" : "" }
                }
            };

            return new RestResponse
            {
                Status = status,
                Meta = meta,
                Data = result
            };
        }

        public async Task<RestResponse> TogglePostLikeAsync(string postId)
        {
            RestStatus status = RestStatus.Ok;

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
                }
                else
                {
                    await dataAccessor.AddPostLikeAsync(new Data.Entities.PostLike
                    {
                        UserId = Guid.Parse(userId),
                        PostId = Guid.Parse(postId),
                        CreatedAt = DateTime.UtcNow
                    });
                }

            }
            catch (Exception ex)
            {
                status = new RestStatus { IsOk = false, Code = 400, Phrase = ex.Message };
            }

            var meta = new RestMeta
            {
                Service = "SocialMediaBackend",
                Resource = "PostLike",
                Method = "POST",
                Path = "/api/post/like",
                DataType = "application/json (object)",
                ServerTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Cache = 0,
                Links = new Dictionary<string, string> { { "self", "/api/post/like" } }
            };

            return new RestResponse { Status = status, Meta = meta, Data = null };
        }

        public async Task<RestResponse> GetUserProfileByIdAsync(string userId)
        {
            RestStatus status = RestStatus.Ok;
            Models.User.UserProfileViewModel? result = null;

            try
            {
                var user = await dataAccessor.GetUserByIdAsync(userId);
                if (user is null)
                {
                    throw new UserException(UserNotFoundError);
                }

                result = new()
                {
                    Id = user.Id,
                    Race = new Models.Race.Race
                    {
                        Id = user.Race.Id,
                        Name = user.Race.Name,
                        ThemeColorHex = user.Race.ThemeColorHex,
                    },
                    Login = user.Login,
                    Nickname = user.Nickname,
                    Bio = user.Bio,
                    ImageUrl = user.ImageUrl,
                    LastLoginAt = user.LastLoginAt,
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
            var meta = new RestMeta
            {
                Service = "SocialMediaBackend",
                Resource = "Users",
                Method = "GET",
                Path = $"/api/user/profile/{userId}",
                DataType = "application/json (object)",
                ServerTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Cache = 0,
                Links = new Dictionary<string, string>
                {
                    { "self", $"/api/user/profile/{userId}" },
                }
            };

            return new RestResponse
            {
                Status = status,
                Meta = meta,
                Data = result
            };
        }

        public async Task<RestResponse> GetUserProfileByLoginAsync(string userLogin)
        {
            RestStatus status = RestStatus.Ok;
            Models.User.UserProfileViewModel? result = null;

            try
            {
                var user = await dataAccessor.GetUserByLoginAsync(userLogin);
                if (user is null)
                {
                    throw new UserException(UserNotFoundError);
                }

                result = new()
                {
                    Id = user.Id,
                    Race = new Models.Race.Race
                    {
                        Id = user.Race.Id,
                        Name = user.Race.Name,
                        ThemeColorHex = user.Race.ThemeColorHex,
                    },
                    Login = user.Login,
                    Nickname = user.Nickname,
                    Bio = user.Bio,
                    ImageUrl = user.ImageUrl,
                    LastLoginAt = user.LastLoginAt,
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
            var meta = new RestMeta
            {
                Service = "SocialMediaBackend",
                Resource = "Users",
                Method = "GET",
                Path = $"/api/user/profile/{userLogin}",
                DataType = "application/json (object)",
                ServerTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Cache = 0,
                Links = new Dictionary<string, string>
                {
                    { "self", $"/api/user/profile/{userLogin}" },
                }
            };

            return new RestResponse
            {
                Status = status,
                Meta = meta,
                Data = result
            };
        }

        //TODO: Доробити CommentLike та відображення 
    }
}