using Microsoft.AspNetCore.Mvc;
using ProSolution.BL.DTOs;
using ProSolution.BL.Services.Interfaces;

namespace ProSolution.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] BrandCreateDto dto)
        {
            await _brandService.CreateAsync(dto);
            return StatusCode(201);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromForm] BrandUpdateDto dto)
        {
            await _brandService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _brandService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> SoftDelete(string id, [FromQuery] bool isDelete)
        {
            await _brandService.SoftDeleteAsync(id, isDelete);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var brands = await _brandService.GetAllAsync();
            return Ok(brands);
        }

        [HttpGet("filtered")]
        public async Task<IActionResult> GetAllFiltered([FromQuery] string? search, [FromQuery] int take = 10, [FromQuery] int page = 1, [FromQuery] int order = 1, [FromQuery] bool isDeleted = false)
        {
            var result = await _brandService.GetAllFilteredAsync(search, take, page, order, isDeleted);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var brand = await _brandService.GetByIdAsync(id);
            return Ok(brand);
        }
    }
}
