using AutoMapper;
using ProSolution.BL.DTOs;
using ProSolution.Core.Entities;

namespace ProSolution.BL.MapperProfiles
{
    internal class SliderMP : Profile
    {
        public SliderMP()
        {
            CreateMap<SliderGetDto, Slider>().ReverseMap();

            CreateMap<SliderCreateDto, Slider>()
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

            CreateMap<SliderUpdateDto, Slider>()
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