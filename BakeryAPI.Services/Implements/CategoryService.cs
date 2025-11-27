using BakeryShopAPI.Data.Entities;
using BakeryShopAPI.Data.Repositories;
using BakeryShopAPI.Services.DTOs;
using BakeryShopAPI.Services.Interfaces;

namespace BakeryShopAPI.Services.Implements
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;

        public CategoryService(ICategoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<CategoryDTO>> GetAllCategoriesAsync()
        {
            var categories = await _repo.GetAllAsync();

            // Chuyển đổi (Mapping) từ Entity sang DTO
            return categories.Select(c => new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();
        }

        public async Task CreateCategoryAsync(string name)
        {
            var category = new Category { Name = name, Description = "" };
            await _repo.AddAsync(category);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}   