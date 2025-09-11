using ProSolution.BL.DTOs.Commons;
using ProSolution.BL.DTOs.FeatureOPtions;
using System.ComponentModel.DataAnnotations;

namespace ProSolution.BL.DTOs.Characteristics
{
    public record FeatureOptionCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;
   

    }

    public record FeatureOptionUpdateDto
    {
        public string? Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;
    }


    public record FeatureOptionGetDto : BaseEntityDTO
    {
       
        public string Name { get; set; } = null!;
        public string? Slug { get; set; } = null!;

        public ICollection<FeatureOptionItemIncludeDto>? FeatureOptionItems { get; set; }  
    }
    public record FeatureOptionIncludeDto : BaseEntityDTO
    {
        public string Name { get; set; } = null!;
        public string? Slug { get; set; } = null!;
    }

    public record FeatureOptionIncludeProductDto
    {
        public string? Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Slug { get; set; } = null!;
    }

}
