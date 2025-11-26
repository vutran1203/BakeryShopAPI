using Microsoft.AspNetCore.Http;

namespace BakeryShopAPI.Services.Interfaces
{
    public interface ICloudinaryService
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}