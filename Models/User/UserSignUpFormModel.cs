namespace SocialMediaBackend.Models.User
{
    public class UserSignUpFormModel
    {
        public string Login { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Base64Password { get; set; } = null!;
        public string Nickname { get; set; } = null!;
        public IFormFile? Avatar { get; set; }
        public string Race { get; set; } = null!;
        public string[] Interests { get; set; } = [];
    }
}
