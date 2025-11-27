using BakeryShopAPI.Data.Repositories;
using BakeryShopAPI.Services.DTOs;
using BakeryShopAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BakeryShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepo;

        public AuthController(IUserService userService, IUserRepository userRepo)
        {
            _userService = userService;
            _userRepo = userRepo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO registerDto)
        {
            try
            {
                await _userService.RegisterAsync(registerDto);
                return Ok(new { Message = "Đăng ký thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // Class DTO dùng tạm (hoặc bạn tạo file riêng trong folder DTOs càng tốt)
        public class UserLoginDTO
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        // Trong AuthController
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO loginDto)
        {
            try
            {
                // 1. Lấy Token từ Service (Chuỗi string đã mã hóa xong)
                var tokenString = await _userService.LoginAsync(loginDto.Username, loginDto.Password);

                // 2. Lấy thông tin User từ Database để trả về cho Frontend
                var user = await _userRepo.GetByUsernameAsync(loginDto.Username);

                // 3. Trả về kết quả gộp
                return Ok(new
                {
                    Token = tokenString, // Vé thông hành
                    User = new
                    {
                        Id = user.Id,
                        Username = user.Username,
                        FullName = user.FullName,
                        Role = user.Role // Frontend cần cái này để phân quyền Admin
                    }
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }

        [HttpGet("me")]
        [Authorize] // <--- Attribute quan trọng: BẮT BUỘC PHẢI CÓ TOKEN MỚI VÀO ĐƯỢC
        public IActionResult GetMyProfile()
        {
            // Lấy ID user từ Token (User đã đăng nhập)
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new
            {
                Id = userId,
                Username = username,
                Role = role,
                Message = "Đây là thông tin mật, chỉ xem được khi đã đăng nhập!"
            });
        }
    }
}