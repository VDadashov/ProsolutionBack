namespace ProSolution.BL.DTOs;

public record CategoryUpdateDto
{
    public string Title { get; set; }
    public int? Index { get; set; }
}
