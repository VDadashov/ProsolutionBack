using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProSolution.BL.DTOs;
using ProSolution.BL.Exceptions.Common;
using ProSolution.BL.Services.Interfaces;

namespace ProSolution.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlidersController : ControllerBase
    {
        private readonly ISliderService _sliderService;

        public SlidersController(ISliderService sliderService)
        {
            _sliderService = sliderService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] SliderCreateDto dto)
        {
            await _sliderService.CreateAsync(dto);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromForm] SliderUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new NegativIdException();

            await _sliderService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new NegativIdException();

            await _sliderService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPatch("softdelete/{id}")]
        public async Task<IActionResult> SoftDelete(string id, [FromQuery] bool isDeleted)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new NegativIdException();

            await _sliderService.SoftDeleteAsync(id, isDeleted);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new NegativIdException();

            var slider = await _sliderService.GetByIdAsync(id);
            return Ok(slider);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var sliders = await _sliderService.GetAllAsync();
            return Ok(sliders);
        }

        [HttpGet("filtered")]
        public async Task<IActionResult> GetAllFiltered([FromQuery] string? search, [FromQuery] int take = 10, [FromQuery] int page = 1, [FromQuery] int order = 1, [FromQuery] bool isDeleted = false)
        {
            var sliders = await _sliderService.GetAllFilteredAsync(search, take, page, order, isDeleted);
            return Ok(sliders);
        }
    }
}
