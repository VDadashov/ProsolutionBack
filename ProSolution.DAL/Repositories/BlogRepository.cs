using Microsoft.EntityFrameworkCore;
using ProSolution.Core.Entities;
using ProSolution.Core.Repositories;
using ProSolution.DAL.Contexts;
using ProSolution.DAL.Repositories.Common;

public class BlogRepository : GenericRepository<Blog>, IBlogRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<BlogReview> _blogReviewsRepository;
    private readonly DbSet<BlogReviewReply> _blogReviewRepliesRepository;

    public BlogRepository(AppDbContext context) : base(context)
    {
        _context = context;
        _blogReviewRepliesRepository = context.Set<BlogReviewReply>();
        _blogReviewsRepository = context.Set<BlogReview>();
    }

    public async Task<BlogReview> AddBlogReview(BlogReview blogReview)
    {
        await _blogReviewsRepository.AddAsync(blogReview);
        await _context.SaveChangesAsync();
        return blogReview;
    }

    public async Task<BlogReviewReply> AddBlogReviewReply(BlogReviewReply blogReviewReply)
    {
        await _blogReviewRepliesRepository.AddAsync(blogReviewReply);
        await _context.SaveChangesAsync();
        return blogReviewReply;
    }

    public async Task<BlogReview?> DeleteBlogReview(string id)
    {
        var replies = _blogReviewRepliesRepository.Where(x => x.BlogReviewId == id).ToList();
        _blogReviewRepliesRepository.RemoveRange(replies);

        var review = await _blogReviewsRepository.FindAsync(id);
        if (review != null)
        {
            _blogReviewsRepository.Remove(review);
        }

        await _context.SaveChangesAsync();
        return review;
    }

    public async Task<BlogReviewReply?> DeleteBlogReviewReply(string id)
    {
        var reply = await _blogReviewRepliesRepository.FindAsync(id);
        if (reply != null)
        {
            _blogReviewRepliesRepository.Remove(reply);
            await _context.SaveChangesAsync();
        }

        return reply;
    }
}
