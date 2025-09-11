using Microsoft.EntityFrameworkCore;
using ProSolution.Core.Entities;
using ProSolution.Core.Enums;
using ProSolution.Core.Repositories;
using ProSolution.DAL.Contexts;
using ProSolution.DAL.Repositories.Common;
using System.Linq;
using System.Linq.Expressions;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    private readonly DbSet<ProductReview> _productReviewsRepository;
    private readonly DbSet<Product> _productRepository;
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context) : base(context)
    {
        _productReviewsRepository = context.Set<ProductReview>();
        _productRepository = context.Set<Product>();
        _context = context;
    }

    public async Task AddProductReview(ProductReview productReview)
    {
        await _productReviewsRepository.AddAsync(productReview);
    }
    public async Task DeleteProductReview(string id)
    {
        await _productReviewsRepository.Where(x => x.Id == id).ExecuteDeleteAsync();

    }

    public IQueryable<ProductReview> GetAllWhereProductReview(Expression<Func<ProductReview, bool>>? expression = null, bool IsTracking = true, params string[] includes)
    {
        var query = _productReviewsRepository.AsQueryable();
        if (expression != null) query = query.Where(expression);

        query = _addIncludes(query, includes);

        return IsTracking ? query : query.AsNoTracking();
    }

    public async Task<double> GetRatingAverageAsync(string productId)
    {
        var ratings = await _productReviewsRepository
            .Where(pr => pr.ProductId == productId)
            .Select(pr => pr.Rating)
            .ToListAsync();

        var numericRatings = ratings
            .Select(r =>
            {
                if (Enum.TryParse<RatingEnum>(r, out var ratingEnum))
                    return (int)ratingEnum;
                return 0; // Əgər parse alınmazsa 0 qiymət verilir
            })
            .Where(val => val > 0); // 0-ları çıxırıq, yəni düzgün dəyərləri saxlayırıq

        return numericRatings.Any() ? numericRatings.Average() : 0;
    }

    public async Task<double?> MinPriceAsync(string? search, string? categorySlug, bool isDeleted)
    {
        var now = DateTime.Now;

        var query = _productRepository.Where(x => x.IsDeleted == isDeleted);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(x => x.Title.Contains(search));

        if (!string.IsNullOrWhiteSpace(categorySlug))
            query = query.Where(x => x.ProductSlugs!.Any(cp =>
                cp.Slug.Trim().ToLower().Contains(categorySlug.Trim().ToLower())));

        return await query
            .Select(x => new
            {
                FinalPrice = x.DiscountPrice > 0 && x.DiscountStartDate <= now && x.DiscountEndDate >= now
                    ? x.DiscountPrice
                    : x.Price,
                HasDiscount = x.DiscountPrice > 0 && x.DiscountStartDate <= now && x.DiscountEndDate >= now
            })
            .OrderBy(x => x.FinalPrice)
            .ThenByDescending(x => x.HasDiscount)
            .Select(x => (double?)x.FinalPrice)
            .FirstOrDefaultAsync();
    }
    public async Task<double?> MaxPriceAsync(string? search, string? categorySlug, bool isDeleted)
    {
        var now = DateTime.Now;

        var query = _productRepository.Where(x => x.IsDeleted == isDeleted);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(x => x.Title.Contains(search));

        if (!string.IsNullOrWhiteSpace(categorySlug))
            query = query.Where(x => x.ProductSlugs!.Any(cp =>
                cp.Slug.Trim().ToLower().Contains(categorySlug.Trim().ToLower())));

        return await query
            .Select(x => new
            {
                FinalPrice = x.DiscountPrice > 0 && x.DiscountStartDate <= now && x.DiscountEndDate >= now
                    ? x.DiscountPrice
                    : x.Price,
                HasDiscount = x.DiscountPrice > 0 && x.DiscountStartDate <= now && x.DiscountEndDate >= now
            })
            .OrderByDescending(x => x.FinalPrice)
            .ThenByDescending(x => x.HasDiscount)
            .Select(x => (double?)x.FinalPrice)
            .FirstOrDefaultAsync();
    }


    private IQueryable<ProductReview> _addIncludes(IQueryable<ProductReview> query, params string[] includes)
    {
        if (includes != null)
        {
            for (int i = 0; i < includes.Length; i++)
            {
                query = query.Include(includes[i]);
            }
        }
        return query;
    }










    public async Task AddProductFeatureAsync(ProductFeature feature)
    {
        await _context.Set<ProductFeature>().AddAsync(feature);
    }

    public async Task AddCategoryProductAsync(CategoryProduct categoryProduct)
    {
        await _context.Set<CategoryProduct>().AddAsync(categoryProduct);
    }

    public async Task AddProductSlugAsync(ProductSlug slug)
    {
        await _context.Set<ProductSlug>().AddAsync(slug);
    }

    public async Task<List<ProductFeature>> GetProductFeaturesAsync(string productId)
    {
        return await _context.Set<ProductFeature>()
            .Where(x => x.ProductId == productId)
            .ToListAsync();
    }

    public async Task RemoveProductFeaturesAsync(List<ProductFeature> features)
    {
        _context.RemoveRange(features);
    }

    public async Task RemoveProductFeatureAsync(ProductFeature feature)
    {
        _context.Remove(feature);
    }

    public async Task<List<ProductSlug>> GetProductSlugsAsync(string productId)
    {
        return await _context.Set<ProductSlug>()
            .Where(x => x.ProductId == productId)
            .ToListAsync();
    }

    public async Task RemoveProductSlugsAsync(List<ProductSlug> slugs)
    {
        _context.RemoveRange(slugs);
    }

    public async Task RemoveProductImageAsync(ProductImage image)
    {
        _context.Remove(image);
    }

    public async Task AddProductImageAsync(ProductImage image)
    {
        await _context.Set<ProductImage>().AddAsync(image);
    }

    public async Task AddCategoryProductsAsync(IEnumerable<CategoryProduct> categoryProducts)
    {
        await _context.Set<CategoryProduct>().AddRangeAsync(categoryProducts);
    }

    public async Task AddProductSlugsAsync(IEnumerable<ProductSlug> slugs)
    {
        await _context.Set<ProductSlug>().AddRangeAsync(slugs);
    }

    public async Task RemoveCategoryProductsAsync(IEnumerable<CategoryProduct> categoryProducts)
    {
        _context.Set<CategoryProduct>().RemoveRange(categoryProducts);
    }

    public async Task RemoveProductImagesAsync(IEnumerable<ProductImage> images)
    {
        _context.RemoveRange(images);
    }


}
