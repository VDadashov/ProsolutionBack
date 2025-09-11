using ProSolution.Core.Entities;
using ProSolution.Core.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSolution.Core.Repositories
{
    using ProSolution.Core.Entities;

    public interface IReviewRepository
    {
        // ProductReview
        Task<List<ProductReview>> GetUnconfirmedProductReviewsAsync();
        Task ConfirmProductReviewAsync(string id);

        // BlogReview
        Task<List<BlogReview>> GetUnconfirmedBlogReviewsAsync();
        Task ConfirmBlogReviewAsync(string id);

        // BlogReviewReply
        Task<List<BlogReviewReply>> GetUnconfirmedBlogReviewRepliesAsync();
        Task ConfirmBlogReviewReplyAsync(string id);

        Task DeleteProductReviewAsync(string id);
        Task DeleteBlogReviewAsync(string id);
        Task DeleteBlogReviewReplyAsync(string id);

    }

}
