using BakeryShopAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;
namespace BakeryShopAPI.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; } // Mật khẩu đã mã hóa
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } = "Customer"; // Mặc định là Khách hàng
    }
}