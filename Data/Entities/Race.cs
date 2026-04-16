namespace SocialMediaBackend.Data.Entities
{
    public class Race
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string ThemeColorHex { get; set; } = null!;
    }
}
