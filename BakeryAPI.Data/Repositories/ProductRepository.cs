using BakeryShopAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;
namespace BakeryShopAPI.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly BakeryDbContext _context;
        public ProductRepository(BakeryDbContext context) => _context = context;

        public async Task<List<Product>> GetAllAsync() => await _context.Products.ToListAsync();
        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            // Tìm bánh theo ID
            return await _context.Products.FindAsync(id);
        }
    }
}