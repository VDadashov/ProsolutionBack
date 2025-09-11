namespace ProSolution.BL.DTOs;

public record BasketItemCreateDto 
{
    public int Count { get; set; }

    public string ProductId { get; set; } = null!;
}
