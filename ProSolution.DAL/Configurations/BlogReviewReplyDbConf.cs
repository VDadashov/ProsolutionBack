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
    internal class BlogReviewReplyDbConf : IEntityTypeConfiguration<BlogReviewReply>
    {
        public void Configure(EntityTypeBuilder<BlogReviewReply> builder)
        {
            builder.Property(x => x.Text)
                .IsRequired();

           
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(150);


           

            builder.HasOne(x => x.BlogReview)
                .WithMany(x => x.BlogReviewReplies)
                .HasForeignKey(x => x.BlogReviewId)
                .OnDelete(DeleteBehavior.Restrict);
        }


    }
}
