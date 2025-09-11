using ProSolution.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSolution.BL.Services.Interfaces
{
    public interface IReviewService
    {
        //rewiev

        Task<List<ProductReview>> GetUnconfirmedProductReviewsAsync();
        Task ConfirmProductReviewAsync(string id);

        Task<List<BlogReview>> GetUnconfirmedBlogReviewsAsync();
        Task ConfirmBlogReviewAsync(string id);

        Task<List<BlogReviewReply>> GetUnconfirmedBlogReviewRepliesAsync();
        Task ConfirmBlogReviewReplyAsync(string id);


        Task DeleteProductReviewAsync(string id);
        Task DeleteBlogReviewAsync(string id);
        Task DeleteBlogReviewReplyAsync(string id);

    }
}
