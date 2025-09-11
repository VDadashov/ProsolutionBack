using FluentValidation;
using ProSolution.BL.DTOs;

namespace ProSolution.BL.Validators.ProductValidator
{
    public class UpdateProductDtoValidator : AbstractValidator<ProductUpdateDto>
    {
        public UpdateProductDtoValidator()
        {

            RuleFor(dto => dto.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(150).WithMessage("Title must be less than 150 characters.");

            RuleFor(dto => dto.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(1000).WithMessage("Description must be less than 1000 characters.");

            RuleFor(dto => dto.Price)
                .NotNull().WithMessage("Price is required.")
                .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to 0.");


            RuleFor(dto => dto.SoldCount)
                .GreaterThanOrEqualTo(0).WithMessage("Stock must be greater than or equal to 0.");

            RuleFor(dto => dto.SoldCount)
                .GreaterThanOrEqualTo(0).WithMessage("Stock must be greater than or equal to 0.");
            RuleFor(dto => dto.InStock)
            .NotNull().WithMessage("InStock status is required.");

            // --- ДРУГИЕ ВАЖНЫЕ ПРАВИЛА ---
            // Не забывайте добавлять правила для других обязательных полей, таких как BrandId и категории
            RuleFor(dto => dto.BrandId)
                .NotEmpty().WithMessage("BrandId is required.");

            RuleFor(dto => dto.CategoryItemIds)
                .NotEmpty().WithMessage("At least one category is required.");
        }
    }
}
