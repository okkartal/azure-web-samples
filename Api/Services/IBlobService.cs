namespace Api.Services;

public interface IBlobService
{
    Task<string> UploadBlobAsync(string blobName, Stream imageStream);
}