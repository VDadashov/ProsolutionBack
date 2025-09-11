using Microsoft.AspNetCore.Mvc;
using ProSolution.BL.DTOs.User;
using ProSolution.BL.Services.Interfaces;
using ProSolution.Core.Repositories;
using System.Security.Claims;

namespace ProSolution.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
       
        public AuthController(IAuthService authService)
        {
            _authService = authService;
     

        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO dto)
        {
            await _authService.RegisterAsync(dto);
            return Ok($" User name  and password send to {dto.Email}");
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO dto)
        {
            return Ok(await _authService.LoginAsync(dto));

        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateProfileDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            await _authService.UpdateProfileAsync(userId, dto);
            return NoContent();
        }
        [HttpPut("[action]/{userId}")]
        public async Task<IActionResult> UpdateByAdmin(string userId,[FromBody] UpdateProfileDTO dto)
        {
           
            if (userId == null) return Unauthorized();

            await _authService.UpdateProfileAsync(userId, dto);
            return NoContent();
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO dto)
        {
            await _authService.ForgotPasswordAsync(dto.Email);
            return Ok("Password reset email sent.");
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            await _authService.ResetPasswordAsync(dto);
            return Ok("Password has been reset successfully.");
        }
        [HttpGet("by-id/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _authService.GetUserByIdAsync(id);
            return Ok(user);
        }

        [HttpGet("by-username/{username}")]
        public async Task<IActionResult> GetUserByUserName(string username)
        {
            var user = await _authService.GetUserByUserNameAsync(username);
            return Ok(user);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers([FromQuery] string? search, [FromQuery] bool isActivate)
        {
            var users = await _authService.GetAllUsersAsync(search, isActivate);
            return Ok(users);
        }

        [HttpGet("filtered")]
        public async Task<IActionResult> GetAllUsersFiltered(
            [FromQuery] string? search,
            [FromQuery] int take = 10,
            [FromQuery] int page = 1,
            [FromQuery] int order = 0,
            [FromQuery] bool isActivate = true)
        {
            var users = await _authService.GetAllUsersFilteredAsync(search, take, page, order, isActivate);
            return Ok(users);
        }

        [HttpPost("change-role")]
        public async Task<IActionResult> ChangeRole([FromBody] ChangeRoleDto dto)
        {
            var result = await _authService.ChangeUserRoleAsync(dto);
            return Ok(result);
        }

        [HttpGet("get-role/{userId}")]
        public async Task<IActionResult> GetUserRole(string userId)
        {
            var role = await _authService.GetUserRoleAsync(userId);
            return Ok(role);
        }

        [HttpGet("is-authenticated")]
        public async Task<IActionResult> IsAuthenticated()
        {
            var result = await _authService.IsAuthenticatedAsync(User);
            return Ok(result);
        }
      


    }
}
