namespace ProSolution.Core.Entities
{
    public class ProductFeature : BaseEntity
    {
        public string ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public string FeatureOptionItemId { get; set; }
        public FeatureOptionItem FeatureOptionItem { get; set; } = null!;
    }
}
