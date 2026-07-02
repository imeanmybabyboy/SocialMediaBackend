using SocialMediaBackend.Data.Entities;

namespace SocialMediaBackend.Models.Post
{
    public class PostAddFormModel
    {
        public string Title { get; set; } = null!;
        public IFormFile? PostImage { get; set; }
        public string Bio { get; set; } = null!;
        public string[] Interests { get; set; } = [];
        public bool IsPrivate { get; set; } = false;
    }
}
