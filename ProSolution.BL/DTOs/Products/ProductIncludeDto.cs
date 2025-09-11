using ProSolution.BL.DTOs.Commons;
using ProSolution.Core.Entities;

namespace ProSolution.BL.DTOs;

public record ProductIncludeDto : BaseEntityDTO
{
    public string Title { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public int SoldCount { get; set; }
    public string? DetailSlug { get; set; }
    public ICollection<ProductImageGetDto>? Images { get; set; }
    public bool? InStock { get; set; } 
    public DateTime DiscountStartDate { get; set; }
    public DateTime DiscountEndDate { get; set; }
    public double DiscountPrice { get; set; }

}
