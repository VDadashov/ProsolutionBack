namespace ProSolution.BL.DTOs.Commons;

public abstract record ReviewDTO : BaseEntityDTO
{
    public string Text { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public bool Checked { get; set; } = false;
}
