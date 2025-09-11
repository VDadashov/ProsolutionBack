using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProSolution.BL.DTOs;
using ProSolution.BL.Services.Interfaces;
using System.Threading.Tasks;

namespace ProSolution.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogsController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        // GET: api/blogs
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _blogService.GetAllAsync());
        }

        // GET: api/blogs/filtered
        [HttpGet("filtered")]
        public async Task<IActionResult> GetAllFiltered([FromQuery] string? search, [FromQuery] int take = 10, [FromQuery] int page = 1, [FromQuery] int order = 1, [FromQuery] bool isDeleted = false, [FromQuery]string? AuthorSlug=null)
        {
            return Ok(await _blogService.GetAllFilteredAsync(search, take, page, order, isDeleted, AuthorSlug));
        }

        // GET: api/blogs/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            return Ok(await _blogService.GetByIdAsync(id));
        }

        [HttpGet("[Action]/{id}")]
        public async Task<IActionResult> GetBySlug(string id)
        {
            return Ok(await _blogService.GetByIdAsync(id));
        }

        // POST: api/blogs
        [Authorize(Roles = "Admin, SuperAdmin, Author")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] BlogCreateDto dto)
        {
            await _blogService.CreateAsync(dto);
            return StatusCode(201);
        }

        // PUT: api/blogs/{id}
        [Authorize(Roles = "Admin, SuperAdmin, Author")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromForm] BlogUpdateDto dto)
        {
            await _blogService.UpdateAsync(id, dto);
            return NoContent();
        }

        // PATCH: api/blogs/{id}/soft-delete?isDelete=true
        [Authorize(Roles = "Admin, SuperAdmin, Author")]
        [HttpPatch("{id}/soft-delete")]
        public async Task<IActionResult> SoftDelete(string id, [FromQuery] bool isDelete)
        {
            await _blogService.SoftDeleteAsync(id, isDelete);
            return NoContent();
        }

        // DELETE: api/blogs/{id}
        [Authorize(Roles = "SuperAdmin, Author")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _blogService.DeleteAsync(id);
            return NoContent();
        }

        // POST: api/blogs/reviews
        [HttpPost("reviews")]
        public async Task<IActionResult> AddReview([FromBody] BlogReviewCreateDto dto)
        {
            var review = await _blogService.AddReviewAsync(dto);
            return Ok(review);
        }

        // DELETE: api/blogs/reviews/{id}
        [HttpDelete("reviews/{id}")]
        public async Task<IActionResult> DeleteReview(string id)
        {
            await _blogService.DeleteReviewAsync(id);
            return NoContent();
        }

        // POST: api/blogs/replies
        [HttpPost("replies")]
        public async Task<IActionResult> AddReviewReply([FromBody] BlogReviewReplyCreateDto dto)
        {
            var reply = await _blogService.AddReviewReplyAsync(dto);
            return Ok(reply);
        }

        // DELETE: api/blogs/replies/{id}
        [HttpDelete("replies/{id}")]
        public async Task<IActionResult> DeleteReviewReply(string id)
        {
            await _blogService.DeleteReviewReplyAsync(id);
            return NoContent();
        }
    }
}
