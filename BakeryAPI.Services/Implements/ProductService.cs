using BakeryShopAPI.Data;
using BakeryShopAPI.Data.Entities;
using BakeryShopAPI.Data.Repositories;
using BakeryShopAPI.Services.DTOs;
using BakeryShopAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BakeryShopAPI.Services.Implements
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly BakeryDbContext _context;

        public ProductService(IProductRepository repo, ICloudinaryService cloudinaryService, BakeryDbContext context)
        {
            _repo = repo;
            _cloudinaryService = cloudinaryService;
            _context = context;
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
                Description = string.IsNullOrEmpty(dto.Description) ? "Chưa có mô tả" : dto.Description,
                CategoryId = dto.CategoryId,
                ImageUrl = imageUrl,
                IsBestSeller = dto.IsBestSeller
            };

            await _repo.AddAsync(product);
        }

        public async Task UpdateProductAsync(ProductUpdateDTO request)
        {
            // 1. Tìm sản phẩm cũ trong DB
            var product = await _context.Products.FindAsync(request.Id);
            if (product == null) throw new Exception("Không tìm thấy sản phẩm");

            // 2. LOGIC PATCH (Quan trọng): Chỉ cập nhật nếu có dữ liệu mới

            // Nếu Name có gửi lên và khác rỗng -> Cập nhật. Nếu null -> Giữ nguyên cái cũ.
            if (!string.IsNullOrEmpty(request.Name))
                product.Name = request.Name;

            // Nếu Price có giá trị -> Cập nhật
            if (request.Price.HasValue)
                product.Price = request.Price.Value;

            // Nếu Description khác null -> Cập nhật (Cho phép cập nhật thành chuỗi rỗng nếu muốn xóa mô tả)
            // Lưu ý: Nếu bạn muốn "không nhập thì giữ nguyên" cho Description, dùng !string.IsNullOrEmpty
            if (request.Description != null)
                product.Description = request.Description;

            if (request.CategoryId.HasValue)
                product.CategoryId = request.CategoryId.Value;

            // 3. Xử lý ảnh (Nếu có up ảnh mới thì mới thay, không thì giữ ảnh cũ)
            if (request.ImageFile != null)
            {
                // Gọi hàm upload ảnh lên Cloudinary/Server
                var newImageUrl = await _cloudinaryService.UploadImageAsync(request.ImageFile);
                product.ImageUrl = newImageUrl;
            }

            product.IsBestSeller = (bool)request.IsBestSeller;

            // 4. Lưu thay đổi
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}