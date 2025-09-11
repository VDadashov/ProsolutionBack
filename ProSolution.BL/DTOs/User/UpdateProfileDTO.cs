namespace ProSolution.BL.DTOs.User
{
    public class UpdateProfileDTO
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;

        public string? CurrentPassword { get; set; } 
        public string? NewPassword { get; set; }     
    }
}
