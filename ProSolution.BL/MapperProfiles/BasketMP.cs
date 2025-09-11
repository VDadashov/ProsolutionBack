using AutoMapper;
using ProSolution.BL.DTOs;
using ProSolution.Core.Entities;

namespace ProSolution.BL.MapperProfiles;

internal class BasketMP : Profile
{
    public BasketMP()
    {
        CreateMap<Basket, BasketGetDto>().ReverseMap();

        CreateMap<BasketCreateDto, Basket>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember, context) =>
            {
                if (srcMember is string strVal)
                {
                    // "string" literal olaraq gəlirsə, ignore et
                    return strVal != "string";
                }
                return true;
            }));

        CreateMap<BasketUpdateDto, Basket>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember, context) =>
            {
                if (srcMember is string strVal)
                {
                    // "string" literal olaraq gəlirsə, ignore et
                    return strVal != "string";
                }
                return true;
            }));

        CreateMap<BasketItem, BasketItemGetDto>().ReverseMap();

        CreateMap<BasketItemCreateDto, BasketItem>()
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
