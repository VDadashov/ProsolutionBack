using AutoMapper;
using ProSolution.BL.DTOs;
using ProSolution.Core.Entities;

namespace ProSolution.BL.MapperProfiles
{
    internal class BlogMP : Profile
    {
        public BlogMP()
        {
            CreateMap<Blog, BlogGetDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.User.Surname)).ReverseMap();

            CreateMap<Blog, BlogIncludeDto>().ReverseMap();

            CreateMap<BlogCreateDto, Blog>()
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

            CreateMap<BlogUpdateDto, Blog>()
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

            // ✅ Вот нужный маппинг
            CreateMap<BlogReview, BlogReviewGetDto>().ReverseMap();

            CreateMap<Blog, BlogReviewIncludeDto>();
            CreateMap<BlogReviewCreateDto, BlogReview>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember, context) =>
                {
                    if (srcMember is string strVal)
                    {
                        // "string" literal olaraq gəlirsə, ignore et
                        return strVal != "string";
                    }
                    return true;
                }));
            CreateMap<BlogReviewUpdateDto, BlogReview>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember, context) =>
                {
                    if (srcMember is string strVal)
                    {
                        // "string" literal olaraq gəlirsə, ignore et
                        return strVal != "string";
                    }
                    return true;
                }));

            CreateMap<BlogReviewReply, BlogReviewReplyGetDto>().ReverseMap();
            CreateMap<BlogReviewReply, BlogReviewReplyIncludeDto>().ReverseMap();
            CreateMap<BlogReviewReplyCreateDto, BlogReviewReply>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember, context) =>
                {
                    if (srcMember is string strVal)
                    {
                        // "string" literal olaraq gəlirsə, ignore et
                        return strVal != "string";
                    }
                    return true;
                }));
            CreateMap<BlogReviewReply, BlogReviewReplyCreateDto>();

            CreateMap<BlogReviewReplyUpdateDto, BlogReviewReply>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember, context) =>
                {
                    if (srcMember is string strVal)
                    {
                        // "string" literal olaraq gəlirsə, ignore et
                        return strVal != "string";
                    }
                    return true;
                }));
            CreateMap<BlogReviewReply, BlogReviewReplyUpdateDto>();

            CreateMap<BlogReview, BlogReviewIncludeDto>().ReverseMap();

        }


    }
}
