using Microsoft.AspNetCore.Mvc;
using ProSolution.BL.DTOs.ContactMessages;
using ProSolution.BL.Services.Interfaces;

namespace ProSolution.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactUsController(IEmailService _emailService) : ControllerBase
    {
        [HttpPost("contact")]
        public async Task<IActionResult> SendContactMessage([FromBody] ContactMessageDto dto)
        {
            try
            {
                await _emailService.SendContactMessageAsync(dto);
                return Ok("Mesaj uğurla göndərildi.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
