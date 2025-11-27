using BakeryShopAPI.Data.Entities;
using BakeryShopAPI.Data.Repositories;
using BakeryShopAPI.Services.DTOs;
using BakeryShopAPI.Services.Interfaces;

namespace BakeryShopAPI.Services.Implements
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly ICloudinaryService _cloudinaryService;

        public ProductService(IProductRepository repo, ICloudinaryService cloudinaryService)
        {
            _repo = repo;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<List<ProductDTO>> GetAllProductsAsync()
        {
            var products = await _repo.GetAllAsync();

            return products.Select(p => new ProductDTO
            {
                Id = p.Id, // <--- ĐẢM BẢO DÒNG NÀY KHÔNG BỊ THIẾU
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                CategoryName = p.Category?.Name
            }).ToList();
        }

        public async Task CreateProductAsync(ProductCreateDTO dto)
        {
            // 1. Upload ảnh lên Cloudinary (chỉ 1 dòng code!)
            string imageUrl = null;
            if (dto.ImageFile != null)
            {
                imageUrl = await _cloudinaryService.UploadImageAsync(dto.ImageFile);
            }

            // 2. Lưu vào DB
            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                ImageUrl = imageUrl // Bây giờ link sẽ là https://res.cloudinary.com/...
            };

            await _repo.AddAsync(product);
        }
    }
}