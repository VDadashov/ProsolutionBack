using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProSolution.BL.DTOs;
using ProSolution.BL.Services.Interfaces;
using ProSolution.Core.Entities;
using ProSolution.Core.Entities.Identity;
using ProSolution.Core.Repositories;
using ProSolution.Core.Repositories.Common;
using System.Net.Http;
using System.Security.Claims;


namespace ProSolution.BL.Services.Implements
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;
        private readonly ICloudStorageService _cloudStorageService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        public BlogService(IBlogRepository blogRepository, IMapper mapper, ICloudStorageService cloudStorageService, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
            _cloudStorageService = cloudStorageService;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        // Blog CRUD
        public async Task CreateAsync(BlogCreateDto dto)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId))
            {
                throw new Exception("zehmet olmasa qeydiyatdan kecin");
            }

          
            var currentUser = await _userManager.FindByIdAsync(currentUserId);
            if (currentUser == null)
            {
                throw new Exception("zehmet olmasa qeydiyatdan kecin");
            }

            Blog blog = _mapper.Map<Blog>(dto);

            blog.UserId = currentUserId;
            blog.Slug = _generateSlug(blog.Title);

            if (dto.Image != null)
            {
                blog.ImageUrl = await _cloudStorageService.UploadFileAsync(dto.Image, "Blog");
            }
            else
            {
                throw new Exception("sekil elave edin");
            }

            await _blogRepository.AddAsync(blog);
            await _blogRepository.SaveChangeAsync();
        }

        public async Task UpdateAsync(string id, BlogUpdateDto dto)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId))
            {
                throw new Exception("zehmet olmasa qeydiyatdan kecin");
            }

            var blog = await _blogRepository.GetByExpressionAsync(b => b.Id == id);
            if (blog == null)
            {
                throw new Exception("blog tapilmadi");
            }

            var isUserAdmin = _httpContextAccessor.HttpContext.User.IsInRole("Admin");

            // Если пользователь НЕ является создателем поста И при этом НЕ является администратором,
            // то запрещаем доступ.
            if (blog.UserId != currentUserId && !isUserAdmin)
            {
                throw new Exception("siz bu blogun yaradicisi deyilsiz");
            }
            string oldImageUrl = blog.ImageUrl;
            _mapper.Map(dto, blog);

            if (dto.Image != null)
            {
                if (!string.IsNullOrEmpty(oldImageUrl))
                {
                    await _cloudStorageService.DeleteFileAsync(oldImageUrl);
                }
                blog.ImageUrl = await _cloudStorageService.UploadFileAsync(dto.Image, "Blog");
            }

            blog.Slug = _generateSlug(blog.Title);
            _blogRepository.Update(blog);
            await _blogRepository.SaveChangeAsync();
        }


        public async Task SoftDeleteAsync(string id, bool isDelete)
        {
            var blog = await _blogRepository.GetByIdAsync(id);
            if (blog == null) throw new Exception("Blog not found");
            blog.IsDeleted = isDelete;
            await _blogRepository.SaveChangeAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var blog = await _blogRepository.GetByIdAsync(id);
            if (blog == null) throw new Exception("Blog not found");
            _blogRepository.Delete(blog);
            await _blogRepository.SaveChangeAsync();
        }

        public async Task<ICollection<BlogGetDto>> GetAllAsync()
        {
            var blogs = await _blogRepository.GetAll().Include(b => b.Category).ToListAsync();

            return _mapper.Map<ICollection<BlogGetDto>>(blogs);
        }

        public async Task<PaginationDto<BlogGetDto>> GetAllFilteredAsync(
      string? search,
      int take,
      int page,
      int order,
      bool isDeleted,
      string? authorSlug) // Parameter renamed for clarity
        {
            if (page <= 0 || take <= 0 || order <= 0 || order > 2)
                throw new Exception("Filter parameters are incorrect.");

            // Start with the base IQueryable
            var query = _blogRepository.GetAll(isTracking: false);

            // --- REFACTORED FILTERING LOGIC ---

            // Apply filters first
            query = query.Where(b => b.IsDeleted == isDeleted);

            if (!string.IsNullOrWhiteSpace(authorSlug))
            {
                query = query.Where(b => b.User.Slug.ToLower() == authorSlug.ToLower());
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchTerm = search.ToLower();
                query = query.Where(b =>
                    b.Title.ToLower().Contains(searchTerm) ||
                    b.User.Name.ToLower().Contains(searchTerm) ||
                    b.User.Surname.ToLower().Contains(searchTerm));
            }

            // Get the total count after filtering
            double count = await query.CountAsync();

            // --- REFACTORED ORDERING, INCLUDES, AND PAGINATION ---

            // Apply ordering
            IOrderedQueryable<ProSolution.Core.Entities.Blog> orderedQuery = order switch
            {
                1 => query.OrderBy(b => b.CreatedAt),      // Oldest first
                2 => query.OrderByDescending(b => b.CreatedAt), // Newest first
                _ => query.OrderBy(b => b.CreatedAt) // Default case
            };

            // Apply pagination and then includes
            var blogs = await orderedQuery
                .Skip((page - 1) * take)
                .Take(take)
                .Include(b => b.User) // Apply includes after pagination
                .Include(b => b.Category)
                .ToListAsync();

            var dtos = _mapper.Map<ICollection<BlogGetDto>>(blogs);

            return new PaginationDto<BlogGetDto>
            {
                SlugPath = authorSlug,
                Take = take,
                Search = search,
                Order = order,
                CurrentPage = page,
                TotalPage = Math.Ceiling(count / take),
                Count = count,
                Items = dtos
            };
        }

        public async Task<BlogGetDto> GetByIdAsync(string id)
        {
            string[] includes =
            { $"{nameof(Blog.Category)}",
            $"{nameof(Blog.BlogReviews)}.{nameof(BlogReview.BlogReviewReplies)}",
            $"{nameof(Blog.User)}"};

            var blog = await _blogRepository.GetByIdAsync(id, false, includes);

            if (blog == null) throw new Exception("Blog not found");

            // Убираем непроверенные отзывы и их ответы
            blog.BlogReviews = blog.BlogReviews
                .Where(r => r.Checked)
                .Select(r =>
                {
                    r.BlogReviewReplies = r.BlogReviewReplies?.Where(reply => reply.Checked).ToList();
                    return r;
                })
                .ToList();

            return _mapper.Map<BlogGetDto>(blog);
        }

        public async Task<BlogGetDto> GetBySlugAsync(string slug)
        {
            string[] includes =
            { $"{nameof(Blog.Category)}",
            $"{nameof(Blog.BlogReviews)}.{nameof(BlogReview.BlogReviewReplies)}",
            $"{nameof(Blog.User)}" };

            var blog = await _blogRepository.GetByExpressionAsync(x => x.Slug.Trim().ToLower().Contains(slug.Trim().ToLower()));

            if (blog == null) throw new Exception("Blog not found");

            // Убираем непроверенные отзывы и их ответы
            blog.BlogReviews = blog.BlogReviews
                .Where(r => r.Checked)
                .Select(r =>
                {
                    r.BlogReviewReplies = r.BlogReviewReplies?.Where(reply => reply.Checked).ToList();
                    return r;
                })
                .ToList();

            return _mapper.Map<BlogGetDto>(blog);
        }

        // BlogReview
        public async Task<BlogReviewGetDto> AddReviewAsync(BlogReviewCreateDto dto)
        {
            var review = _mapper.Map<BlogReview>(dto);
            await _blogRepository.AddBlogReview(review);
            return _mapper.Map<BlogReviewGetDto>(review);
        }

        public async Task DeleteReviewAsync(string id)
        {
            await _blogRepository.DeleteBlogReview(id);
        }

        // BlogReviewReply
        public async Task<BlogReviewReplyGetDto> AddReviewReplyAsync(BlogReviewReplyCreateDto dto)
        {
            var reply = _mapper.Map<BlogReviewReply>(dto);
            await _blogRepository.AddBlogReviewReply(reply);
            return _mapper.Map<BlogReviewReplyGetDto>(reply);
        }

        public async Task DeleteReviewReplyAsync(string id)
        {
            await _blogRepository.DeleteBlogReviewReply(id);
        }

        private string _generateSlug(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return string.Empty;

            // Trim və bütün ardıcıl boşluqları tək boşluğa sal
            var normalized = System.Text.RegularExpressions.Regex.Replace(title.Trim().ToLower(), @"\s+", " ");

            // Sonra boşluqları "-" ilə əvəz et
            var slug = normalized.Replace(" ", "-");

            return slug.ToLowerInvariant();
        }
    }
}
