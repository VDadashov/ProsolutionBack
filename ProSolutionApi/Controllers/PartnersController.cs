using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProSolution.BL.DTOs;
using ProSolution.BL.Exceptions.Common;
using ProSolution.BL.Services.Interfaces;

namespace ProSolution.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartnersController : ControllerBase
    {
        private readonly IPartnerService _partnerService;

        public PartnersController(IPartnerService partnerService)
        {
            _partnerService = partnerService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] PartnerCreateDto dto)
        {
            await _partnerService.CreateAsync(dto);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromForm] PartnerUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new NegativIdException();

            await _partnerService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new NegativIdException();

            await _partnerService.DeleteAsync(id);
            return NoContent();
        }

        [HttpDelete("softdelete/{id}")]
        public async Task<IActionResult> SoftDelete(string id, [FromQuery] bool isDeleted)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new NegativIdException();

            await _partnerService.SoftDeleteAsync(id, isDeleted);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new NegativIdException();

            var partner = await _partnerService.GetByIdAsync(id);
            return Ok(partner);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var partners = await _partnerService.GetAllAsync();
            return Ok(partners);
        }

        [HttpGet("filtered")]
        public async Task<IActionResult> GetAllFiltered([FromQuery] string? search, [FromQuery] int take = 10, [FromQuery] int page = 1, [FromQuery] int order = 1, [FromQuery] bool isDeleted = false)
        {
            var partners = await _partnerService.GetAllFilteredAsync(search, take, page, order, isDeleted);
            return Ok(partners);
        }
    }
}
