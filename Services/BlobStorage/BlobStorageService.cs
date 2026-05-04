using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using SocialMediaBackend.Exceptions;

namespace SocialMediaBackend.Services.BlobStorage
{
    public abstract class BlobStorageService
    {
        protected readonly BlobContainerClient ContainerClient;

        private static readonly HashSet<string> AllowedExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp"];
        private static readonly HashSet<string> AllowedContentTypes = ["image/jpeg", "image/png", "image/gif", "image/webp"];

        protected BlobStorageService(IConfiguration config)
        {
            string connectionString = config["AzureBlobStorage:ConnectionString"]!;
            string containerName = config["AzureBlobStorage:ContainerName"]!;

            ContainerClient = new BlobContainerClient(connectionString, containerName);
            ContainerClient.CreateIfNotExists(PublicAccessType.Blob);
        }

        protected static void ValidateImage(IFormFile file)
        {
            string extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!AllowedExtensions.Contains(extension))
                throw new InvalidFileExtensionException($"The '{extension}' extension is not allowed. List of allowed extensions: {string.Join(", ", AllowedExtensions)}");

            if (!AllowedContentTypes.Contains(file.ContentType))
                throw new InvalidFileTypeException($"The '{file.ContentType}' file type is not allowed.");
        }

        protected async Task<string> UploadAsync(IFormFile file, string blobName)
        {
            BlobClient blobClient = ContainerClient.GetBlobClient(blobName);

            await blobClient.UploadAsync(file.OpenReadStream(), overwrite: true);
            await blobClient.SetHttpHeadersAsync(new BlobHttpHeaders
            {
                ContentType = file.ContentType
            });

            return blobClient.Uri.ToString();
        }

        public async Task DeleteImageAsync(string imageUrl)
        {
            Uri uri = new(imageUrl);
            string blobName = string.Join("", uri.Segments[2..]);

            BlobClient blobClient = ContainerClient.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();
        }
    }
}
