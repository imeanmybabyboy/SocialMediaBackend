namespace SocialMediaBackend.Data.Entities
{
    public class Post
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string Title { get; set; } = null!;
        public string? ImageUrl { get; set; } = null!;
        public string Bio { get; set; } = null!;
        public int LikesQnt { get; set; }
        public int SharesQnt { get; set; }

        public User User { get; set; } = null!;

        public List<Comment> Comments { get; set; } = [];
    }
}
