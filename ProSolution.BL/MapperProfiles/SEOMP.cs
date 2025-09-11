using AutoMapper;
using ProSolution.BL.DTOs;
using ProSolution.Core.Entities;

namespace ProSolution.BL.MappingProfiles
{
    internal class SEOMP : Profile
    {
        public SEOMP()
        {
            // Create
            CreateMap<CreateSEODTO, SeoData>().ReverseMap();
            CreateMap<SeoDataGetDto, SeoData>().ReverseMap();
            CreateMap<SeoDataUpdateDto, SeoData>().ReverseMap();

            // Update
            CreateMap<UpdateSEODTO, SeoData>().ReverseMap();

            // Read
            CreateMap<SeoData, SEODTO>().ReverseMap();
        }
    }
}
