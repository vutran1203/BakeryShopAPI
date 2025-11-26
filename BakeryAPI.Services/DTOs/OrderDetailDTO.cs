namespace BakeryShopAPI.Services.DTOs
{
    public class OrderDetailDTO
    {
        public string ProductName { get; set; } // Tên bánh
        public string ProductImage { get; set; } // Ảnh bánh
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}