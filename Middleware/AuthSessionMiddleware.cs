using SocialMediaBackend.Data.Entities;
using SocialMediaBackend.Models.Rest;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace SocialMediaBackend.Middleware
{
    public class SessionUser
    {
        public string Id { get; set; } = null!;
        public string Login { get; set; } = null!;
        public string? Email { get; set; }
        public string? RoleId { get; set; }
    }


    public class AuthSessionMiddleware
    {
        public const string SessionKey = "AuthUser";
        private readonly RequestDelegate _next;

        public AuthSessionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value ?? string.Empty;

            if (context.Session.Keys.Contains(SessionKey))
            {
                var user = JsonSerializer.Deserialize<SessionUser>(
                    context.Session.GetString(SessionKey)!)!;

                context.User = new ClaimsPrincipal(
                    new ClaimsIdentity(
                        [
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Sid, user.Id.ToString()),
                            new Claim(ClaimTypes.Name, user.Login),
                            new Claim(ClaimTypes.Email, user.Email ?? ""),
                            new Claim(ClaimTypes.Role, user.RoleId?.ToString() ?? ""),
                        ],
                        nameof(AuthSessionMiddleware)));
            }

            var endpoint = context.GetEndpoint();
            bool isAllowAnonymous = endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null;

            if (path.StartsWith("/api", StringComparison.OrdinalIgnoreCase)
                && !isAllowAnonymous
                && context.Request.Method != HttpMethods.Options
                && context.User.Identity?.IsAuthenticated != true)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new RestResponse
                {
                    Status = new RestStatus { IsOk = false, Code = 401, Phrase = "Unauthorized. Please sign in" },
                    Meta = new RestMeta
                    {
                        Service = "SocialMediaBackend",
                        Resource = "Authentication",
                        Method = "---",
                        Path = "---",
                        DataType = "application/json (object)",
                        ServerTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                        Cache = 0,
                        Links = new Dictionary<string, string>
                            {
                                { "self", "---" }
                            }
                    },
                    Data = null
                });
                return;
            }

            await _next(context);
        }

        public static void SaveAuth(HttpContext context, User user)
        {
            var sessionUser = new SessionUser
            {
                Id = user.Id.ToString(),
                Login = user.Login,
                Email = user.Email,
                RoleId = user.RoleId?.ToString(),
            };

            context.Session.SetString(SessionKey, JsonSerializer.Serialize(sessionUser));
        }


        public static void Logout(HttpContext context)
        {
            context.Session.Remove(SessionKey);
        }
    }

    public static class AuthSessionMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthSession(this IApplicationBuilder app)
            => app.UseMiddleware<AuthSessionMiddleware>();
    }

}
