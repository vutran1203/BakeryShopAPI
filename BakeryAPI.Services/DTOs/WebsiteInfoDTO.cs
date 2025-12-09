using Microsoft.AspNetCore.Http;

namespace BakeryShopAPI.Services.DTOs
{
    public class WebsiteInfoDTO
    {
        // Text Fields
        public string ShopName { get; set; }
        public string Slogan { get; set; }
        public string Address { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string FooterContent { get; set; }
        public string? AboutUsTitle { get; set; }
        public string? AboutUsContent { get; set; }

        // File Uploads
        public IFormFile? LogoFile { get; set; }
        public IFormFile? BannerFile { get; set; }
        public IFormFile? AboutUsImageFile { get; set; }
    }
}