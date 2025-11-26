// IProductRepository.cs
using BakeryShopAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;
namespace BakeryShopAPI.Data.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task AddAsync(Product product);

        Task<Product> GetByIdAsync(int id);
    }
}

