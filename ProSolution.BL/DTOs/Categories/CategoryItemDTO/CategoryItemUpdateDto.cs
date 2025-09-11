namespace ProSolution.BL.DTOs.Categories.CategoryItemDTO;

public record CategoryItemUpdateDto
{
    public string Title { get; set; }
    public string? ParentId { get; set; }
    public int? Index { get; set; }
}
