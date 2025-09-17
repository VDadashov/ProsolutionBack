using Microsoft.AspNetCore.Mvc;
using ProSolution.BL.DTOs.Characteristics;
using ProSolution.BL.DTOs.FeatureOPtions;
using ProSolution.BL.Exceptions.Common;
using ProSolution.BL.Services.Interfaces;
using ProSolution.Core.Entities;

namespace ProSolution.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeatureOptionsController : ControllerBase
    {
        private readonly IFeatureOptionService _featureOptionService;

        public FeatureOptionsController(IFeatureOptionService featureOptionService)
        {
            _featureOptionService = featureOptionService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FeatureOptionCreateDto dto)
        {
            await _featureOptionService.CreateAsync(dto);
            return StatusCode(StatusCodes.Status201Created);
        }

        // [HttpPut]
        // public async Task<IActionResult> Update([FromBody] FeatureOptionUpdateDto dto)
        // {
        //     await _featureOptionService.UpdateAsync(dto);
        //     return NoContent();
        // }
        
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] FeatureOptionUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Id cannot be empty.");

            if (dto == null)
                return BadRequest("Feature option data is required.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _featureOptionService.UpdateAsync(id, dto);
                return NoContent();
            }
            catch (NotFoundException<FeatureOption>)
            {
                return NotFound("Feature option not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(400, "An error occurred while updating the feature option.");
            }
        }
        
        

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new NegativIdException();

            await _featureOptionService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new NegativIdException();

            var data = await _featureOptionService.GetByIdAsync(id);
            return Ok(data);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _featureOptionService.GetAll();
            return Ok(data);
        }

        [HttpGet("filtred-pagination")]
        public async Task<IActionResult> GetAll([FromQuery] string? search = null, [FromQuery] int take = 10, [FromQuery] int page = 1, [FromQuery] int order = 1, [FromQuery] bool IsDeleted = false)
        {
            var data = await _featureOptionService.GetAllFilteredAsync(search, take, page, order, IsDeleted);
            return Ok(data);
        }

        // ---------------- FeatureOptionItem (вложенные элементы) -----------------

        [HttpPost("items")]
        public async Task<IActionResult> CreateItem([FromBody] FeatureOptionItemCreateDto dto)
        {
            await _featureOptionService.CreateItemAsync(dto);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("items")]
        public async Task<IActionResult> UpdateItem([FromBody] FeatureOptionItemUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Id))
                throw new NegativIdException();

            await _featureOptionService.UpdateItemAsync(dto);
            return NoContent();
        }

        [HttpDelete("items/{id}")]
        public async Task<IActionResult> DeleteItem(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new NegativIdException();

            await _featureOptionService.DeleteItemAsync(id);
            return NoContent();
        }
    }
}
