using System.ComponentModel.DataAnnotations;

namespace BakeryShopAPI.Services.DTOs
{
    public class OrderCreateDTO
    {
        [Required]
        public string ShippingAddress { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        // Danh sách bánh khách chọn mua
        public List<CartItemDTO> Items { get; set; }
    }
}