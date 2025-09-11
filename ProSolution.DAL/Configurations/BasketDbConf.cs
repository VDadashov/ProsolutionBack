using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProSolution.Core.Entities;

namespace ProSolution.DAL.Configurations;

internal class BasketDbConf : IEntityTypeConfiguration<Basket>
{
    public void Configure(EntityTypeBuilder<Basket> builder)
    {
        builder.HasMany(p => p.BasketItems)
               .WithOne(k => k.Basket)
               .HasForeignKey(k => k.BasketId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
