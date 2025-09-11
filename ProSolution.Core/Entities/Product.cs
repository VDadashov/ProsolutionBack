namespace ProSolution.Core.Entities
{
    public class Product : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int SoldCount { get; set; }
        public bool? InStock { get; set; } = true;
        public int? ViewCount { get; set; }
        public string? DetailSlug { get; set; }


        public DateTime? DiscountEndDate { get; set; }
        public DateTime? DiscountStartDate { get; set; }
        public double? DiscountPrice { get; set; }

        public double RatingAvarage { get; set; }

        public string BrandId { get; set; }
        public Brand Brand { get; set; }

        // [JsonIgnore]
        public ICollection<ProductReview>? ProductReviews { get; set; }

      //  [JsonIgnore]
        public ICollection<CategoryProduct>? CategoryProducts { get; set; }

      //  [JsonIgnore]
        public ICollection<ProductImage>? Images { get; set; }

      //  [JsonIgnore]
        public ICollection<ProductFeature>? ProductFeatures { get; set; }

    //    [JsonIgnore]
        public ICollection<ProductSlug>? ProductSlugs { get; set; }
        public ICollection<BasketItem>? BasketItems { get; set; }
    }
}
