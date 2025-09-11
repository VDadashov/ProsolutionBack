using ProSolution.BL.DTOs.Characteristics;
using ProSolution.BL.DTOs.Commons;
using ProSolution.BL.DTOs.FeatureOPtions;
using ProSolution.Core.Entities;

namespace ProSolution.BL.DTOs;

public record ProductGetDto : BaseEntityDTO
{
    public string Title { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public string? DetailSlug { get; set; }

    public int SoldCount { get; set; }
    public bool? InStock { get; set; } 
    public string BrandId { get; set; }
    public string? BrandName { get; set; } // если нужно выводить имя бренда
    public DateTime DiscountStartDate { get; set; }

    public DateTime DiscountEndDate { get; set; }

    public double DiscountPrice { get; set; }
    public bool? IsDeleted { get; set; }

    public double RatingAvarage { get; set; }

    public ICollection<ProductImageGetDto>? Images { get; set; }

    // Правильное отображение связи многие-ко-многим с Category

    public ICollection<ProductSlugGetDto>? ProductSlugs { get; set; }
    public ICollection<CategoryIncludeDto>? Categories { get; set; }
   
    public ICollection<FeatureOptionItemIncludeProductDto>? FeatureOptionItems { get; set; }

    public ICollection<ProductReviewIncludeDto>? ProductReviews { get; set; }
}

public record ProductImageGetDto : BaseEntityDTO
{
    public string ImagePath { get; set; }
    public string AltText { get; set; }
    public bool IsMain { get; set; }
}

public record ProductImageCreateDto
{
    public string ImagePath { get; set; }
    public string AltText { get; set; }
    public bool IsMain { get; set; }
}
