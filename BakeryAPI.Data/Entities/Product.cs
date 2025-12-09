using BakeryShopAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BakeryShopAPI.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Precision(18, 2)]
        public decimal Price { get; set; }
        // Các thuộc tính khác...
        public string? Description { get; set; } // Mô tả bánh
        public string ImageUrl { get; set; }    // Đường dẫn ảnh

        public int CategoryId { get; set; } // Khóa ngoại (bắt buộc có)
        public Category Category { get; set; } // Để truy xuất thông tin danh mục

        public bool IsBestSeller { get; set; } = false; // Mặc định là false
    }
}