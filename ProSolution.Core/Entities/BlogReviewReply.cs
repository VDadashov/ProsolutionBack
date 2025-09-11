namespace ProSolution.Core.Entities
{
    public class BlogReviewReply : Review
    {
      

        public string BlogReviewId { get; set; }
        public BlogReview BlogReview { get; set; }
    }
}
