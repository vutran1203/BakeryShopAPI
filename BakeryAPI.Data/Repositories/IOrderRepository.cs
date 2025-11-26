using BakeryShopAPI.Data.Entities;

namespace BakeryShopAPI.Data.Repositories
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        // Sau này sẽ thêm hàm GetOrdersByUserId, v.v.

        Task<List<Order>> GetOrdersByUserIdAsync(int userId);

        Task<List<Order>> GetAllOrdersAsync();

        // 2. Lấy 1 đơn theo ID (để Admin sửa trạng thái)
        Task<Order> GetByIdAsync(int id);

        // 3. Lưu cập nhật (sau khi sửa trạng thái)
        Task UpdateAsync(Order order);
    }
}