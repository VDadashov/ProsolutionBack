using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProSolution.Core.Entities;

namespace ProSolution.DAL.Configurations
{
    internal class OurServiceDbConf : IEntityTypeConfiguration<OurService>
    {
        public void Configure(EntityTypeBuilder<OurService> builder)
        {
            builder.HasKey(os => os.Id);

            builder.Property(os => os.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(os => os.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(os => os.ImagePath)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(os => os.ContentTitle)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(os => os.ContentDescription)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(os => os.ContentPath)
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}
