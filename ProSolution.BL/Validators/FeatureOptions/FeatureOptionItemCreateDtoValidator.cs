using FluentValidation;
using ProSolution.BL.DTOs.FeatureOPtions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSolution.BL.Validators.FeatureOptions
{
    public class FeatureOptionItemCreateDtoValidator : AbstractValidator<FeatureOptionItemCreateDto>
    {
        public FeatureOptionItemCreateDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            
        }
    }

    public class FeatureOptionItemUpdateDtoValidator : AbstractValidator<FeatureOptionItemUpdateDto>
    {
        public FeatureOptionItemUpdateDtoValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
          
        }
    }

}
