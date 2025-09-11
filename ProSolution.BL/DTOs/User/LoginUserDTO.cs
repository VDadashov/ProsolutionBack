namespace ProSolution.BL.DTOs.User
{
    public record class LoginUserDTO
    {
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }
    }
}
