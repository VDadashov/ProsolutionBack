using AutoMapper;
using ProSolution.BL.DTOs.Characteristics;
using ProSolution.BL.DTOs.FeatureOPtions;
using ProSolution.Core.Entities;

namespace ProSolution.BL.MapperProfiles
{
    internal class FeatureOptionMP : Profile
    {
        public FeatureOptionMP()
        {
            CreateMap<FeatureOption, FeatureOptionGetDto>().ReverseMap();
            CreateMap<FeatureOptionCreateDto, FeatureOption>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember, context) =>
                {
                    if (srcMember is string strVal)
                    {
                        // "string" literal olaraq gəlirsə, ignore et
                        return strVal != "string";
                    }
                    return true;
                }));
            CreateMap<FeatureOptionUpdateDto, FeatureOption>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember, context) =>
                {
                    if (srcMember is string strVal)
                    {
                        // "string" literal olaraq gəlirsə, ignore et
                        return strVal != "string";
                    }
                    return true;
                }));
            CreateMap<FeatureOption, FeatureOptionIncludeDto>().ReverseMap();
            CreateMap<FeatureOption, FeatureOptionIncludeProductDto>().ReverseMap();

            CreateMap<FeatureOptionItem, FeatureOptionItemGetDto>().ReverseMap();
            CreateMap<FeatureOptionItemCreateDto, FeatureOptionItem>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember, context) =>
                {
                    if (srcMember is string strVal)
                    {
                        // "string" literal olaraq gəlirsə, ignore et
                        return strVal != "string";
                    }
                    return true;
                }));
            CreateMap<FeatureOptionItemUpdateDto, FeatureOptionItem>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember, context) =>
                {
                    if (srcMember is string strVal)
                    {
                        // "string" literal olaraq gəlirsə, ignore et
                        return strVal != "string";
                    }
                    return true;
                }));
            CreateMap<FeatureOptionItem, FeatureOptionItemIncludeDto>()
                .ForMember(dest => dest.Children, opt => opt.MapFrom(src => src.Children))
                .ForMember(dest => dest.FeatureOption, opt => opt.MapFrom(src => src.FeatureOption))
                .ReverseMap();

            CreateMap<FeatureOptionItem, FeatureOptionItemIncludeWithProductDto>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.ProductFeatures.Select(x => x.Product).ToList()))
                .ForMember(dest => dest.Children, opt => opt.MapFrom(src => src.Children)) 
                .ForMember(dest => dest.FeatureOption, opt => opt.MapFrom(src => src.FeatureOption)) 
                .ReverseMap();

            CreateMap<FeatureOptionItem, FeatureOptionItemDto>().ReverseMap();
            CreateMap<FeatureOptionItem, FeatureOptionItemIncludeProductDto>().ReverseMap();
            CreateMap<FeatureOptionItem, FeatureOptionIncludeOptionDto>().ReverseMap();
            CreateMap<FeatureOptionItem, FeatureOptionIncludeOptionProductDto>().ReverseMap();

        }
    }

}
