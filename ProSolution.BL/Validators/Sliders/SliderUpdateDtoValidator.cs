using FluentValidation;
using ProSolution.BL.DTOs;

namespace ProSolution.BL.Validators.SliderValidator
{
    public class SliderUpdateDtoValidator : AbstractValidator<SliderUpdateDto>
    {
        public SliderUpdateDtoValidator()
        {
            RuleFor(dto => dto.AltText)
                  .NotEmpty().WithMessage("AltText is required.")
                  .MaximumLength(150).WithMessage("AltText must be less than 150 characters.");

            RuleFor(dto => dto.Image)
                    .NotEmpty().WithMessage("Image is required.");
        }
    }
}
