using BakeryShopAPI.Data.Entities; // Nhớ dùng đúng Namespace mới sửa

namespace BakeryShopAPI.Data.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
    }
}