using BakeryShopAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BakeryShopAPI.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BakeryDbContext _context;

        public CategoryRepository(BakeryDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        // Hàm AddAsync đã có chưa? Nếu chưa thì thêm vào:
        public async Task AddAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        // Thêm hàm Xóa:
        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}