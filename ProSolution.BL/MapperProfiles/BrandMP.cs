using AutoMapper;
using ProSolution.BL.DTOs;
using ProSolution.Core.Entities;

namespace ProSolution.BL.MapperProfiles
{
    internal class BrandMP : Profile
    {
        public BrandMP()
        {
            CreateMap<Brand, BrandGetDto>().ForMember(
                destinationMember: dest => dest.ProductCount, // Назначаем в свойство ProductCount...
                memberOptions: opt => opt.MapFrom(src => src.Products.Count)).ReverseMap();

            CreateMap<BrandCreateDto, Brand>()
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

            CreateMap<BrandUpdateDto, Brand>()
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
