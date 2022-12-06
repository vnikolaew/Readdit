using System.Globalization;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Readdit.Services.External.Cloudinary;

public class CloudinaryService : ICloudinaryService
{
    private readonly string[] _validTypes =
    {
        "image/x-png", "image/gif", "image/jpeg", "image/jpg", "image/png", "image/gif", "image/svg",
    };

    private readonly CloudinaryDotNet.Cloudinary _cloudinary;

    public CloudinaryService(CloudinaryDotNet.Cloudinary cloudinary)
    {
        _cloudinary = cloudinary;
    }

    public bool IsFileValid(string contentType)
    {
        return _validTypes.Any(t => t == contentType);
    }

    public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType)
    {
        if (fileStream is null || !IsFileValid(contentType))
        {
            return string.Empty;
        }

        fileName += DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);

        await using var memoryStream = new MemoryStream();
        await fileStream.CopyToAsync(memoryStream);
        
        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(fileName, memoryStream),
            PublicId = fileName
        };
        
        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        return uploadResult.SecureUrl.AbsoluteUri;
    }
}