using SocialMediaBackend.Data;
using SocialMediaBackend.Models.Rest;

namespace SocialMediaBackend.Services.AppService
{
    public class AppService(DataAccessor dataAccessor) : IAppService
    {
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
                Resouce = "Posts",
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
    }
}
