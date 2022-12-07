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

        byte[] destinationImage;
        await using var memoryStream = new MemoryStream();
        fileStream.Position = 0L;
        
        await fileStream.CopyToAsync(memoryStream);
        destinationImage = memoryStream.ToArray();

        var ms = new MemoryStream(destinationImage);
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(fileName, ms),
            UseFilename = true,
            Overwrite = true,
            PublicId = fileName
        };
        
        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        await ms.DisposeAsync();
        
        return uploadResult.SecureUrl.AbsoluteUri;
    }
}