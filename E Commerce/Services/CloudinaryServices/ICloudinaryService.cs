namespace E_Commerce.Services.CloudinaryServices
{
    public interface ICloudinaryService
    {
        Task<string> UploadImageAsync(IFormFile file);

    }
}
