using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProSolution.Core.Entities;

namespace ProSolution.DAL.Configurations
{
    public class ProfuctFeatureKeysConf : IEntityTypeConfiguration<ProductFeatureKeys>
    {




        public void Configure(EntityTypeBuilder<ProductFeatureKeys> builder)
        {
            builder.HasKey(x => x.Id);

            // Связь один ко многим: Category → ProductFeatureKeys
            builder.HasOne(pfk => pfk.Category)
                .WithMany(c => c.ProductFeatureKeys)
                .HasForeignKey(pfk => pfk.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь многие ко многим: ProductFeatureKeys ↔ FeatureOption
            builder
                .HasMany(pfk => pfk.FeatureOptions)
                .WithMany(fo => fo.ProductFeatureKeys)
                .UsingEntity<Dictionary<string, object>>(
                    "CategoryFeatureOptions", // имя связующей таблицы
                    j => j
                        .HasOne<FeatureOption>()
                        .WithMany()
                        .HasForeignKey("FeatureOptionId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<ProductFeatureKeys>()
                        .WithMany()
                        .HasForeignKey("ProductFeatureKeysId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.HasKey("ProductFeatureKeysId", "FeatureOptionId");
                    });
        }
    }
}