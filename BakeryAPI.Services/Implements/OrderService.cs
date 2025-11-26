using BakeryShopAPI.Data;
using BakeryShopAPI.Data.Entities;
using BakeryShopAPI.Data.Repositories;
using BakeryShopAPI.Services.DTOs;
using BakeryShopAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BakeryShopAPI.Services.Implements
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IProductRepository _productRepo; // Cần cái này để lấy giá bánh
        private readonly BakeryDbContext _context;

        public OrderService(IOrderRepository orderRepo, IProductRepository productRepo, BakeryDbContext context)
        {
            _orderRepo = orderRepo;
            _productRepo = productRepo;
            _context = context;
        }

        public async Task CreateOrderAsync(int userId, OrderCreateDTO orderDto)
        {
            // 1. Tạo object Order (Header)
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                ShippingAddress = orderDto.ShippingAddress,
                PhoneNumber = orderDto.PhoneNumber,
                Status = "Pending",
                OrderDetails = new List<OrderDetail>()
            };

            decimal totalAmount = 0;

            // 2. Duyệt qua từng món khách mua để tính tiền và tạo Detail
            foreach (var item in orderDto.Items)
            {
                // Lấy thông tin bánh từ DB để có GIÁ CHUẨN
                var product = await _productRepo.GetByIdAsync(item.ProductId);
                if (product == null) continue; // Hoặc throw lỗi nếu muốn chặt chẽ

                var detail = new OrderDetail
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price // Quan trọng: Lấy giá từ DB
                };

                // Cộng dồn tổng tiền
                totalAmount += (product.Price * item.Quantity);

                // Thêm vào list chi tiết
                order.OrderDetails.Add(detail);
            }

            order.TotalAmount = totalAmount;

            // 3. Lưu tất cả xuống DB
            await _orderRepo.AddAsync(order);
        }

        public async Task<List<OrderDTO>> GetMyOrdersAsync(int userId)
        {
            // 1. Lấy dữ liệu thô từ Repo
            var orders = await _orderRepo.GetOrdersByUserIdAsync(userId);

            // 2. Chuyển đổi sang DTO (Mapping)
            return orders.Select(o => new OrderDTO
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                Status = o.Status,
                ShippingAddress = o.ShippingAddress,
                PhoneNumber = o.PhoneNumber,
                TotalAmount = o.TotalAmount,
                Items = o.OrderDetails.Select(od => new OrderDetailDTO
                {
                    ProductName = od.Product.Name,
                    ProductImage = od.Product.ImageUrl,
                    Quantity = od.Quantity,
                    UnitPrice = od.UnitPrice
                }).ToList()
            }).ToList();
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.User)               // KÈM THÔNG TIN USER
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task<List<AdminOrderDTO>> GetAllOrdersForAdminAsync()
        {
            var orders = await _orderRepo.GetAllOrdersAsync();

            return orders.Select(o => new AdminOrderDTO
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                Status = o.Status,
                ShippingAddress = o.ShippingAddress,
                PhoneNumber = o.PhoneNumber,
                TotalAmount = o.TotalAmount,
                // Mapping thông tin User
                CustomerName = o.User?.FullName ?? o.User?.Username,
                CustomerEmail = o.User?.Email,
                // Mapping chi tiết đơn (giống hệt cũ)
                Items = o.OrderDetails.Select(od => new OrderDetailDTO
                {
                    ProductName = od.Product.Name,
                    ProductImage = od.Product.ImageUrl,
                    Quantity = od.Quantity,
                    UnitPrice = od.UnitPrice
                }).ToList()
            }).ToList();
        }

        public async Task UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            var order = await _orderRepo.GetByIdAsync(orderId);
            if (order == null) throw new Exception("Đơn hàng không tồn tại!");

            order.Status = newStatus; // Cập nhật trạng thái mới
            await _orderRepo.UpdateAsync(order);
        }
    }
}