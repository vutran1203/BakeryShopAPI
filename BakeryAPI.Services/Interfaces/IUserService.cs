using BakeryShopAPI.Services.DTOs;

namespace BakeryShopAPI.Services.Interfaces
{
    public interface IUserService
    {
        // Trả về true nếu đăng ký thành công, false hoặc throw lỗi nếu thất bại
        Task RegisterAsync(UserRegisterDTO registerDto);

        Task<string> LoginAsync(string username, string password);
    }
}