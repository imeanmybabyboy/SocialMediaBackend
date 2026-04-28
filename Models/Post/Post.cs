namespace SocialMediaBackend.Models.Post
{
    public class Post
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public Race.Race Race { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? ImageUrl { get; set; } = null!;
        public string Bio { get; set; } = null!;
        public int LikesQnt { get; set; }
        public int SharesQnt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public List<Comment.Comment> Comments { get; set; } = [];
    }
}
