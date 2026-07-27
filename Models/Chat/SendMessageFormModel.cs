namespace SocialMediaBackend.Models.Chat
{
    public class SendMessageFormModel
    {
        public string TargetUserId { get; set; } = null!;
        public string Text { get; set; } = null!;
    }
}
