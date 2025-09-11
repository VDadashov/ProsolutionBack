namespace ProSolution.Core.Entities
{
    public class ProductSlug : BaseEntity
    {
        public string Slug { get; set; }

        public string ProductId { get; set; }
        public Product Product { get; set; }
    }
}
