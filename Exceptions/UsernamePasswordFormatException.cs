namespace SocialMediaBackend.Exceptions
{
    public class UsernamePasswordFormatException : Exception
    {
        public UsernamePasswordFormatException(string message) : base(message) { }
    }
}
