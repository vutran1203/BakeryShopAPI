using BakeryShopAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BakeryShopAPI.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BakeryDbContext _context;

        public UserRepository(BakeryDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            // Trả về true nếu tìm thấy bất kỳ user nào có email này
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}