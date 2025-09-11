using ProSolution.Core.Entities;
using ProSolution.Core.Repositories.Common;
using System.Linq.Expressions;

namespace ProSolution.Core.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task AddProductReview(ProductReview productReview);
       
        Task DeleteProductReview(string id);

        Task<double> GetRatingAverageAsync(string productId);

        IQueryable<ProductReview> GetAllWhereProductReview(Expression<Func<ProductReview, bool>>? expression = null, bool IsTracking = true, params string[] includes);

        Task<double?> MinPriceAsync(string? search, string? categorySlug, bool isDeleted);

        Task<double?> MaxPriceAsync(string? search, string? categorySlug, bool isDeleted);

        Task AddProductFeatureAsync(ProductFeature feature);
        Task AddCategoryProductAsync(CategoryProduct categoryProduct);
        Task AddProductSlugAsync(ProductSlug slug);
        Task<List<ProductFeature>> GetProductFeaturesAsync(string productId);
        Task RemoveProductFeaturesAsync(List<ProductFeature> features);
        Task RemoveProductFeatureAsync(ProductFeature feature);
        Task<List<ProductSlug>> GetProductSlugsAsync(string productId);
        Task RemoveProductSlugsAsync(List<ProductSlug> slugs);
        Task RemoveProductImageAsync(ProductImage image);
        Task AddProductImageAsync(ProductImage image);
        Task AddCategoryProductsAsync(IEnumerable<CategoryProduct> categoryProducts);
        Task AddProductSlugsAsync(IEnumerable<ProductSlug> slugs);
        Task RemoveCategoryProductsAsync(IEnumerable<CategoryProduct> categoryProducts);
        Task RemoveProductImagesAsync(IEnumerable<ProductImage> images);


    }
}
