using Microsoft.AspNetCore.Mvc;
using ProSolution.BL.DTOs;
using ProSolution.BL.Services.Interfaces;

namespace ProSolution.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeoDatasController : ControllerBase
    {
        private readonly ISeoDataService _seoDataService;

        public SeoDatasController(ISeoDataService seoDataService)
        {
            _seoDataService = seoDataService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SeoDataGetDto>>> GetAll()
        {
            var result = await _seoDataService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SeoDataGetDto>> GetById(string id)
        {
            var result = await _seoDataService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<SeoDataGetDto>> Create([FromBody] CreateSEODTO dto)
        {
            var created = await _seoDataService.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SeoDataGetDto>> Update(string id, [FromBody] SeoDataUpdateDto dto)
        {
            var updated = await _seoDataService.UpdateAsync(id, dto);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<SeoDataGetDto>> Delete(string id)
        {
            var deleted = await _seoDataService.DeleteAsync(id);
            return Ok(deleted);
        }
    }
}
