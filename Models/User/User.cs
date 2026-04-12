namespace SocialMediaBackend.Models.User
{
    public class User
    {
        public Guid Id { get; set; }
        public Guid? RoleId { get; set; }
        public string Login { get; set; } = null!;
        public string Nickname { get; set; } = null!;
        public string? Bio { get; set; }
        public string? Email { get; set; }
        public string? ImageUrl { get; set; }

        public DateTime? LastLoginAt { get; set; }
        public DateTime RegisteredAt { get; set; }
        public DateTime? DeletedAt { get; set; }

    }
}
