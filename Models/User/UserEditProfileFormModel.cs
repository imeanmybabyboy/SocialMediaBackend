namespace SocialMediaBackend.Models.User
{
    public class UserEditProfileFormModel
    {
        public string UserId { get; set; } = null!;
        public string? Login { get; set; }
        public string? Nickname { get; set; }
        public string? Bio { get; set; }
        public string? Email { get; set; }
        public IFormFile? Avatar { get; set; }
        public string? OldBase64Password { get; set; }
        public string? Base64Password { get; set; }
        public string[]? Interests { get; set; }
    }
}
