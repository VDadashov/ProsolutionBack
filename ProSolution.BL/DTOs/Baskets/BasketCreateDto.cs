namespace ProSolution.BL.DTOs;

public record BasketCreateDto
{
    public ICollection<BasketItemCreateDto>? BasketItems { get; set; }
}
