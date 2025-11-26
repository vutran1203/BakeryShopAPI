using BakeryShopAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BakeryShopAPI.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly BakeryDbContext _context;

        public OrderRepository(BakeryDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)       // Kèm bảng chi tiết
                    .ThenInclude(od => od.Product)  // Kèm tiếp bảng Product (để lấy tên, ảnh bánh)
                .Where(o => o.UserId == userId)     // Lọc theo người dùng
                .OrderByDescending(o => o.OrderDate) // Đơn mới nhất lên đầu
                .ToListAsync();
        }

        // 1. Lấy tất cả đơn (cho Admin)
        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.User) // Quan trọng: Lấy kèm thông tin người đặt
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product) // Lấy kèm chi tiết bánh
                .OrderByDescending(o => o.OrderDate) // Đơn mới nhất lên đầu
                .ToListAsync();
        }

        // 2. Tìm đơn theo ID
        public async Task<Order> GetByIdAsync(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        // 3. Update đơn
        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}