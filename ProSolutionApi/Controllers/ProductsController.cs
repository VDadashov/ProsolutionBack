using Microsoft.AspNetCore.Mvc;
using ProSolution.BL.DTOs;
using ProSolution.BL.DTOs.Products;
using ProSolution.BL.DTOs.Products.ProductReviews;
using ProSolution.BL.Services.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    // ---------- GET METHODS ----------

    [HttpGet("GetallFiltered")]
    public async Task<IActionResult> GetallFiltered(string? search, string? slug, int take, int skip, 
        int order, bool isDeleted, double? minPrice, double? maxPrice, bool isDiscount, [FromQuery(Name = "featureslugs")] List<string>? featureslugs = null)
    {
        return Ok(await _productService.GetAllFilteredAsync(search, slug, take, skip, order, isDeleted, minPrice, maxPrice, isDiscount, featureslugs));
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _productService.GetAllAsync());
    }

    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        return Ok(await _productService.GetByIdAsync(id));
    }
    [HttpGet("GetBySlug/{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        return Ok(await _productService.GetBySlugAsync(slug));
    }
    // ---------- POST METHODS ----------
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
    {
        //frombady deyisib stringi obyekte cevirecem
        await _productService.CreateAsync(dto);
        return Ok();
    }
    [HttpPost("ImageUpload")]
    public async Task<IActionResult> ImageUpload([FromForm] ImageUploadRequestDto dto)
    {
        // Вызываем сервис, передавая ему данные из DTO
        return Ok(await _productService.ImageUploadAsync(dto.File, dto.IsMain,dto.AltText));
    }

    [HttpDelete("DeleteImage")]
    public async Task<IActionResult> DeleteImage(string imagePath)
    {
       
        var resultMessage = await _productService.DeleteImage(imagePath);

      
        return Ok(resultMessage);
    }

    [HttpPost("CreateReview")]
    public async Task<IActionResult> CreateReview([FromBody] ProductReviewCreateDto dto)
    {
        await _productService.CreateReviewAsync(dto);
        return Ok(new { message = "Review added successfully." });
    }

    [HttpPost("SoldCount/{id}")]
    public async Task<IActionResult> SoldCount(string id, [FromBody] SoldCountDto dto)
    {
        await _productService.SoldCountUpdate(id,dto);
        return Ok(new { message = "Sold count updated successfully." });
    }

    // ---------- PUT METHODS ----------
    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] ProductUpdateDto dto)
    {
        await _productService.UpdateAsync(id, dto);
        return Ok();
    }

    // ---------- DELETE METHODS ----------
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _productService.DeleteAsync(id);
        return Ok();
    }

    [HttpDelete("SoftDelete/{id}")]
    public async Task<IActionResult> SoftDelete(string id, bool IsDeleted)
    {
        await _productService.SoftDeleteAsync(id, IsDeleted);
        return Ok(new
        {
            message = IsDeleted ? "Product deleted (soft delete) successfully." : "Product restored successfully."
        });
    }

    [HttpDelete("DeleteReview/{id}")]
    public async Task<IActionResult> DeleteReview(string id)
    {
        await _productService.DeleteReviewAsync(id);
        return Ok(new { message = "Review deleted successfully." });
    }
}
