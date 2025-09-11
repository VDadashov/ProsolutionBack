using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProSolution.BL.DTOs.User.UserAddressDto;

namespace ProSolution.BL.Validators.User
{
   
    public class UserAddressCreateValidator : AbstractValidator<UserAddressCreateDTO>
    {
        public UserAddressCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Surname).NotEmpty();
            RuleFor(x => x.Country).NotEmpty();
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.State).NotEmpty();
            RuleFor(x => x.Address).NotEmpty();
            RuleFor(x => x.ZipCode).NotEmpty();
        }
    }

}
