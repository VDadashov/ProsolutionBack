using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProSolution.BL.DTOs;
using ProSolution.BL.DTOs.Categories.CategoryItemDTO;
using ProSolution.BL.Exceptions.Common;
using ProSolution.BL.Services.Interfaces;

namespace ProSolution.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }



        [HttpGet("all")]
        public async Task<IActionResult> GetAll(string? search, bool isDeleted)
        {
            var categories = await _categoryService.GetAllAsync(search, isDeleted);
            return Ok(categories);
        }

        [HttpGet("filtered")]
        public async Task<IActionResult> GetAllFiltered(string? search, int take = 10, int page = 1, int order = 1, bool isDeleted = false)
        {
            var categories = await _categoryService.GetAllFilteredAsync(search, take, page, order, isDeleted);
            return Ok(categories);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryCreateDto dto)
        {
            await _categoryService.CreateAsync(dto);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPost("item")]
        public async Task<IActionResult> CreateItem([FromBody] CategoryItemCreateDto dto)
        {
            await _categoryService.CreateItemAsync(dto);
            return StatusCode(StatusCodes.Status201Created);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] CategoryUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new NegativIdException();

            await _categoryService.UpdateAsync(id, dto);
            return NoContent();
        }

     

        [HttpPatch("softdelete/{id}")]
        public async Task<IActionResult> SoftDelete(string id, [FromQuery] bool isDeleted)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new NegativIdException();

            await _categoryService.SoftDeleteAsync(id, isDeleted);
            return NoContent();
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new NegativIdException();

            var category = await _categoryService.GetByIdAsync(id);
            return Ok(category);
        }

      

        [HttpPut("item/{id}")]
        public async Task<IActionResult> UpdateItem(string id, [FromBody] CategoryItemUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new NegativIdException();

            await _categoryService.UpdateItemAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("item/{id}")]
        public async Task<IActionResult> DeleteItem(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new NegativIdException();

            await _categoryService.DeleteItemAsync(id);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new NegativIdException();

            await _categoryService.DeleteAsync(id);
            return NoContent();
        }

    }
}
