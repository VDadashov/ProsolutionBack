using AutoMapper;
using ProSolution.Core.Entities.Identity;
using static ProSolution.BL.DTOs.User.UserAddressDto;

namespace ProSolution.BL.MapperProfiles
{

    internal class UserAddressMP : Profile
    {
        public UserAddressMP()
        {
            CreateMap<UserAddress, UserAddressResultDTO>().ReverseMap();
            CreateMap<UserAddressCreateDTO, UserAddress>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember, context) =>
                {
                    if (srcMember is string strVal)
                    {
                        // "string" literal olaraq gəlirsə, ignore et
                        return strVal != "string";
                    }
                    return true;
                }));
            CreateMap<UserAddressUpdateDTO, UserAddress>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember, context) =>
                {
                    if (srcMember is string strVal)
                    {
                        // "string" literal olaraq gəlirsə, ignore et
                        return strVal != "string";
                    }
                    return true;
                }));
        }
    }

}
