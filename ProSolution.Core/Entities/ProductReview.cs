namespace ProSolution.Core.Entities
{
    public class ProductReview : Review
    {
        public string Rating { get; set; } = null!;

        public string ProductId { get; set; }
        public Product Product { get; set; }
    }
}
