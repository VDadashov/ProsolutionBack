using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProSolution.BL.DTOs;
using ProSolution.BL.Exceptions.Common;
using ProSolution.BL.Services.Interfaces;

namespace ProSolution.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OurServicesController : ControllerBase
    {
        private readonly IOurServiceService _ourServiceService;

        public OurServicesController(IOurServiceService ourServiceService)
        {
            _ourServiceService = ourServiceService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] OurServiceCreateDto dto)
        {
            await _ourServiceService.CreateAsync(dto);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromForm] OurServiceUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new NegativIdException();

            await _ourServiceService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new NegativIdException();

            await _ourServiceService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPatch("softdelete/{id}")]
        public async Task<IActionResult> SoftDelete(string id, [FromQuery] bool isDeleted)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new NegativIdException();

            await _ourServiceService.SoftDeleteAsync(id, isDeleted);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new NegativIdException();

            var service = await _ourServiceService.GetByIdAsync(id);
            return Ok(service);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var services = await _ourServiceService.GetAllAsync();
            return Ok(services);
        }

        [HttpGet("filtered")]
        public async Task<IActionResult> GetAllFiltered([FromQuery] string? search, [FromQuery] int take = 10, [FromQuery] int page = 1, [FromQuery] int order = 1, [FromQuery] bool isDeleted = false)
        {
            var services = await _ourServiceService.GetAllFilteredAsync(search, take, page, order, isDeleted);
            return Ok(services);
        }
    }
}
