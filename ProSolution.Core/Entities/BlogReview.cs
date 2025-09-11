namespace ProSolution.Core.Entities
{
    public class BlogReview : Review
    {
        public string BlogId { get; set; }
        public Blog Blog { get; set; }

        public ICollection<BlogReviewReply>? BlogReviewReplies { get; set; }
    }
}
