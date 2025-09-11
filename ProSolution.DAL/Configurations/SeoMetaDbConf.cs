using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProSolution.Core.Entities;

namespace ProSolution.DAL.Configurations
{
    internal class SeoMetaDbConf : IEntityTypeConfiguration<SeoMeta>
    {
        public void Configure(EntityTypeBuilder<SeoMeta> builder)
        {
            builder.HasKey(sm => sm.Id);

            builder.Property(sm => sm.MetaDescription)
                .HasMaxLength(1000);

            builder.Property(sm => sm.MetaTitle)
                .HasMaxLength(255);

            builder.Property(sm => sm.MetaTags)
                .HasMaxLength(500);
        }
    }
}
