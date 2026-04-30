namespace SocialMediaBackend.Data.Entities
{
    public class PostInterest
    {
        public Guid PostId { get; set; }
        public Post Post { get; set; } = null!;

        public Guid InterestId { get; set; }
        public Interest Interest { get; set; } = null!;

    }
}
