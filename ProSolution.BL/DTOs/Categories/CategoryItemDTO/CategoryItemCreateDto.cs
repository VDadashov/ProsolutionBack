namespace ProSolution.BL.DTOs.Categories.CategoryItemDTO;

public record CategoryItemCreateDto
{
    public string Title { get; set; }
    public string? ParentId { get; set; }
    public int? Index { get; set; }
}