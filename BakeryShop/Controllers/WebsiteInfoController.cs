using BakeryShopAPI.Data;
using BakeryShopAPI.Data.Entities;
using BakeryShopAPI.Services.DTOs;
using BakeryShopAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class WebsiteInfoController : ControllerBase
{
    private readonly BakeryDbContext _context;
    private readonly ICloudinaryService _cloudinaryService;

    public WebsiteInfoController(BakeryDbContext context, ICloudinaryService cloudinaryService)
    {
        _context = context;
        _cloudinaryService = cloudinaryService;
    }

    // GET: Lấy thông tin
    [HttpGet]
    public async Task<IActionResult> GetInfo()
    {
        var info = await _context.WebsiteInfos.FirstOrDefaultAsync();
        if (info == null) // Nếu chưa có thì tạo mới mặc định
        {
            info = new WebsiteInfo();
            _context.WebsiteInfos.Add(info);
            await _context.SaveChangesAsync();
        }
        return Ok(info);
    }

    // PUT: Cập nhật thông tin
    [HttpPatch]
    public async Task<IActionResult> UpdateInfo([FromForm] WebsiteInfoDTO request)
    {
        var info = await _context.WebsiteInfos.FirstOrDefaultAsync();
        if (info == null) return NotFound();

        // Cập nhật Text
        info.ShopName = request.ShopName ?? info.ShopName;
        info.Slogan = request.Slogan ?? info.Slogan;

        // Các trường bắt buộc (Không bao giờ bị lỗi null nữa vì luôn có fallback giá trị cũ)
        info.Address = request.Address ?? info.Address;
        info.ContactEmail = request.ContactEmail ?? info.ContactEmail;
        info.ContactPhone = request.ContactPhone ?? info.ContactPhone;
        info.FooterContent = request.FooterContent ?? info.FooterContent;

        // Các trường tùy chọn
        info.AboutUsTitle = request.AboutUsTitle ?? info.AboutUsTitle;
        info.AboutUsContent = request.AboutUsContent ?? info.AboutUsContent;

        // Cập nhật Ảnh (nếu có upload mới)
        if (request.LogoFile != null)
            info.LogoUrl = await _cloudinaryService.UploadImageAsync(request.LogoFile);

        if (request.BannerFile != null)
            info.BannerUrl = await _cloudinaryService.UploadImageAsync(request.BannerFile);

        if (request.AboutUsImageFile != null)
            info.AboutUsImageUrl = await _cloudinaryService.UploadImageAsync(request.AboutUsImageFile);

        await _context.SaveChangesAsync();
        return Ok(info);
    }
}