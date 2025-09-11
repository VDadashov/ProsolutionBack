using FluentValidation;
using ProSolution.BL.DTOs;

namespace ProSolution.BL.Validators.OurServicies
{
    public class OurServiceUpdateDtoValidator : AbstractValidator<OurServiceUpdateDto>
    {
        public OurServiceUpdateDtoValidator()
        {
            RuleFor(x => x.Title)
                 .NotEmpty().WithMessage("Title is required.")
                 .MaximumLength(100).WithMessage("Title must be at most 100 characters.")
                 .Matches(@"^[\p{L}0-9\s\-()']+$")
                 .WithMessage("Title can only contain letters, numbers, spaces, and basic symbols.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(1000).WithMessage("Description must be at most 1000 characters.");

            RuleFor(x => x.Image)
                .NotNull().WithMessage("Image is required.");

            RuleFor(x => x.ContentTitle)
                .NotEmpty().WithMessage("ContentTitle is required.")
                .MaximumLength(100).WithMessage("ContentTitle must be at most 100 characters.")
                .Matches(@"^[\p{L}0-9\s\-()']+$")
                .WithMessage("ContentTitle can only contain letters, numbers, spaces, and basic symbols.");

            RuleFor(x => x.ContentDescription)
                .NotEmpty().WithMessage("ContentDescription is required.")
                .MaximumLength(1000).WithMessage("ContentDescription must be at most 1000 characters.");

            RuleFor(x => x.ContentImage)
                .NotNull().WithMessage("ContentImage is required.");
        }
    }
}
