using FluentValidation;
using ProSolution.BL.DTOs.Characteristics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSolution.BL.Validators.Characteristics
{
    public class FeatureOptionCreateDtoValidator : AbstractValidator<FeatureOptionCreateDto>
    {
        public FeatureOptionCreateDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        }
    }

}
