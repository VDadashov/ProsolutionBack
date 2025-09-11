using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProSolution.Core.Entities;

namespace ProSolution.DAL.Configurations
{
    internal class FeatureOptItemConf : IEntityTypeConfiguration<FeatureOptionItem>
    {
        public void Configure(EntityTypeBuilder<FeatureOptionItem> builder)
        {
            builder.Property(f => f.Name)
                    .IsRequired()
                    .HasMaxLength(100);

            builder.HasOne(x => x.FeatureOption)
                   .WithMany(x => x.FeatureOptionItems) 
                   .HasForeignKey(x => x.FeatureOptionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(f => f.ProductFeatures)
                   .WithOne(i => i.FeatureOptionItem)
                   .HasForeignKey(i => i.FeatureOptionItemId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ci => ci.Parent)
                   .WithMany(ci => ci.Children)
                   .HasForeignKey(ci => ci.ParentId)
                   .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
