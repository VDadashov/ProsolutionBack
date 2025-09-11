using ProSolution.BL.DTOs.Characteristics;
using ProSolution.BL.DTOs.Commons;

namespace ProSolution.BL.DTOs.FeatureOPtions
{
    public record FeatureOptionItemCreateDto
    {
        public string Name { get; set; }
        public string? FeatureOptionId { get; set; }
        public string? ParentId { get; set; }  
    }

    public record FeatureOptionItemUpdateDto 
    {
     public string Id { get; set; }
        public string Name { get; set; }
        public string? FeatureOptionId { get; set; }
        public string? ParentId { get; set; }  
    }

    public record FeatureOptionItemDto : BaseEntityDTO
    {
        
        public string Name { get; set; }
    }

    
    

    public record FeatureOptionItemGetDto : BaseEntityDTO
    {
        public string Name { get; set; }
        public string FeatureOptionId { get; set; }
        public string? Slug { get; set; } = null!;
        public string? ParentId { get; set; }
        public FeatureOptionItemIncludeDto? Parent { get; set; }
    }

    public record FeatureOptionItemIncludeDto : BaseEntityDTO
    {
        public string Name { get; set; } = null!;
        public string? Slug { get; set; } = null!;

        public string? FeatureOptionId { get; set; }
        public FeatureOptionIncludeDto? FeatureOption { get; set; }
        public string? ParentId { get; set; }
        public FeatureOptionIncludeOptionDto? Parent { get; set; }
        public ICollection<FeatureOptionItemIncludeDto>? Children { get; set; }
    }

    public record FeatureOptionItemIncludeProductDto
    {
        public string? Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Slug { get; set; } = null!;

        public string? FeatureOptionId { get; set; }
        public FeatureOptionIncludeProductDto? FeatureOption { get; set; }
        public string? ParentId { get; set; }
        public FeatureOptionIncludeOptionProductDto? Parent { get; set; }
        public ICollection<FeatureOptionItemIncludeProductDto>? Children { get; set; }
    }

    public record FeatureOptionIncludeOptionDto : BaseEntityDTO
    {
        public string? Name { get; set; }
        public string? Slug { get; set; } = null!;
        public string? FeatureOptionId { get; set; }
        public FeatureOptionIncludeDto? FeatureOption { get; set; }
    }

    public record FeatureOptionIncludeOptionProductDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; } = null!;
        public string? FeatureOptionId { get; set; }
        public FeatureOptionIncludeProductDto? FeatureOption { get; set; }
    }

    public record  FeatureOptionItemIncludeWithProductDto : FeatureOptionItemIncludeDto
    {
       
        public ICollection<ProductIncludeDto>? Products { get; set; }
       
    }

}
