namespace BakeryShopAPI.Services.DTOs
{
    public class DashboardSummaryDTO
    {
        public int TotalProducts { get; set; }
        public int TotalOrders { get; set; }
        public int TotalUsers { get; set; }
        public decimal TotalRevenue { get; set; } // Tổng doanh thu
    }
}