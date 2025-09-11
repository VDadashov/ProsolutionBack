using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProSolution.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSolution.DAL.Configurations
{
    internal class BlogReviewDbConf : IEntityTypeConfiguration<BlogReview>
    {
        public void Configure(EntityTypeBuilder<BlogReview> builder)
        {
            builder.Property(x => x.Text)
                .IsRequired();

          
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(150);

          

            builder.HasOne(x => x.Blog)
                .WithMany(x => x.BlogReviews)
                .HasForeignKey(x => x.BlogId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.BlogReviewReplies)
                .WithOne(x => x.BlogReview)
                .HasForeignKey(x => x.BlogReviewId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
