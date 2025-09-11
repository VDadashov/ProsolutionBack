using FluentValidation;
using ProSolution.BL.DTOs;

namespace ProSolution.BL.Validators.Blogs
{
    public class BlogCreateDtoValidator : AbstractValidator<BlogCreateDto>
    {
        public BlogCreateDtoValidator()
        {
           
          
            RuleFor(x => x.Image)
                .NotNull().WithMessage("ImageUrl is required.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(100000).WithMessage("Description must be at most 2000 characters.")
                .Matches(@"^[\p{L}0-9\s.,:;!?()'""%&@\-]{1,100000}$")
                .WithMessage("Description can only contain letters, numbers, spaces, and basic symbols.");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("CategoryId is required.");
        }
    }
}
