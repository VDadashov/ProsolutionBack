namespace ProSolution.Core.Entities;

public class Basket : BaseEntity
{
    public string? Token { get; set; } = Guid.NewGuid().ToString();
    public bool IsVerified { get; set; } = false; 

    public ICollection<BasketItem>? BasketItems{ get; set; }
}
