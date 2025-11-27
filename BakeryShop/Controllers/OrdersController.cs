using BakeryShopAPI.Hubs;
using BakeryShopAPI.Services.DTOs;
using BakeryShopAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace BakeryShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Bắt buộc phải đăng nhập mới được đặt hàng
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _service;
        private readonly IHubContext<NotificationHub> _hubContext;

        public OrdersController(IOrderService service, IHubContext<NotificationHub> hubContext)
        {
            _service = service;
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDTO orderDto)
        {
            try
            {
                // Lấy ID người dùng từ Token
                var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                int userId = int.Parse(userIdString);

                await _service.CreateOrderAsync(userId, orderDto);

                await _hubContext.Clients.All.SendAsync("ReceiveOrder", "Khách vừa đặt đơn hàng mới!");
                return Ok(new { Message = "Đặt hàng thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        [HttpGet("my-orders")]
        public async Task<IActionResult> GetMyOrders()
        {
            try
            {
                // Lấy ID từ Token
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var orders = await _service.GetMyOrdersAsync(userId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 1. Xem tất cả đơn (Admin)
        [HttpGet("admin/all")]
        [Authorize(Roles = "Admin")] // <--- QUAN TRỌNG: Chỉ Admin mới vào được
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _service.GetAllOrdersForAdminAsync();
            return Ok(orders);
        }

        // 2. Cập nhật trạng thái đơn
        [HttpPut("admin/{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status) // Nhận chuỗi status từ Body
        {
            try
            {
                await _service.UpdateOrderStatusAsync(id, status);
                return Ok(new { Message = "Cập nhật trạng thái thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}