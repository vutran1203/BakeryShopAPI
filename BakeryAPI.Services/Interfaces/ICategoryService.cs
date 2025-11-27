using BakeryShopAPI.Services.DTOs;

namespace BakeryShopAPI.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetAllCategoriesAsync();

        Task CreateCategoryAsync(string name);
        Task DeleteCategoryAsync(int id);
    }
}