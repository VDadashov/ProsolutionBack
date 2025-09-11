
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProSolution.Core.Entities;

namespace ProSolution.DAL.Configurations
{
    internal class ProductDbConf : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(p => p.Price)
                .HasPrecision(18, 4)
                .IsRequired();

            builder.HasMany(s=>s.ProductSlugs)
                   .WithOne(s=>s.Product)
                   .HasForeignKey(s=>s.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Brand)
                    .WithMany(b => b.Products)
                    .HasForeignKey(p => p.BrandId)
                    .OnDelete(DeleteBehavior.Restrict);

            // Catagory связь
            builder.HasMany(p => p.CategoryProducts)
                   .WithOne(cp => cp.Product)
                   .HasForeignKey(cp => cp.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            // ProductImage связь
            builder.HasMany(p => p.Images)
                   .WithOne(i => i.Product)
                   .HasForeignKey(i => i.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            // ProductFeature связь
            builder.HasMany(p => p.ProductFeatures)
                   .WithOne(k => k.Product)
                   .HasForeignKey(k => k.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            // ProductFeature связь
            builder.HasMany(p => p.BasketItems)
                   .WithOne(k => k.Product)
                   .HasForeignKey(k => k.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}


