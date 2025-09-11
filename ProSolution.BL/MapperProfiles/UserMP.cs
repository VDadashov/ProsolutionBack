using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MimeKit.Encodings;
using ProSolution.BL.DTOs.User;
using ProSolution.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSolution.BL.MapperProfiles
{
    public class UserMP : Profile
    {
        public UserMP()
        {
            CreateMap<User, LoginUserDTO>().ReverseMap();
            CreateMap<RegisterUserDTO, User>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember, context) =>
                {
                    if (srcMember is string strVal)
                    {
                        // "string" literal olaraq gəlirsə, ignore et
                        return strVal != "string";
                    }
                    return true;
                }));
            CreateMap<User, RegisterUserDTO>();

            CreateMap<User, AppUserGetDto>()
     .ForMember(dest => dest.Role, opt => opt.MapFrom<RoleResolver>());



            CreateMap<User, ChangeRoleDto>().ReverseMap();

            CreateMap<UpdateProfileDTO, User>()
                
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))

           
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Slug, opt => opt.Ignore()) 
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) 

            
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                {
                    if (srcMember is string str)
                    {
                        return !string.IsNullOrWhiteSpace(str);
                    }
                    return srcMember != null;
                }));
        }
    }
}
public class RoleResolver : IValueResolver<User, AppUserGetDto, string?>
{
    private readonly UserManager<User> _userManager;

    public RoleResolver(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public string? Resolve(User source, AppUserGetDto destination, string? destMember, ResolutionContext context)
    {
        var roles = _userManager.GetRolesAsync(source).Result;
        return roles.FirstOrDefault();
    }
}
