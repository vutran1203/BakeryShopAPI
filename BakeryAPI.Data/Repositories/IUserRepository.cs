using BakeryShopAPI.Data.Entities;

namespace BakeryShopAPI.Data.Repositories
{
    public interface IUserRepository
    {
        // Kiểm tra xem email đã có người dùng chưa (tránh trùng)
        Task<bool> ExistsByEmailAsync(string email);

        // Thêm user mới vào DB
        Task AddAsync(User user);

        // Lấy thông tin user bằng username (dùng cho lúc Đăng nhập sau này)
        Task<User> GetByUsernameAsync(string username);
    }
}