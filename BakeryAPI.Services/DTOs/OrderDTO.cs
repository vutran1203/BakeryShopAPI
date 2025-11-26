namespace BakeryShopAPI.Services.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } // Pending, Shipped...
        public string ShippingAddress { get; set; }
        public string PhoneNumber { get; set; }
        public decimal TotalAmount { get; set; }

        // Danh sách các món trong đơn này
        public List<OrderDetailDTO> Items { get; set; }
    }
}