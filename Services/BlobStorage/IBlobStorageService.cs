namespace SocialMediaBackend.Services.BlobStorage
{
    public interface IBlobStorageService
    {
        Task<string> UploadImageAsync(IFormFile file, Guid userId);
        Task DeleteImageAsync(string imageUrl);
    }
}
