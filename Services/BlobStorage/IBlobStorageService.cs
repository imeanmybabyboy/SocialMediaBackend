namespace SocialMediaBackend.Services.BlobStorage
{
    public interface IBlobStorageService
    {
        Task<string> UploadImageAsync(IFormFile file, Guid entityId);
        Task DeleteImageAsync(string imageUrl);
    }
}
