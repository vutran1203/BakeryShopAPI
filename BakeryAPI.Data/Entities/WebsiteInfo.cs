using System.ComponentModel.DataAnnotations;

namespace BakeryShopAPI.Data.Entities
{
    public class WebsiteInfo
    {
        [Key]
        public int Id { get; set; } // Luôn là 1

        // 1. Thông tin Chung & Banner
        public string ShopName { get; set; } = "Bakery Love";
        public string Slogan { get; set; } = "Đánh thức vị giác";
        public string? LogoUrl { get; set; }
        public string? BannerUrl { get; set; }

        // 2. Thông tin Footer (Liên hệ)
        public string Address { get; set; } = "123 Đường Bánh Ngọt, TP.HCM";
        public string ContactEmail { get; set; } = "order@bakery.com";
        public string ContactPhone { get; set; } = "0909 123 456";
        public string FooterContent { get; set; } = "© 2025 All Rights Reserved.";

        // 3. Thông tin Trang Giới Thiệu (About Us)
        public string? AboutUsTitle { get; set; } = "Về Chúng Tôi";
        public string? AboutUsContent { get; set; } = "Nội dung giới thiệu...";
        public string? AboutUsImageUrl { get; set; }

        public string? FacebookUrl { get; set; } = "https://m.me/tiembanhcuamai";

        public string Theme { get; set; } = "default";
        public bool SnowEffect { get; set; } = false;
    }
}