using AutoMapper;
using ProSolution.BL.DTOs;
using ProSolution.Core.Entities;

namespace ProSolution.BL.Mappers
{
    internal class BadgeMP : Profile
    {
        public BadgeMP()
        {
            CreateMap<Badge, BadgeGetDto>().ReverseMap();

            CreateMap<BadgeCreateDto, Badge>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
           .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember, context) =>
           {
               if (srcMember is string strVal)
               {
                   // "string" literal olaraq gəlirsə, ignore et
                   return strVal != "string";
               }
               return true;
           }));

            CreateMap<BadgeUpdateDto, Badge>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
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
