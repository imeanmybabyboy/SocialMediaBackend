namespace SocialMediaBackend.Models.Chat
{
    public class ChatPreviewViewModel
    {
        public Guid ChatId { get; set; }
        public Guid OtherUserId { get; set; }
        public string OtherUserLogin { get; set; } = null!;
        public string OtherUserNickname { get; set; } = null!;
        public string? OtherUserImageUrl { get; set; }
        public string? LastMessageText { get; set; }
        public Guid? LastMessageSenderId { get; set; }
        public DateTime? LastMessageAt { get; set; }
    }
}
