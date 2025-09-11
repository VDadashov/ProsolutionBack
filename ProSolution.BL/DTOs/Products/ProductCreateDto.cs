using ProSolution.BL.DTOs.Characteristics;
using ProSolution.Core.Entities;

namespace ProSolution.BL.DTOs;

public record ProductCreateDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int SoldCount { get; set; }
    public bool? InStock { get; set; } = true;
    public DateTime? DiscountStartDate { get; set; }
    public DateTime? DiscountEndDate { get; set; }
    public double? DiscountPrice { get; set; }
   
    public string BrandId { get; set; }

    public ICollection<string> CategoryItemIds { get; set; }
    public ICollection<ProductImageCreateDto>? Images { get; set; }
   

    public List<string>? FeatureOptionItemIds { get; set; }


}
