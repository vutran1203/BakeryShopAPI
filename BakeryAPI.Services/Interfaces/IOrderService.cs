using BakeryShopAPI.Data.Entities;
using BakeryShopAPI.Services.DTOs;

namespace BakeryShopAPI.Services.Interfaces
{
    public interface IOrderService
    {
        // userId: Để biết ai là người đặt
        Task CreateOrderAsync(int userId, OrderCreateDTO orderDto);

        Task<List<OrderDTO>> GetMyOrdersAsync(int userId);

        // Lấy tất cả đơn hàng (cho Admin)
        Task<List<Order>> GetAllOrdersAsync();

        // Tìm đơn theo ID (để sửa trạng thái)
        Task<Order> GetByIdAsync(int id);

        // Lưu thay đổi (sau khi sửa trạng thái)
        Task UpdateAsync(Order order);

        Task<List<AdminOrderDTO>> GetAllOrdersForAdminAsync();
        Task UpdateOrderStatusAsync(int orderId, string newStatus);
    }
}