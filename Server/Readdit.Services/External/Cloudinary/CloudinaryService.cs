using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ImageUploadResult = Readdit.Services.External.Cloudinary.Models.ImageUploadResult;

namespace Readdit.Services.External.Cloudinary;

public class CloudinaryService : ICloudinaryService
{
    private readonly string[] _validTypes =
    {
        "image/x-png", "image/gif", "image/jpeg", 
        "image/jpg", "image/png", "image/gif",
        "image/svg", "image/webp"
    };

    private static string Timestamp
        => DateTime.UtcNow.ToShortDateString();

    private readonly CloudinaryDotNet.Cloudinary _cloudinary;

    public CloudinaryService(CloudinaryDotNet.Cloudinary cloudinary)
        => _cloudinary = cloudinary;

    private bool IsFileValid(string contentType)
        => _validTypes.Any(t => t == contentType);

    public async Task<ImageUploadResult> UploadAsync(Stream? fileStream, string fileName, string contentType)
    {
        if (fileStream is null || !IsFileValid(contentType))
        {
            return null!;
        }

        var newFileName = AppendTimestampAndGuid(fileName);

        await using var memoryStream = new MemoryStream();
        fileStream.Position = 0L;
        
        await fileStream.CopyToAsync(memoryStream);
        var destinationImage = memoryStream.ToArray();

        var ms = new MemoryStream(destinationImage);
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(newFileName, ms),
            Format = GetFileExtension(fileName),
            UseFilename = true,
            Overwrite = true,
            PublicId = newFileName
        };
        
        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        await ms.DisposeAsync();
        
        return new ImageUploadResult
        {
            AbsoluteImageUrl = uploadResult.SecureUrl?.AbsoluteUri ?? string.Empty,
            ImagePublidId = uploadResult.PublicId ?? string.Empty
        };
    }

    public async Task<bool> DeleteFileAsync(string filePublicId)
    {
        try
        {
            await _cloudinary.DestroyAsync(new DeletionParams(filePublicId));
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private static string AppendTimestampAndGuid(string fileName)
    {
        var dotIndex = fileName.LastIndexOf('.');
        return $"{fileName[..dotIndex]}_{Timestamp}_{Guid.NewGuid().ToString()}";
    }

    private static string GetFileExtension(string fileName)
    {
        var dotIndex = fileName.LastIndexOf('.');
        return $"{fileName[(dotIndex + 1)..]}";
    }
}