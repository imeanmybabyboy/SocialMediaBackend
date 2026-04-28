namespace SocialMediaBackend.Data.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public Guid? RoleId { get; set; }
        public Guid? RaceId { get; set; }
        public string Login { get; set; } = null!;
        public string Nickname { get; set; } = null!;
        public string? Bio { get; set; }
        public string Email { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public DateTime? LastLoginAt { get; set; }

        public string Salt { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public DateTime RegisteredAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public UserRole Role { get; set; } = null!;
        public List<Post> Posts { get; set; } = [];

        public List<Comment> Comments { get; set; } = [];
        public Race Race { get; set; } = null!;
        public List<UserInterest> UserInterests { get; set; } = [];

    }
}