using BakeryShopAPI.Services.DTOs; // Import để dùng được ProductDTO

namespace BakeryShopAPI.Services.Interfaces
{
    public interface IProductService
    {
        // Đây là bản hợp đồng: Controller chỉ cần biết Service có hàm này
        // Input: Không có
        // Output: Danh sách ProductDTO (không phải Entity Product)
        Task<List<ProductDTO>> GetAllProductsAsync();

        Task CreateProductAsync(ProductCreateDTO dto);

        Task UpdateProductAsync(ProductUpdateDTO request);
    }
}