using Microsoft.EntityFrameworkCore; // Để dùng Precision

namespace BakeryShopAPI.Data.Entities
{
    public class OrderDetail
    {
        public int Id { get; set; }

        public int Quantity { get; set; } // Số lượng mua

        [Precision(18, 2)]
        public decimal UnitPrice { get; set; } // Giá tại thời điểm mua (Lưu cứng)

        // Liên kết với Order
        public int OrderId { get; set; }
        public Order Order { get; set; }

        // Liên kết với Product
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}