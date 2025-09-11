using ProSolution.BL.DTOs.Token;
using ProSolution.Core.Entities.Identity;
using System.Security.Claims;

namespace ProSolution.BL.Services.Interfaces
{
    public interface ITokenHandler
    {
        TokenResponseDto CreateJwt(User user, ICollection<Claim> claims, int minutes);
    }
}
