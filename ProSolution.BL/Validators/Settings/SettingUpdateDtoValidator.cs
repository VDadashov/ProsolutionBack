using FluentValidation;
using ProSolution.BL.DTOs;

namespace ProSolution.BL.Validators.Settings
{
    public class SettingUpdateDtoValidator : AbstractValidator<SettingUpdateDto>
    {
        public SettingUpdateDtoValidator()
        {
            RuleFor(x => x.Value)
                .NotEmpty().WithMessage("Value is required.")
                .MaximumLength(1000).WithMessage("Value must be at most 1000 characters.")
                .Matches(@"^[\s\S]{1,1000}$")
                .WithMessage("Value contains invalid characters.");
        }
    }
}
