using Microsoft.AspNetCore.Mvc;
using ProSolution.BL.DTOs.ProductFeatureKeys;
using ProSolution.BL.Services.Interfaces;

[Route("api/[controller]")]
[ApiController]
public class ProductFeatureKeysController : ControllerBase
{
    private readonly IProductFeatureKeysService _service;

    public ProductFeatureKeysController(IProductFeatureKeysService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id) =>
        Ok(await _service.GetByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductFeatureKeysCreateDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ProductFeatureKeysUpdateDto dto) =>
        Ok(await _service.UpdateAsync(dto));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
