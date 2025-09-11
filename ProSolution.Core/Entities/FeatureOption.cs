namespace ProSolution.Core.Entities
{
    public class FeatureOption : BaseEntity
    {
        public string Name { get; set; }
        public string? Slug { get; set; }
        public ICollection<FeatureOptionItem>? FeatureOptionItems { get; set; }

        public ICollection<ProductFeatureKeys>? ProductFeatureKeys { get; set; } = new List<ProductFeatureKeys>();
    }
}
