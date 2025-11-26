using BakeryShopAPI.Services.DTOs;
using BakeryShopAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllProductsAsync();
        return Ok(result);
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
}