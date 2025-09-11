using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProSolution.BL.DTOs;
using ProSolution.BL.Exceptions.Common;
using ProSolution.BL.Services.Interfaces;

namespace ProSolution.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class BadgesController : ControllerBase
    {
        private readonly IBadgeService _badgeService;

        public BadgesController(IBadgeService badgeService)
        {
            _badgeService = badgeService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] BadgeCreateDto dto)
        {
            await _badgeService.CreateAsync(dto);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromForm] BadgeUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new NegativIdException();

            await _badgeService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new NegativIdException();

            await _badgeService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPatch("softdelete/{id}")]
        public async Task<IActionResult> SoftDelete(string id, [FromQuery] bool isDeleted)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new NegativIdException();

            await _badgeService.SoftDeleteAsync(id, isDeleted);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new NegativIdException();

            var badge = await _badgeService.GetByIdAsync(id);
            return Ok(badge);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var badges = await _badgeService.GetAllAsync();
            return Ok(badges);
        }

        [HttpGet("filtered")]
        public async Task<IActionResult> GetAllFiltered([FromQuery] string? search, [FromQuery] int take = 10, [FromQuery] int page = 1, [FromQuery] int order = 1, [FromQuery] bool isDeleted = false)
        {
            var result = await _badgeService.GetAllFilteredAsync(search, take, page, order, isDeleted);
            return Ok(result);
        }
    }
}
