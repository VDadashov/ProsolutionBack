using AutoMapper;
using ProSolution.BL.DTOs.ProductFeatureKeys;
using ProSolution.Core.Entities;

namespace ProSolution.BL.MapperProfiles
{
    internal class ProductFeatureKeysMP : Profile
    {
        public ProductFeatureKeysMP()
        {
            CreateMap<ProductFeatureKeys, ProductFeatureKeysGetDto>()
                .ForMember(dest => dest.CategoryTitle, opt => opt.MapFrom(src => src.Category.Title))
                .ForMember(dest => dest.FeatureOptions, opt => opt.MapFrom(src => src.FeatureOptions.ToList()));

            CreateMap<ProductFeatureKeysCreateDto, ProductFeatureKeys>()
                .ForMember(dest => dest.FeatureOptions, opt => opt.Ignore()); 

            CreateMap<ProductFeatureKeysUpdateDto, ProductFeatureKeys>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FeatureOptions, opt => opt.Ignore());
        }
    }
}
