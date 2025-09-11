using FluentValidation;
using ProSolution.BL.DTOs;

namespace ProSolution.BL.Validators.Blogs.BlogReviews
{
    public class BlogReviewCreateDtoValidator : AbstractValidator<BlogReviewCreateDto>
    {
        public BlogReviewCreateDtoValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Text is required.")
                .MaximumLength(1000).WithMessage("Text must be at most 1000 characters.")
                .Matches(@"^[\s\S]{1,1000}$").WithMessage("Text contains invalid characters.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must be at most 100 characters.")
                .Matches(@"^[\p{L}\s\-']+$").WithMessage("Name can only contain letters, spaces, hyphens, and apostrophes.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .MaximumLength(150).WithMessage("Email must be at most 150 characters.")
                .EmailAddress().WithMessage("Email format is invalid.");


            RuleFor(x => x.BlogId)
                .NotEmpty().WithMessage("BlogId is required.");
        }
    }
}
