namespace SocialMediaBackend.Services.BlobStorage
{
    public class PostImageStorageService(IConfiguration config) : BlobStorageService(config), IBlobStorageService
    {
        public async Task<string> UploadImageAsync(IFormFile file, Guid postId)
        {
            ValidateImage(file);
            return await UploadAsync(file, $"posts/{postId}/image");
        }
    }
}
