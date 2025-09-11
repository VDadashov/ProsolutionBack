using FluentValidation;
using ProSolution.BL.DTOs.Commons;

namespace ProSolution.BL.Validators.Commons
{
    public class BaseEntityValidator<T> : AbstractValidator<T> where T : BaseEntityDTO
    {
        public BaseEntityValidator()
        {
            RuleFor(entity => entity.Id)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Id is required.");
        }   
    }
}
