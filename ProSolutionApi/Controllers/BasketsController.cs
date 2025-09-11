using Microsoft.AspNetCore.Mvc;
using ProSolution.BL.DTOs;
using ProSolution.BL.Services.Interfaces;

namespace ProSolution.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BasketsController : ControllerBase
{
    private readonly IBasketService _BasketService;

    public BasketsController(IBasketService BasketService)
    {
        _BasketService = BasketService;
    }

    [HttpPost("[Action]")]
    public async Task<IActionResult> Create([FromBody] BasketCreateDto dto) 
    {
        return Ok(await _BasketService.CreateAsync(dto));
    }

    [HttpPut("[Action]/{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] BasketUpdateDto dto)
    {
        return Ok(await _BasketService.UpdateAsync(id, dto));
    }

    [HttpDelete("[Action]/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _BasketService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPatch("softdelete/{id}")]
    public async Task<IActionResult> SoftDelete(string id)
    {
        await _BasketService.SoftDeleteAsync(id);
        return NoContent();
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _BasketService.GetAllAsync());
    }

    [HttpGet("filtered")]
    public async Task<IActionResult> GetAllFiltered([FromQuery] string? search, [FromQuery] int take = 10, [FromQuery] int page = 1, [FromQuery] int order = 1, [FromQuery] bool isDeleted = false)
    {
        return Ok(await _BasketService.GetAllFilteredAsync(search, take, page, order, isDeleted));
    }
    [HttpGet("[Action]/{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        return Ok(await _BasketService.GetByIdAsync(id));
    }
    [HttpGet("[Action]/{token}")]
    public async Task<IActionResult> GetByToken(string token)
    {
        return Ok(await _BasketService.GetByTokenAsync(token));
    }
}
