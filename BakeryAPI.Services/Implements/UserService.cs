using BakeryShopAPI.Data.Entities;
using BakeryShopAPI.Data.Repositories; // Nhớ using Repository
using BakeryShopAPI.Services.DTOs;
using BakeryShopAPI.Services.Interfaces;
using BCrypt.Net; // Để dùng hàm Hash
using Microsoft.Extensions.Configuration; // Để đọc file appsettings
using Microsoft.IdentityModel.Tokens; // Để dùng SymmetricSecurityKey
using System.IdentityModel.Tokens.Jwt; // Để tạo Token
using System.Security.Claims; // Để lưu thông tin vào Token
using System.Text;

namespace BakeryShopAPI.Services.Implements
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IConfiguration _config;

        public UserService(IUserRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        public async Task RegisterAsync(UserRegisterDTO dto)
        {
            // 1. Kiểm tra Email đã tồn tại chưa
            if (await _repo.ExistsByEmailAsync(dto.Email))
            {
                throw new Exception("Email đã tồn tại!");
            }

            // 2. Mã hóa mật khẩu (Hashing)
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            // 3. Tạo Entity User mới
            var newUser = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                FullName = dto.FullName,
                PasswordHash = passwordHash, // Lưu chuỗi đã mã hóa
                Role = "Customer" // Mặc định là khách
            };

            // 4. Lưu xuống DB
            await _repo.AddAsync(newUser);
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            // 1. Tìm user trong DB
            var user = await _repo.GetByUsernameAsync(username);
            if (user == null)
            {
                throw new Exception("Tài khoản không tồn tại!");
            }

            // 2. Kiểm tra mật khẩu (So sánh hash)
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (!isPasswordValid)
            {
                throw new Exception("Mật khẩu không đúng!");
            }

            // 3. Tạo Token (In vé)
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(1), // Vé có hạn 1 ngày
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token); // Trả về chuỗi Token
        }
    }
}