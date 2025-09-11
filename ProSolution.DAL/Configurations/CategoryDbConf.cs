using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProSolution.Core.Entities;

namespace ProSolution.DAL.Configurations
{
    internal class CategoryDbConf : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(255);


            builder.HasMany(ci => ci.CategoryProducts)
                   .WithOne(cp => cp.Category)
                   .HasForeignKey(cp => cp.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ci => ci.Parent)
                   .WithMany(ci => ci.Children)
                   .HasForeignKey(ci => ci.ParentId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
