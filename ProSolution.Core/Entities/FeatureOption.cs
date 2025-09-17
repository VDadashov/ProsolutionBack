using System.ComponentModel.DataAnnotations;

namespace ProSolution.Core.Entities
{
    // public class FeatureOption : BaseEntity
    // {
    //     public string Name { get; set; }
    //     public string? Slug { get; set; }
    //     public ICollection<FeatureOptionItem>? FeatureOptionItems { get; set; }
    //
    //     public ICollection<ProductFeatureKeys>? ProductFeatureKeys { get; set; } = new List<ProductFeatureKeys>();
    // }
    
    public class FeatureOption : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;
        public string? Slug { get; set; }
        public int? Index { get; set; }
        public string? ParentId { get; set; }
        public FeatureOption? Parent { get; set; }
        public ICollection<FeatureOption>? Children { get; set; }
        public ICollection<FeatureOptionItem>? FeatureOptionItems { get; set; }
        public ICollection<ProductFeatureKeys>? ProductFeatureKeys { get; set; } = new List<ProductFeatureKeys>();
    }
}
