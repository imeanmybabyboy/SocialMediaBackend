using SocialMediaBackend.Exceptions;
using SocialMediaBackend.Data;
using SocialMediaBackend.Data.Entities;
using SocialMediaBackend.Models.Rest;
using System.Text;
using ASP_PV411.Services.Kdf;

namespace SocialMediaBackend.Services.AppService
{
    public class AppService(DataAccessor dataAccessor, IKdfService kdfService) : IAppService
    {
        private const string UnauthorizedError = "Unauthorized action";
        private const string UserNotFoundError = "User not found";
        private const string NoChangesToSaveError = "No changes to save";
        private const string ChangesSavedWarning = "Changes were saved successfully";
        private const string UnavailableHttpContextError = "HttpContext is not available";
        private const string MissingAuthorizationHeaderError = "Missing Authorization header";
        private const string InvalidAuthorizationSchemeError = "Invalid Authorization scheme";
        private const string CredentialsError = "Invalid or empty Credentials";
        private const string AuthorizationFormatError = "Invalid Authorization format";
        private const string InvalidUserPasswordFormat = "Invalid user-pass format";
        private const string InvalidCredentialsError = "The email or password you entered is not valid. Please try again";
        private const string CategoryExistsError = "Category already exists";
        private const string CategoryAddedWarning = "Category was added";
        private const string SubcategoryExistsError = "Subcategory already exists";
        private const string SubcategoryAddedWarning = "Subcategory was added";



        public async Task<RestResponse> GetPostsAsync(int page = 1, int pageSize = 10)
        {
            RestStatus status = RestStatus.Ok;
            List<Models.Post.Post> result = new();

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

        public async Task<RestResponse> GetRacesAsync()
        {
            RestStatus status = RestStatus.Ok;
            List<Models.Race.Race> result = new();

            try
            {
                result = await dataAccessor.GetRacesAsync();
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
                Resource = "Races",
                Method = "GET",
                Path = "/api/home/races",
                DataType = "application/json (object)",
                ServerTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Cache = 0,
                Links = new Dictionary<string, string>
                {
                    { "self", $"/api/home/races" }
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
                    DeletedAt = user.DeletedAt
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
            catch (CredentialsException ex)
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

            var user = await dataAccessor.GetUserAsyncByLogin(login);

            string passwordHash = user != null ? kdfService.Dk(password, user.Salt)
                : kdfService.Dk(password, "dummy-salt-32-chars-long-xxxx");

            if (user == null || passwordHash != user.PasswordHash)
                throw new CredentialsException(InvalidCredentialsError);

            return user;
        }
    }
}