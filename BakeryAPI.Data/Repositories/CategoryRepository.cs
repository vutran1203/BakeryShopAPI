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
    }
}