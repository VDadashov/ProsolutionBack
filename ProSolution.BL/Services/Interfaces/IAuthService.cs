using ProSolution.BL.DTOs;
using ProSolution.BL.DTOs.Token;
using ProSolution.BL.DTOs.User;
using ProSolution.Core.Entities;
using ProSolution.Core.Entities.Commons;
using System.Security.Claims;

namespace ProSolution.BL.Services.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterUserDTO dto);
        Task<TokenResponseDto> LoginAsync(LoginUserDTO dto);
        Task<AppUserGetDto> IsAuthenticatedAsync(ClaimsPrincipal user);
        string? GetUserId(ClaimsPrincipal user);
        Task UpdateProfileAsync(string userId, UpdateProfileDTO dto);
        Task ForgotPasswordAsync(string email);
        Task ResetPasswordAsync(ResetPasswordDTO dto);
       
        Task<AppUserGetDto> GetUserByIdAsync(string id);
        Task<AppUserGetDto> GetUserByUserNameAsync(string userName);
        Task<ICollection<AppUserGetDto>> GetAllUsersAsync(string? search, bool isActivate);
        Task<PaginationDto<AppUserGetDto>> GetAllUsersFilteredAsync(string? search, int take, int page, int order, bool isActivate);
        Task<string> ChangeUserRoleAsync(ChangeRoleDto dto);
        Task<string> GetUserRoleAsync(string userId);

        
    }
}
