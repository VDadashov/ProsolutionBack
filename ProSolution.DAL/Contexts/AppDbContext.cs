using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProSolution.Core.Entities;
using ProSolution.Core.Entities.Commons;
using ProSolution.Core.Entities.Identity;
using System.Security.Claims;

namespace ProSolution.DAL.Contexts
{
    public class AppDbContext : IdentityDbContext<User>
    {
        private IHttpContextAccessor _http;
        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor http) : base(options)
        {
            _http = http;
        }


        public DbSet<Badge> Badges  { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogReview> BlogReviews { get; set; }
        public DbSet<BlogReviewReply> BlogReviewReplies { get; set; }
        public DbSet<Brand> Brands  { get; set; }
        public DbSet<Category> Catagories { get; set; }
        public DbSet<CategoryProduct> CategoryProducts { get; set; }
        public DbSet<OurService> OurServices { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<SeoData> SeoDatas { get; set; }
        public DbSet<SeoMeta> SeoMetas { get; set; }
        public DbSet<SeoUrl> SeoUrls { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }
        public DbSet<FeatureOption> FeatureOptions { get; set; }
        public DbSet<FeatureOptionItem> FeatureOptionItems { get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; }
        public DbSet<ProductSlug> ProductSlugs { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entities = ChangeTracker.Entries<BaseEntity>();
            string? name = _http?.HttpContext?.User?.Identity?.IsAuthenticated == true
                          ? _http.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value : "System";

            foreach (var data in entities)
            {
                switch (data.State)
                {
                    case EntityState.Added:
                        data.Entity.CreatedAt = DateTime.Now;
                        data.Entity.CreatedBy = name;
                        break;
                    case EntityState.Modified:
                        data.Entity.UpdatedAt = DateTime.Now;
                        data.Entity.CreatedBy = name;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }

}
