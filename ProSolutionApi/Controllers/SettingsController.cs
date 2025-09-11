using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProSolution.BL.DTOs;
using ProSolution.BL.Exceptions.Common;
using ProSolution.BL.Services.Interfaces;

namespace ProSolution.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingService _settingService;

        public SettingsController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] SettingUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new NegativIdException("ID bos ola bilmez");

            await _settingService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new NegativIdException("ID bos ola bilmez");

            var setting = await _settingService.GetByIdAsync(id);
            return Ok(setting);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var settings = await _settingService.GetAllAsync();
            return Ok(settings);
        }

        [HttpGet("filtered")]
        public async Task<IActionResult> GetAllFiltered([FromQuery] string? search, [FromQuery] int take = 10, [FromQuery] int page = 1, [FromQuery] int order = 1, [FromQuery] bool isDeleted = false)
        {
            var result = await _settingService.GetAllFilteredAsync(search, take, page, order, isDeleted);
            return Ok(result);
        }
    }
}
