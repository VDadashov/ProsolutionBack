using ProSolution.Core.Entities;
using ProSolution.Core.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSolution.Core.Repositories
{
    public interface IBlogRepository : IGenericRepository<Blog>
    {
        // rewiew and replay add delete
        Task<BlogReview> AddBlogReview(BlogReview blogReview);
        Task<BlogReviewReply> AddBlogReviewReply(BlogReviewReply blogReviewReply);
        Task<BlogReview> DeleteBlogReview(string id);
        Task<BlogReviewReply> DeleteBlogReviewReply(string id);
    }
    
    
}
