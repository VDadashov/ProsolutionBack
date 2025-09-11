using ProSolution.BL.Services.Interfaces;
using ProSolution.Core.Entities;
using ProSolution.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSolution.BL.Services.Implements
{
    public class ReviewService : IReviewService
    {

        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }
        public async Task<List<ProductReview>> GetUnconfirmedProductReviewsAsync()
        {
            return await _reviewRepository.GetUnconfirmedProductReviewsAsync();
        }

        public async Task ConfirmProductReviewAsync(string id)
        {
            await _reviewRepository.ConfirmProductReviewAsync(id);
        }

        // ----------------------------
        // BlogReview
        // ----------------------------
        public async Task<List<BlogReview>> GetUnconfirmedBlogReviewsAsync()
        {
            return await _reviewRepository.GetUnconfirmedBlogReviewsAsync();
        }

        public async Task ConfirmBlogReviewAsync(string id)
        {
            await _reviewRepository.ConfirmBlogReviewAsync(id);
        }

        // ----------------------------
        // BlogReviewReply
        // ----------------------------
        public async Task<List<BlogReviewReply>> GetUnconfirmedBlogReviewRepliesAsync()
        {
            return await _reviewRepository.GetUnconfirmedBlogReviewRepliesAsync();
        }

        public async Task ConfirmBlogReviewReplyAsync(string id)
        {
            await _reviewRepository.ConfirmBlogReviewReplyAsync(id);
        }

        public async Task DeleteProductReviewAsync(string id)
        {
            await _reviewRepository.DeleteProductReviewAsync(id);
        }

        public async Task DeleteBlogReviewAsync(string id)
        {
            await _reviewRepository.DeleteBlogReviewAsync(id);
        }

        public async Task DeleteBlogReviewReplyAsync(string id)
        {
            await _reviewRepository.DeleteBlogReviewReplyAsync(id);
        }

    }
}
