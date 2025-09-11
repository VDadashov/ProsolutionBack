using FluentValidation;
using ProSolution.BL.DTOs;

namespace ProSolution.BL.Validators.Blogs
{
    public class BlogUpdateDtoValidator : AbstractValidator<BlogUpdateDto>
    {
        public BlogUpdateDtoValidator()
        {
            //RuleFor(x => x.UserImageUrl)
            //    .NotEmpty().WithMessage("UserImageUrl is required.")
            //    .Matches(@"^https?:\/\/[\w\-\.]+(\.[\w\-]+)+[/#?]?.*$")
            //    .WithMessage("UserImageUrl must be a valid URL.");

      

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(10000).WithMessage("Description must be at most 2000 characters.")
                .Matches(@"^[\p{L}0-9\s.,:;!?()'""%&@\-]{1,10000}$")
                .WithMessage("Description can only contain letters, numbers, spaces, and basic symbols.");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("CategoryId is required.")
                .Matches(@"^[a-fA-F0-9]{24}$")
                .WithMessage("CategoryId must be a valid 24-character hex string (MongoDB ObjectId).");
        }
    }
}
