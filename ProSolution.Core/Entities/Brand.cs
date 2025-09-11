namespace ProSolution.Core.Entities
{
    public class Brand : BaseEntity
    {

        public string? ImagePath { get; set; }
   

        public string? Title { get; set; }
        public string? Slug { get; set; }

        public string? Description { get; set; }

        public ICollection<Product>? Products { get; set; }

    }
}
