using Microsoft.AspNetCore.Http;

namespace BakeryShopAPI.Services.DTOs
{
    public class ProductUpdateDTO
    {
        public int Id { get; set; } // Cần ID để biết sửa cái nào
        public string? Name { get; set; } // Có dấu ?
        public decimal? Price { get; set; } // Có dấu ?
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public IFormFile? ImageFile { get; set; } // Ảnh mới (nếu có)

        public bool? IsBestSeller { get; set; } // Có dấu ?
    }
}