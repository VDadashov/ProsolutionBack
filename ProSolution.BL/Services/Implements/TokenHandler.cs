using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProSolution.BL.DTOs.Token;
using ProSolution.BL.Services.Interfaces;
using ProSolution.Core.Entities.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProSolution.BL.Services.Implements
{
    public class TokenHandler : ITokenHandler
    {
        private readonly IConfiguration _configuration;

        public TokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TokenResponseDto CreateJwt(User user, ICollection<Claim> claims, int minutes)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecurityKey"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.Now.AddMinutes(minutes),
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return new TokenResponseDto(tokenString, tokenOptions.ValidTo, user.Name, CreateRefreshToken(), tokenOptions.ValidTo.AddMinutes(minutes / 4));
        }

        public string CreateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }
    }

    
}
