namespace SocialMediaBackend.Services.BlobStorage
{
    public class AvatarStorageService(IConfiguration config) : BlobStorageService(config), IBlobStorageService
    {
        public async Task<string> UploadImageAsync(IFormFile file, Guid userId)
        {
            ValidateImage(file);
            return await UploadAsync(file, $"users/{userId}/avatar");
        }

    }
}
