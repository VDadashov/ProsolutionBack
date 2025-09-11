using ProSolution.Core.Entities.Identity;

namespace ProSolution.Core.Entities
{
    public class Blog : BaseEntity
    {
        // public string UserImageUrl { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public string? Slug { get; set; }

        public string CategoryId { get; set; }
        public Category Category { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public ICollection<BlogReview>? BlogReviews { get; set; }
    }
}
