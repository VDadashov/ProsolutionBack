using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProSolution.Core.Entities;

namespace ProSolution.DAL.Configurations
{
    internal class SeoDataDbConf : IEntityTypeConfiguration<SeoData>
    {
        public void Configure(EntityTypeBuilder<SeoData> builder)
        {
            builder.HasKey(sd => sd.Id);

            builder.Property(sd => sd.AltText)
                .HasMaxLength(255);

            builder.Property(sd => sd.AnchorText)
                .HasMaxLength(255);
        }
    }
}
