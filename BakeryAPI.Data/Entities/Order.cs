using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BakeryShopAPI.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        // Trạng thái đơn: "Pending" (Chờ xử lý), "Shipped" (Đã giao), "Cancelled"...
        public string Status { get; set; } = "Pending";

        public string ShippingAddress { get; set; }
        public string PhoneNumber { get; set; }

        [Precision(18, 2)]
        public decimal TotalAmount { get; set; } // Tổng tiền đơn hàng

        // Liên kết với User (Khách hàng)
        public int UserId { get; set; }
        public User User { get; set; }

        // Liên kết với chi tiết đơn hàng
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}