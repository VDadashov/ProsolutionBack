using Microsoft.AspNetCore.Mvc;
using ProSolution.BL.DTOs;
using ProSolution.BL.Services.Interfaces;

namespace ProSolution.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeoMetasController : ControllerBase
    {
        private readonly ISeoMetaService _seoMetaService;

        public SeoMetasController(ISeoMetaService seoMetaService)
        {
            _seoMetaService = seoMetaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SEOMetaDTO>>> GetAll()
        {
            var result = await _seoMetaService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SEOMetaDTO>> GetById(string id)
        {
            var result = await _seoMetaService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<SEOMetaDTO>> Create([FromBody] CreateSEOMetaDTO dto)
        {
            var created = await _seoMetaService.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SEOMetaDTO>> Update(string id, [FromBody] UpdateSEOMetaDTO dto)
        {
            var updated = await _seoMetaService.UpdateAsync(id, dto);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<SEOMetaDTO>> Delete(string id)
        {
            var deleted = await _seoMetaService.DeleteAsync(id);
            return Ok(deleted);
        }
    }
}
