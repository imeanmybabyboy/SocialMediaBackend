namespace SocialMediaBackend.Models.Comment
{
    public class Comment
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? PostId { get; set; }
        public string Bio { get; set; } = null!;
        public int LikesQnt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsEdited { get; set; }
        public DateTime? EditedAt { get; set; }

    }
}
