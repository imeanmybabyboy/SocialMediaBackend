namespace SocialMediaBackend.Data.Entities
{
    public class UserRole
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
