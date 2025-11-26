using Microsoft.AspNetCore.Http; // Để dùng IFormFile
using System.ComponentModel.DataAnnotations;

namespace BakeryShopAPI.Services.DTOs
{
    public class ProductCreateDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string Description { get; set; }

        [Required]
        public int CategoryId { get; set; }

        // Đây là chỗ hứng file ảnh gửi lên
        public IFormFile ImageFile { get; set; }
    }
}