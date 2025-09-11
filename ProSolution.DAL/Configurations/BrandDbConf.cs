using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProSolution.Core.Entities;

namespace ProSolution.DAL.Configurations
{
    internal class BrandDbConf : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ImagePath)
                .HasMaxLength(300)    // Ограничение длины для URL картинки
                .IsRequired(false);   // Поле может быть пустым (nullable)

            builder.Property(x => x.Title)
                .HasMaxLength(255)
                .IsRequired(false);    // Можно сделать обязательным, если хочешь

            builder.Property(x => x.Description)
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.HasMany(x => x.Products)
                   .WithOne(p => p.Brand) // или p.Category — зависит от связи
                   .HasForeignKey(p => p.BrandId) // или p.CategoryId
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
