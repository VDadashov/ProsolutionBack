using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProSolution.BL.Services.Interfaces;

namespace ProSolution.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }


        [HttpGet("blog/unconfirmed")]
        public async Task<IActionResult> GetUnconfirmedBlogReviews()
        {
            var reviews = await _reviewService.GetUnconfirmedBlogReviewsAsync();
            return Ok(reviews);
        }
        [HttpGet("reply/unconfirmed")]
        public async Task<IActionResult> GetUnconfirmedBlogReviewReplies()
        {
            var reviews = await _reviewService.GetUnconfirmedBlogReviewRepliesAsync();
            return Ok(reviews);
        }

        [HttpGet("product/unconfirmed")]
        public async Task<IActionResult> GetUnconfirmedProductReviews()
        {
            var reviews = await _reviewService.GetUnconfirmedProductReviewsAsync();
            return Ok(reviews);
        }

        [HttpPost("product/confirm/{id}")]
        public async Task<IActionResult> ConfirmProductReview(string id)
        {
            await _reviewService.ConfirmProductReviewAsync(id);
            return Ok(new { message = "Product review confirmed." });
        }

        // ================================
        // Blog Reviews
        // ================================
       

        [HttpPost("blog/confirm/{id}")]
        public async Task<IActionResult> ConfirmBlogReview(string id)
        {
            await _reviewService.ConfirmBlogReviewAsync(id);
            return Ok(new { message = "Blog review confirmed." });
        }

        // ================================
        // Blog Review Replies
        // ================================
       

        [HttpPost("reply/confirm/{id}")]
        public async Task<IActionResult> ConfirmBlogReviewReply(string id)
        {
            await _reviewService.ConfirmBlogReviewReplyAsync(id);
            return Ok(new { message = "Blog review reply confirmed." });
        }

        [HttpDelete("product/delete/{id}")]
        public async Task<IActionResult> DeleteProductReview(string id)
        {
            await _reviewService.DeleteProductReviewAsync(id);
            return Ok(new { message = "Product review deleted." });
        }

        [HttpDelete("blog/delete/{id}")]
        public async Task<IActionResult> DeleteBlogReview(string id)
        {
            await _reviewService.DeleteBlogReviewAsync(id);
            return Ok(new { message = "Blog review deleted." });
        }

        [HttpDelete("reply/delete/{id}")]
        public async Task<IActionResult> DeleteBlogReviewReply(string id)
        {
            await _reviewService.DeleteBlogReviewReplyAsync(id);
            return Ok(new { message = "Blog review reply deleted." });
        }

    }
}
