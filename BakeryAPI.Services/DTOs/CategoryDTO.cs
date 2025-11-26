namespace BakeryShopAPI.Services.DTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // Không cần lấy Products hay Description dài dòng
    }
}