namespace BakeryShopAPI.Services.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; } // <--- BẮT BUỘC PHẢI CÓ DÒNG NÀY
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string ImageUrl { get; set; }
        public string CategoryName { get; set; }

        public bool IsBestSeller { get; set; }
    }
}