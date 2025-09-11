using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProSolution.Core.Entities;

namespace ProSolution.DAL.Configurations
{
    internal class SeoUrlDbConf : IEntityTypeConfiguration<SeoUrl>
    {
        public void Configure(EntityTypeBuilder<SeoUrl> builder)
        {
            builder.HasKey(su => su.Id);

            builder.Property(su => su.Url)
                .HasMaxLength(500);

            builder.Property(su => su.RedirectUrl)
                .HasMaxLength(500);
        }
    }
}
