using AutoMapper;
using ProSolution.BL.DTOs;
using ProSolution.Core.Entities;

namespace ProSolution.BL.MapperProfiles
{
    internal class ProductMP : Profile
    {
        public ProductMP()
        {
            CreateMap<Product, ProductGetDto>()
           
              .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.CategoryProducts.Select(ta => ta.Category).ToList()))
              .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Title))
              .ForMember(dest => dest.FeatureOptionItems,opt => opt.MapFrom(src => src.ProductFeatures.Select(pf => pf.FeatureOptionItem)));

            CreateMap<ProductCreateDto, Product>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember, context) =>
                {
                    if (srcMember is string strVal)
                    {
                        // "string" literal olaraq gəlirsə, ignore et
                        return strVal != "string";
                    }
                    return true;
                }));

            CreateMap<ProductUpdateDto, Product>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember, context) =>
                {
                    if (srcMember is string strVal)
                    {
                        // "string" literal olaraq gəlirsə, ignore et
                        return strVal != "string";
                    }
                    return true;
                }));

            CreateMap<Product, ProductIncludeDto>().ReverseMap();

            CreateMap<ProductImage, ProductImageGetDto>().ReverseMap();
            CreateMap<ProductImageCreateDto, ProductImage>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember, context) =>
                {
                    if (srcMember is string strVal)
                    {
                        // "string" literal olaraq gəlirsə, ignore et
                        return strVal != "string";
                    }
                    return true;
                }));

            CreateMap<ProductReviewCreateDto, ProductReview>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember, context) =>
                {
                    if (srcMember is string strVal)
                    {
                        // "string" literal olaraq gəlirsə, ignore et
                        return strVal != "string";
                    }
                    return true;
                }));

            CreateMap<ProductReview, ProductReviewIncludeDto>().ReverseMap();

            CreateMap<ProductSlug, ProductSlugGetDto>().ReverseMap();

        }
    }
}
