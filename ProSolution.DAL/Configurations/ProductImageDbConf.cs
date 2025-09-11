using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProSolution.Core.Entities;

namespace ProSolution.DAL.Configurations
{
    internal class ProductImageDbConf : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.HasKey(pi => pi.Id);

            builder.Property(pi => pi.ImagePath)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(pi => pi.AltText)
                .HasMaxLength(255);

            builder.Property(pi => pi.IsMain)
                .IsRequired();

            builder.HasOne(pi => pi.Product)
                .WithMany(p => p.Images)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
