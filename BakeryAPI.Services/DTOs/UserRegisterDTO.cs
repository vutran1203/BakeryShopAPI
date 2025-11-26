using System.ComponentModel.DataAnnotations;

namespace BakeryShopAPI.Services.DTOs
{
    public class UserRegisterDTO
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        public string FullName { get; set; }
    }
}