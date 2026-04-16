namespace SocialMediaBackend.Exceptions
{
    public class AuthorizationHeaderException : Exception
    {
        public AuthorizationHeaderException(string message) : base(message) { }
    }
}
