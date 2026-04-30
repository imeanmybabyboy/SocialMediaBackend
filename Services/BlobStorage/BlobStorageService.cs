using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace SocialMediaBackend.Services.BlobStorage
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobContainerClient containerClient;

        public BlobStorageService(IConfiguration config)
        {
            string connectionString = config["AzureBlobStorage:ConnectionString"]!;
            string containerName = config["AzureBlobStorage:ContainerName"]!;

            containerClient = new BlobContainerClient(connectionString, containerName);
            containerClient.CreateIfNotExists(PublicAccessType.Blob);
        }

        public async Task<string> UploadImageAsync(IFormFile file, Guid userId)
        {
            string extension = Path.GetExtension(file.FileName);
            string blobName = $"users/{userId}/avatar{extension}";

            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            var uploadOptions = new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = file.ContentType
                },
            };

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

            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();
        }

    }
}
