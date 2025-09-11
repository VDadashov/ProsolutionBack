namespace ProSolution.Core.Entities;

public class BasketItem : BaseEntity
{
    public int Count { get; set; }

    public string BasketId { get; set; } = null!;
    public Basket Basket { get; set; } 

    public string ProductId { get; set; } = null!;
    public Product Product { get; set; } 
}
