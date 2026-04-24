namespace SocialMediaBackend.Data.Entities
{
    public class Interest
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Emoji { get; set; } = null!;
        public string Color { get; set; } = null!;
    }
}
