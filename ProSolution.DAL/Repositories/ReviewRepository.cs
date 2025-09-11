using Microsoft.EntityFrameworkCore;
using ProSolution.Core.Entities;
using ProSolution.Core.Repositories;
using ProSolution.DAL.Contexts;

namespace ProSolution.DAL.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // ProductReview
        // =========================
        public async Task<List<ProductReview>> GetUnconfirmedProductReviewsAsync()
        {
            return await _context.ProductReviews
                .Where(r => !r.Checked)
                .ToListAsync();
        }

        public async Task ConfirmProductReviewAsync(string id)
        {
            var review = await _context.ProductReviews.FindAsync(id);
            if (review != null)
            {
                review.Checked = true;
                await _context.SaveChangesAsync();
            }
        }

        // =========================
        // BlogReview
        // =========================
        public async Task<List<BlogReview>> GetUnconfirmedBlogReviewsAsync()
        {
            return await _context.BlogReviews
                .Where(r => !r.Checked)
                .ToListAsync();
        }

        public async Task ConfirmBlogReviewAsync(string id)
        {
            var review = await _context.BlogReviews.FindAsync(id);
            if (review != null)
            {
                review.Checked = true;
                await _context.SaveChangesAsync();
            }
        }

        // =========================
        // BlogReviewReply
        // =========================
        public async Task<List<BlogReviewReply>> GetUnconfirmedBlogReviewRepliesAsync()
        {
            return await _context.BlogReviewReplies
                .Where(r => !r.Checked)
                .ToListAsync();
        }

        public async Task ConfirmBlogReviewReplyAsync(string id)
        {
            var review = await _context.BlogReviewReplies.FindAsync(id);
            if (review != null)
            {
                review.Checked = true;
                await _context.SaveChangesAsync();
            }
        }


        public async Task DeleteProductReviewAsync(string id)
        {
            var review = await _context.ProductReviews.FindAsync(id);
            if (review != null)
            {
                _context.ProductReviews.Remove(review);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteBlogReviewAsync(string id)
        {
            var review = await _context.BlogReviews.FindAsync(id);
            if (review != null)
            {
                _context.BlogReviews.Remove(review);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteBlogReviewReplyAsync(string id)
        {
            var review = await _context.BlogReviewReplies.FindAsync(id);
            if (review != null)
            {
                _context.BlogReviewReplies.Remove(review);
                await _context.SaveChangesAsync();
            }
        }

    }
}

