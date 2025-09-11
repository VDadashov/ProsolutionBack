using FluentValidation;
using ProSolution.BL.DTOs.ProductFeatureKeys;

namespace ProSolution.BL.Validators.ProductFeatureKeys
{
    public class ProductFeatureKeysCreateDtoValidator : AbstractValidator<ProductFeatureKeysCreateDto>
    {
        public ProductFeatureKeysCreateDtoValidator()
        {
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Категория обязательна.");
            RuleFor(x => x.FeatureOptionIds).NotEmpty().WithMessage("Нужно выбрать хотя бы один фильтр.");
        }
    }

    public class ProductFeatureKeysUpdateDtoValidator : AbstractValidator<ProductFeatureKeysUpdateDto>
    {
        public ProductFeatureKeysUpdateDtoValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            Include(new ProductFeatureKeysCreateDtoValidator());
        }
    }
}
