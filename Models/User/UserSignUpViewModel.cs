namespace SocialMediaBackend.Models.User
{
    public class UserSignUpViewModel
    {
        public List<Race.Race> Races { get; set; } = [];
        public List<Interest.Interest> Interests { get; set; } = [];
    }
}
