using ProSolution.BL.DTOs.Characteristics;

namespace ProSolution.BL.DTOs;

public record ProductUpdateDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public double? Price { get; set; }
    public bool? InStock { get; set; } 
    public DateTime? DiscountEndDate { get; set; }
    public DateTime? DiscountStartDate { get; set; }
    public double? DiscountPrice { get; set; }

    public int SoldCount { get; set; }

    public string? BrandId { get; set; }

    public ICollection<string>? CategoryItemIds { get; set; }

    public ICollection<ProductImageCreateDto>? Images { get; set; }

 

    public List<string>? FeatureOptionItemIds { get; set; }

}
