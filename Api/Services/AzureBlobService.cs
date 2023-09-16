using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Api.Services;

public class AzureBlobService : IBlobService
{
    private readonly IConfiguration _configuration;
    private readonly string _containerName;

    public AzureBlobService(IConfiguration configuration)
    {
        _configuration = configuration;
        _containerName = _configuration[Constants.KeyBlob];
    }
    
    public async Task<string> UploadBlobAsync(string blobName, Stream imageStream)
    {
        var containerClient = new BlobContainerClient(
            _configuration[Constants.KeyStorageCnn], _containerName);

        var blobClient = containerClient.GetBlobClient(blobName);

        await blobClient.UploadAsync(imageStream,
            new BlobHttpHeaders
            {
                ContentType = "image/jpeg",
                CacheControl = "public"
            });
        return blobClient.Uri.ToString();
    }
}