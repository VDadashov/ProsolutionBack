using AutoMapper;
using ProSolution.BL.DTOs;
using ProSolution.Core.Entities;

namespace ProSolution.BL.MapperProfiles
{
    internal class SEOMetaMP : Profile
    {
        public SEOMetaMP()
        {
            // Create
            CreateMap<CreateSEOMetaDTO, SeoMeta>().ReverseMap();

            // Update
            CreateMap<UpdateSEOMetaDTO, SeoMeta>().ReverseMap();

            // Get (List or Single Get)
            CreateMap<SeoMeta, SEOMetaDTO>().ReverseMap();
        }
    }
}
