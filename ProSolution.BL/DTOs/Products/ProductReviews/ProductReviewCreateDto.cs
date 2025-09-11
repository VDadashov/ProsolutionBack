namespace ProSolution.BL.DTOs;

public record ProductReviewCreateDto 
{
    public string Text { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Rating { get; set; } = null!;

    public string ProductId { get; set; }
}
