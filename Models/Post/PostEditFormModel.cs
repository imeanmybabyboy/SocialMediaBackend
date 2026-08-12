namespace SocialMediaBackend.Models.Post
{
    public class PostEditFormModel
    {
        public string PostId { get; set; } = null!;
        public string? Title { get; set; }
        public IFormFile? PostImage { get; set; }
        public string? Bio { get; set; }
        public string[]? Interests { get; set; }
        public bool? IsPrivate { get; set; }
    }
}
