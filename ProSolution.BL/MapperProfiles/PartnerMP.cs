using AutoMapper;
using ProSolution.BL.DTOs;
using ProSolution.Core.Entities;

namespace ProSolution.BL.MapperProfiles
{
    internal class PartnerMP : Profile
    {
        public PartnerMP()
        {
            CreateMap<Partner, PartnerGetDto>().ReverseMap();

            CreateMap<PartnerCreateDto, Partner>()
                .ForMember(dest => dest.ImagePath, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember, context) =>
                {
                    if (srcMember is string strVal)
                    {
                        // "string" literal olaraq gəlirsə, ignore et
                        return strVal != "string";
                    }
                    return true;
                }));

            CreateMap<PartnerUpdateDto, Partner>()
                .ForMember(dest => dest.ImagePath, opt => opt.Ignore())
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
