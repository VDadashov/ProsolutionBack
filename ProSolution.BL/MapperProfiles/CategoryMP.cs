using AutoMapper;
using ProSolution.BL.DTOs;
using ProSolution.BL.DTOs.Categories.CategoryItemDTO;
using ProSolution.Core.Entities;

namespace ProSolution.BL.MapperProfiles
{
    public class CategoryMP : Profile
    {
        public CategoryMP()
        {
            CreateMap<Category, CategoryGetDto>()
                .ForMember(dest => dest.Products, opt => opt.Ignore())
                .ForMember(dest => dest.CategoryItems, opt => opt.MapFrom(src => src.Children))
                .AfterMap((src, dest, context) =>
                {
                    dest.Products = src.CategoryProducts?
                        .Select(cip => context.Mapper.Map<ProductIncludeDto>(cip.Product))
                        .ToList();
                });

            CreateMap<Category, CategoryItemIncludeDto>().ReverseMap();
            CreateMap<CategoryCreateDto, Category>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember, context) =>
                {
                    if (srcMember is string strVal)
                    {
                        // "string" literal olaraq gəlirsə, ignore et
                        return strVal != "string";
                    }
                    return true;
                }));
            CreateMap<CategoryUpdateDto, Category>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember, context) =>
                {
                    if (srcMember is string strVal)
                    {
                        // "string" literal olaraq gəlirsə, ignore et
                        return strVal != "string";
                    }
                    return true;
                }));
            CreateMap<CategoryIncludeDto, Category>().ReverseMap();
            CreateMap<Product, ProductIncludeDto>().ReverseMap();
            CreateMap<CategoryProduct, ProductIncludeDto>().ReverseMap();
            CreateMap<CategoryItemCreateDto, Category>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember, context) =>
                {
                    if (srcMember is string strVal)
                    {
                        // "string" literal olaraq gəlirsə, ignore et
                        return strVal != "string";
                    }
                    return true;
                }));
            CreateMap<CategoryItemUpdateDto, Category>()
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
