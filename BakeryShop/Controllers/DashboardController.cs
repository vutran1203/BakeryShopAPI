using BakeryShopAPI.Data;
using BakeryShopAPI.Services.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakeryShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DashboardController : ControllerBase
    {
        private readonly BakeryDbContext _context;

        public DashboardController(BakeryDbContext context)
        {
            _context = context;
        }

        [HttpGet("summary")]
        public async Task<ActionResult<DashboardSummaryDTO>> GetSummary()
        {
            // Tính toán các số liệu
            var totalProducts = await _context.Products.CountAsync();
            var totalOrders = await _context.Orders.CountAsync();
            var totalUsers = await _context.Users.CountAsync();

            // Tính tổng tiền các đơn hàng (chỉ tính đơn không bị hủy)
            var totalRevenue = await _context.Orders
                .Where(o => o.Status != "Cancelled")
                .SumAsync(o => o.TotalAmount);

            return new DashboardSummaryDTO
            {
                TotalProducts = totalProducts,
                TotalOrders = totalOrders,
                TotalUsers = totalUsers,
                TotalRevenue = totalRevenue
            };
        }
    }
}