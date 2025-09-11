using Microsoft.AspNetCore.Mvc;
using ProSolution.BL.DTOs;
using ProSolution.BL.Services.Interfaces;

namespace ProSolution.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeoUrlsController : ControllerBase
    {
        private readonly ISeoUrlService _seoUrlService;

        public SeoUrlsController(ISeoUrlService seoUrlService)
        {
            _seoUrlService = seoUrlService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SEOUrlDTO>>> GetAll()
        {
            var result = await _seoUrlService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SEOUrlDTO>> GetById(string id)
        {
            var result = await _seoUrlService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<SEOUrlDTO>> Create([FromBody] CreateSeoUrlDTO dto)
        {
            var created = await _seoUrlService.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SEOUrlDTO>> Update(string id, [FromBody] UpdateSEOUrlDTO dto)
        {
            var updated = await _seoUrlService.UpdateAsync(id, dto);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<SEOUrlDTO>> Delete(string id)
        {
            var deleted = await _seoUrlService.DeleteAsync(id);
            return Ok(deleted);
        }
    }
}
