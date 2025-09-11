using FluentValidation;
using ProSolution.BL.DTOs;

namespace ProSolution.BL.Validators.Partners
{
    public class PartnerCreateDtoValidator : AbstractValidator<PartnerCreateDto>
    {
        public PartnerCreateDtoValidator()
        {
            RuleFor(x => x.Image)
                .NotNull().WithMessage("Image is required.");

            RuleFor(x => x.AltText)
                .MaximumLength(150).WithMessage("AltText must be at most 150 characters.")
                .Matches(@"^[\p{L}0-9\s\-.,'""()!?%]*$").When(x => !string.IsNullOrEmpty(x.AltText))
                .WithMessage("AltText contains invalid characters.");
        }
    }
}
