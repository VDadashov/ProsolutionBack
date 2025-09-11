using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProSolution.BL.Services.Interfaces;
using static ProSolution.BL.DTOs.User.UserAddressDto;

namespace ProSolution.API.Controllers
{
  
    [ApiController]
    [Route("api/[controller]")]
    public class UserAddressController : ControllerBase
    {
        private readonly IUserAddressService _service;

        public UserAddressController(IUserAddressService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id) => Ok(await _service.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create(UserAddressCreateDTO dto)
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, UserAddressUpdateDTO dto)
        {
            dto.Id = id;
            return Ok(await _service.UpdateAsync(dto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }

}
