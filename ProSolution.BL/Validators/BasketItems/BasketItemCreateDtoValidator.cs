using FluentValidation;
using ProSolution.BL.DTOs;

namespace ProSolution.BL.Validators.BasketItems;

public class BasketItemCreateDtoValidator : AbstractValidator<BasketItemCreateDto>
{
    public BasketItemCreateDtoValidator()
    {
        RuleFor(dto => dto.Count)
            .NotNull().WithMessage("Count is required.")
            .GreaterThanOrEqualTo(0).WithMessage("Count must be greater than or equal to 0.");

        RuleFor(dto => dto.ProductId)
            .NotNull().WithMessage("ProductId is required.");
    }
}
