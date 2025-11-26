using System.ComponentModel.DataAnnotations;

namespace BakeryShopAPI.Services.DTOs
{
    public class CartItemDTO
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }
    }
}