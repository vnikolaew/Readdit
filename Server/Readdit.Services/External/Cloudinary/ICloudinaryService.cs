namespace Readdit.Services.External.Cloudinary;

public interface ICloudinaryService
{
    bool IsFileValid(string contentType);

    Task<string> UploadAsync(Stream fileStream, string fileName, string contentType);
}