using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProSolution.Core.Entities;

namespace ProSolution.DAL.Configurations
{
    internal class PartnerDbConf : IEntityTypeConfiguration<Partner>
    {
        public void Configure(EntityTypeBuilder<Partner> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.ImagePath)
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}
