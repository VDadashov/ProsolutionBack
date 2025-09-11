using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProSolution.Core.Entities;

namespace ProSolution.DAL.Configurations
{
    internal class BadgeDbConf : IEntityTypeConfiguration<Badge>
    {
        public void Configure(EntityTypeBuilder<Badge> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Description)
                .HasMaxLength(500); // Можно ограничить длину текста (по желанию)

            builder.Property(b => b.ImageUrl)
                .HasMaxLength(500); // Тоже желательно ограничить

            builder.Property(b => b.IsSertificate)
                .IsRequired();
        }
    }
}
