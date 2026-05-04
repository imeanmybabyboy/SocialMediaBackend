using ASP_PV411.Services.Kdf;
using ASP_PV411.Services.Salt;
using SocialMediaBackend.Data;
using SocialMediaBackend.Data.Entities;
using SocialMediaBackend.Exceptions;
using SocialMediaBackend.Models.Post;
using SocialMediaBackend.Models.Rest;
using SocialMediaBackend.Models.User;
using SocialMediaBackend.Services.AppService;
using SocialMediaBackend.Services.BlobStorage;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SocialMediaBackend.Services.AppService
{
    public class AppService(DataAccessor dataAccessor, IKdfService kdfService, ISaltService saltService, AvatarStorageService avatarStorageService, PostImageStorageService postStorageService) : IAppService
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

        public async Task<RestResponse> GetPostsAsync(int page = 1, int pageSize = 10)
        {
            RestStatus status = RestStatus.Ok;
            List<Models.Post.Post> result = [];

            try
            {
                result = await dataAccessor.GetPostsAsync(page, pageSize);
            }
            catch (Exception ex)
            {
                status = new RestStatus
                {
                    IsOk = false,
                    Code = 500,
                    Phrase = $"Internal Server Error: {ex.Message}"
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
            catch (Exception ex)
            {
                status = new RestStatus
                {
                    IsOk = false,
                    Code = 500,
                    Phrase = $"Internal Server Error: {ex.Message}"
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
            Models.User.UserSignInViewModel? result = null;

            try
            {
                var user = await _AuthenticateAsync(authHeader);

                result = new()
                {
                    Id = user.Id,
                    Role = user.Role.Title,
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
                status = new RestStatus { IsOk = false, Code = 500, Phrase = $"Internal Server Error: {ex.Message}" };
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

        private async Task<User> _AuthenticateAsync(string authHeader)
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

        public async Task<RestResponse> SignUpAsync(UserSignUpFormModel formModel)
        {
            RestStatus status = RestStatus.Ok;
            Models.User.UserSignInViewModel? result = null;

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
                var race = await dataAccessor.GetRaceByNameAsync(formModel.Race);
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
                    throw;
                }

                if (formModel.Interests != null && formModel.Interests.Length != 0)
                {
                    var interests = await dataAccessor.GetInterestByNameAsync(formModel.Interests);

                    var userInterests = interests.Select(interest => new UserInterest
                    {
                        UserId = user.Id,
                        InterestId = interest.Id
                    }).ToList();

                    await dataAccessor.AddUserInterestsAsync(userInterests);
                }

                user = await dataAccessor.GetUserByLoginAsync(user.Login);

                result = new UserSignInViewModel
                {
                    Id = user!.Id,
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
            catch (Exception ex)
            {
                status = new RestStatus { IsOk = false, Code = 400, Phrase = ex.Message };
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

                var user = await dataAccessor.GetUserByIdAsync(formModel.UserId);

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
                    var interests = await dataAccessor.GetInterestByNameAsync(formModel.Interests);

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
                    Comments = post.Comments.Select(c => new Models.Comment.Comment
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
    }
}