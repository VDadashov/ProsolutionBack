using AutoMapper;
using ProSolution.BL.DTOs.User.ProSolution.BL.DTOs.Wishlist;
using ProSolution.Core.Entities.Identity;

namespace ProSolution.BL.MapperProfiles
{
    internal class WishListMP : Profile
    {
        public WishListMP()
        {
            CreateMap<WishlistItem, WishlistItemDTO>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.ProductTitle, opt => opt.MapFrom(src => src.Product.Title)) 
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product.Price));
              
        }
    }
}
