using BakeryShopAPI.Data;
using BakeryShopAPI.Services.DTOs;
using BakeryShopAPI.Services.Implements;
using BakeryShopAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;
    private readonly BakeryDbContext _context;
    private readonly ICloudinaryService _cloudinaryService;

    public ProductsController(IProductService service, BakeryDbContext context, ICloudinaryService cloudinaryService)
    {
        _service = service;
        _context = context;
        _cloudinaryService = cloudinaryService;
    }

    // GET: api/Products?search=...&categoryId=1&page=1...
    [HttpGet]
    public async Task<IActionResult> GetProducts(string? search, int? categoryId, bool? isBestSeller, int page = 1, int pageSize = 12)
    {
        var query = _context.Products.Include(p => p.Category).AsQueryable();

        // 1. Lọc theo từ khóa
        if (!string.IsNullOrEmpty(search))
        {
            // Bước 1: Đưa từ khóa tìm kiếm về chữ thường
            var searchLower = search.ToLower();

            // Bước 2: Đưa cả Tên và Mô tả trong DB về chữ thường rồi mới so sánh
            query = query.Where(p => p.Name.ToLower().Contains(searchLower)
                                  || p.Description.ToLower().Contains(searchLower));
        }

        // 2. Lọc theo danh mục (NẾU CÓ)
        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        // 👇 THÊM ĐOẠN NÀY: Lọc Best Seller
        if (isBestSeller.HasValue && isBestSeller.Value == true)
        {
            query = query.Where(p => p.IsBestSeller == true);
        }

        // ... (Đoạn tính toán Total và Phân trang giữ nguyên) ...
        int totalItems = await query.CountAsync();

        var products = await query
            .OrderByDescending(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var productDTOs = products.Select(p => new ProductDTO
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            Description = p.Description,
            ImageUrl = p.ImageUrl,
            CategoryName = p.Category?.Name,
            IsBestSeller = p.IsBestSeller
        }).ToList();

        return Ok(new
        {
            Data = productDTOs,
            Total = totalItems,
            Page = page,
            PageSize = pageSize
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDTO>> GetProduct(int id)
    {
        // Tìm bánh theo ID
        var product = await _context.Products
            .Include(p => p.Category) // Lấy kèm tên danh mục
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return NotFound(new { Message = "Không tìm thấy bánh này!" });
        }

        // Chuyển đổi sang DTO để trả về
        return Ok(new ProductDTO
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Description = product.Description,
            ImageUrl = product.ImageUrl,
            CategoryName = product.Category?.Name
        });
    }

    [HttpPost]
    [Authorize(Roles = "Admin")] // <--- CHỈ ADMIN MỚI ĐƯỢC GỌI
    public async Task<IActionResult> Create([FromForm] ProductCreateDTO dto) // Dùng [FromForm] để nhận file
    {
        try
        {
            await _service.CreateProductAsync(dto);
            return Ok(new { Message = "Thêm bánh thành công!" });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Hàm này giúp tách Public ID từ URL
    private string GetPublicIdFromUrl(string url)
    {
        if (string.IsNullOrEmpty(url)) return null;

        try
        {
            // 1. Chỉ xử lý nếu là link Cloudinary
            if (!url.Contains("cloudinary.com")) return null;

            // 2. Tách phần mở rộng (.jpg, .png)
            int lastDotIndex = url.LastIndexOf('.');
            string urlWithoutExtension = url.Substring(0, lastDotIndex);

            // 3. Tìm vị trí của folder (sau chữ upload/v.../)
            // Cloudinary thường có dạng .../upload/v123456/FOLDER/NAME
            // Chúng ta tìm dấu gạch chéo cuối cùng và lùi lại để lấy folder

            // Cách đơn giản nhất: Lấy từ tên folder bạn đặt (bakery-shop-products)
            // Nếu bạn đổi tên folder trong CloudinaryService thì nhớ đổi ở đây nhé
            string folderName = "bakery-shop-products";
            int folderIndex = urlWithoutExtension.IndexOf(folderName);

            if (folderIndex != -1)
            {
                return urlWithoutExtension.Substring(folderIndex);
            }

            return null;
        }
        catch
        {
            return null; // Nếu lỗi thì bỏ qua, không xóa ảnh
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")] // Chỉ Admin được xóa
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        // 👇 BƯỚC 1: Xóa ảnh trên Cloudinary trước
        if (!string.IsNullOrEmpty(product.ImageUrl))
        {
            string publicId = GetPublicIdFromUrl(product.ImageUrl);
            if (publicId != null)
            {
                await _cloudinaryService.DeleteImageAsync(publicId);
            }
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return Ok(new { Message = "Xóa thành công!" });
    }

    [HttpPut("{id}")] // Dùng PUT (hoặc PATCH đều được)
    public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductUpdateDTO request)
    {
        if (id != request.Id) return BadRequest("ID không khớp");

        try
        {
            await _service.UpdateProductAsync(request);
            return Ok("Cập nhật thành công");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}