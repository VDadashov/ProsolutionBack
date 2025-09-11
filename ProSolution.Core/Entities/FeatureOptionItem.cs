namespace ProSolution.Core.Entities
{
    public class FeatureOptionItem : BaseEntity
    {
        public string Name { get; set; }
        public string? Slug { get; set; }

        public string? FeatureOptionId { get; set; }
        public FeatureOption? FeatureOption { get; set; }

        public string? ParentId { get; set; }
        public FeatureOptionItem? Parent { get; set; }
        public ICollection<FeatureOptionItem>? Children { get; set; }

        public ICollection<ProductFeature>? ProductFeatures { get; set; }
    }
}
