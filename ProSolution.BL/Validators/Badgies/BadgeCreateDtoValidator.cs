using FluentValidation;
using ProSolution.BL.DTOs;

namespace ProSolution.BL.Validators.BadgeValidator
{
    public class BadgeCreateDtoValidator : AbstractValidator<BadgeCreateDto>
    {
        public BadgeCreateDtoValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(255).WithMessage("Description must not exceed 255 characters.");

            RuleFor(x => x.Image)
                .NotNull().WithMessage("Image file is required.");

            RuleFor(x => x.IsSertificate)
                .NotNull().WithMessage("IsSertificate field is required.");
        }
    }
}
