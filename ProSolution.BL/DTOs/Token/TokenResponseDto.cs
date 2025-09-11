namespace ProSolution.BL.DTOs.Token
{
    public record TokenResponseDto(string Token, DateTime ExpireTime, string UserName, string RefreshToken, DateTime RefreshTokenExpireAt);
}
