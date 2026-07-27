namespace SocialMediaBackend.Data.Entities
{
    public class Chat
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid User1Id { get; set; }
        public Guid User2Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual User User1 { get; set; } = null!;
        public virtual User User2 { get; set; } = null!;
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
    }

    public class Message
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ChatId { get; set; }
        public Guid SenderId { get; set; }
        public string Text { get; set; } = string.Empty;
        public string? MediaUrl { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual Chat Chat { get; set; } = null!;
        public virtual User Sender { get; set; } = null!;
    }
}
