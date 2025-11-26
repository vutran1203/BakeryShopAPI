using BakeryShopAPI.Services.DTOs;

namespace BakeryShopAPI.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetAllCategoriesAsync();
    }
}