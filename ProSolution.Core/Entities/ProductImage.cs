namespace ProSolution.Core.Entities
{
    public class ProductImage : BaseEntity
    {
        public string ImagePath { get; set; } 
        public string AltText { get; set; }
        public bool IsMain { get; set; }


        public string ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
