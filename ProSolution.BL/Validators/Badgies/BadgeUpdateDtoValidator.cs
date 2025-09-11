using FluentValidation;
using ProSolution.BL.DTOs;

namespace ProSolution.BL.Validators.BadgeValidator
{
    public class BadgeUpdateDtoValidator : AbstractValidator<BadgeUpdateDto>
    {
        public BadgeUpdateDtoValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(255).WithMessage("Description must not exceed 255 characters.");

            // Image is optional, but if present, validate it
            When(x => x.Image != null, () =>
            {
                RuleFor(x => x.Image)
                    .Must(f => f.Length > 0).WithMessage("Uploaded image file cannot be empty.");
            });
        }
    }
}
