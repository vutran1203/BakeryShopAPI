namespace BakeryShopAPI.Services.DTOs
{
    // Kế thừa lại OrderDTO cũ để đỡ phải viết lại các trường như Id, TotalAmount...
    public class AdminOrderDTO : OrderDTO
    {
        public string CustomerName { get; set; } // Thêm tên khách hàng
        public string CustomerEmail { get; set; } // Thêm email để liên hệ
    }
}