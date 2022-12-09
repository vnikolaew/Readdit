using Readdit.Services.External.Cloudinary.Models;

namespace Readdit.Services.External.Cloudinary;

public interface ICloudinaryService
{
    Task<ImageUploadResult> UploadAsync(Stream? fileStream, string fileName, string contentType);

    Task<bool> DeleteFileAsync(string filePublicId);
}