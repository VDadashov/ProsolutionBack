using FluentValidation;
using ProSolution.BL.DTOs;

namespace ProSolution.BL.Validators.SEOValidator
{
    public class CreateSEODTOValidator : AbstractValidator<CreateSEODTO>
    {
        public CreateSEODTOValidator()
        {
            RuleFor(dto => dto.AltText)
                .NotEmpty().WithMessage("AltText is required.")
                .MaximumLength(360).WithMessage("AltText must be 360 characters or less.");

            RuleFor(dto => dto.AnchorText)
                .NotEmpty().WithMessage("AnchorText  is required.")
                .MaximumLength(1600).WithMessage("AnchorText must be 160 characters or less.");
 
        }
    }
    public class CreateSEOMetaDTOValidator : AbstractValidator<CreateSEOMetaDTO>
    {
        public CreateSEOMetaDTOValidator()
        {
            RuleFor(dto => dto.MetaDescription)
                .MaximumLength(360).WithMessage("Meta Description must be 360 characters or less.");

            RuleFor(dto => dto.MetaTitle)
                .MaximumLength(160).WithMessage("Meta Title must be 160 characters or less.");

            RuleFor(dto => dto.MetaTags)
                .MaximumLength(455).WithMessage("Meta Tags must be 455 characters or less.");
        }
    }

    public class CreateSEOUrlDTOValidator : AbstractValidator<CreateSeoUrlDTO>
    {
        public CreateSEOUrlDTOValidator()
        {
            RuleFor(dto => dto.Url)
                .NotEmpty().WithMessage("Url is required.")
                .MaximumLength(360).WithMessage("Url must be 360 characters or less.");

            RuleFor(dto => dto.RedirectUrl)
                .NotEmpty().WithMessage("RedirectUrl is required.")
                .MaximumLength(1600).WithMessage("RedirectUrl must be 160 characters or less.");
        }
    }
}
