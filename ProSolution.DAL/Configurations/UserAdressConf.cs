using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProSolution.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSolution.DAL.Configurations
{
    internal class UserAdressConf : IEntityTypeConfiguration<UserAddress>
    {
        public void Configure(EntityTypeBuilder<UserAddress> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Address)
                .IsRequired()
                .HasMaxLength(500);
            builder.Property(e => e.City)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(e => e.Country)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(e => e.State)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(e => e.ZipCode)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(e => e.Surname)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(e => e.UserId)
                .IsRequired();
            builder.HasOne(e => e.User).WithOne(e => e.UserAddress)
                .HasForeignKey<UserAddress>(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
    
    }

