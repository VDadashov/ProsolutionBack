using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProSolution.Core.Entities.Identity;

namespace ProSolution.DAL.Configurations
{
    internal class UserConfigurationv: IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(e => e.Surname)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(e => e.IsActivate)
               .IsRequired();

        builder.Property(e => e.RefreshToken)
               .HasMaxLength(500);

        builder.Property(e => e.RefreshTokenExpireAt);

            builder.HasMany(e => e.WishlistItems)
                       .WithOne(w => w.User)
                       .HasForeignKey(w => w.UserId)
                       .OnDelete(DeleteBehavior.Cascade);
        }
}

    }

