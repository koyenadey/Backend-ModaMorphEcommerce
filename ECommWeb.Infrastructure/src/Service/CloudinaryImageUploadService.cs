using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ECommWeb.Business.src.ServiceAbstract.EntityServiceAbstract;
using Microsoft.AspNetCore.Http;

namespace ECommWeb.Business.src.ServiceImplement.EntityServiceImplement;

public class CloudinaryImageUploadService : IImageUploadService
{
    private readonly Cloudinary _cloudinary;
    private readonly IConfiguration _configuration;
    public CloudinaryImageUploadService(IConfiguration configuration)
    {

        _configuration = configuration;

        var cloudName = _configuration["Cloudinary:CloudName"];
        var apiKey = _configuration["Cloudinary:ApiKey"];
        var apiSecret = _configuration["Cloudinary:ApiSecret"];

        var account = new Account(cloudName, apiKey, apiSecret);
        _cloudinary = new Cloudinary(account);
        _cloudinary.Api.Secure = true;
    }
    public async Task<List<string>> Upload(IEnumerable<IFormFile> files)
    {
        var imageUrls = new List<string>();
        foreach (var file in files)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, memoryStream),
                PublicId = Guid.NewGuid().ToString()
            };

            var result = _cloudinary.Upload(uploadParams);
            if (result.Error != null)
                throw new Exception($"Cloudinary error occured: {result.Error.Message}");

            imageUrls.Add(result.SecureUrl.ToString());
        }
        return imageUrls;
    }

    public async Task<string> Upload(IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(file.FileName, memoryStream),
            PublicId = Guid.NewGuid().ToString()
        };

        var result = await _cloudinary.UploadAsync(uploadParams);
        if (result.Error != null)
            throw new Exception($"Cloudinary error occured: {result.Error.Message}");

        var imageUrl = result.SecureUrl.ToString();

        return imageUrl;
    }
}
