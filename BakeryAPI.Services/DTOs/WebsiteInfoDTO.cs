using Microsoft.AspNetCore.Http;

namespace BakeryShopAPI.Services.DTOs
{
    public class WebsiteInfoDTO
    {
        // 👇 Thêm dấu ? vào tất cả các dòng string
        public string? ShopName { get; set; }
        public string? Slogan { get; set; }

        public string? Address { get; set; }        // <--- Quan trọng
        public string? ContactEmail { get; set; }   // <--- Quan trọng
        public string? ContactPhone { get; set; }   // <--- Quan trọng
        public string? FooterContent { get; set; }  // <--- Quan trọng

        public string? AboutUsTitle { get; set; }
        public string? AboutUsContent { get; set; }

        public IFormFile? LogoFile { get; set; }
        public IFormFile? BannerFile { get; set; }
        public IFormFile? AboutUsImageFile { get; set; }

        public string? FacebookUrl { get; set; }

        public string? Theme { get; set; }
        public bool? SnowEffect { get; set; }
    }
}