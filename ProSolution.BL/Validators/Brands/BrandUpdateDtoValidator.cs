using FluentValidation;
using ProSolution.BL.DTOs;

namespace ProSolution.BL.Validators.Brands
{
    public class BrandUpdateDtoValidator : AbstractValidator<BrandUpdateDto>
    {
        public BrandUpdateDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title must be at most 100 characters.")
                .Matches(@"^[\p{L}0-9\s\-()']+$")
                .WithMessage("Title can only contain letters, numbers, spaces, and basic symbols.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.")
                .Matches(@"^[\p{L}0-9\s.,:;!?()'""%&@\-]{1,500}$")
                .WithMessage("Description can only contain letters, numbers, spaces, and basic symbols.");
        }
    }
}
