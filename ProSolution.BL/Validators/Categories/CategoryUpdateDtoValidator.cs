using FluentValidation;
using ProSolution.BL.DTOs;

namespace ProSolution.BL.Validators.Categories
{
    public class CategoryUpdateDtoValidator : AbstractValidator<CategoryUpdateDto>
    {
        public CategoryUpdateDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title must be at most 100 characters.")
                .Matches(@"^[\p{L}0-9\s\-()']+$")
                .WithMessage("Title can only contain letters, numbers, spaces, and basic symbols.");
        }
    }
}
