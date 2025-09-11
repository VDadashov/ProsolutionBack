namespace ProSolution.BL.DTOs;

public record BasketUpdateDto
{
    public ICollection<string>? BasketItemIds { get; set; }
    public ICollection<BasketItemCreateDto>? BasketItemNews { get; set; }
}
