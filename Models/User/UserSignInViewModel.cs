using SocialMediaBackend.Data.Entities;

namespace SocialMediaBackend.Models.User
{
    public class UserSignInViewModel
    {
        public Guid Id { get; set; }
        public string Role { get; set; } = null!;
        public string Race { get; set; } = null!;
        public string Login { get; set; } = null!;
        public string Nickname { get; set; } = null!;
        public string? Bio { get; set; }
        public string? Email { get; set; }
        public string? ImageUrl { get; set; }
        public List<Interest.Interest> Interests { get; set; } = [];

        public DateTime? LastLoginAt { get; set; }
        public DateTime RegisteredAt { get; set; }
        public DateTime? DeletedAt { get; set; }

    }
}
