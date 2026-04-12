namespace SocialMediaBackend.Models.Rest
{
    public class RestResponse
    {
        public RestStatus Status { get; set; } = null!;
        public RestMeta Meta { get; set; } = new();
        public object? Data { get; set; }
    }
}
