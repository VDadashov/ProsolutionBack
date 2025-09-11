using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProSolution.Core.Entities;

namespace ProSolution.DAL.Configurations
{
    internal class SliderDbConf : IEntityTypeConfiguration<Slider>
    {
        public void Configure(EntityTypeBuilder<Slider> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.ImagePath)
                .IsRequired();

            builder.Property(s => s.AltText)
                .IsRequired()
                .HasMaxLength(500);
        }
    }
}
