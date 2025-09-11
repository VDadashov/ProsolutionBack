using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProSolution.Core.Entities;

namespace ProSolution.DAL.Configurations
{
    internal class FeatureOptConf : IEntityTypeConfiguration<FeatureOption>
    {
        public void Configure(EntityTypeBuilder<FeatureOption> builder)
        {
            builder.Property(f => f.Name)
                    .IsRequired()
                    .HasMaxLength(100);

            builder.HasMany(f => f.FeatureOptionItems)
                   .WithOne(i => i.FeatureOption)
                   .HasForeignKey(i => i.FeatureOptionId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
    
    
}
