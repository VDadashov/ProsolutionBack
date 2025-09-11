using AutoMapper;
using ProSolution.BL.DTOs;
using ProSolution.Core.Entities;

namespace ProSolution.BL.MapperProfiles
{
    internal class SEOUrlMP : Profile
    {
        public SEOUrlMP()
        {
            //Create
            CreateMap<CreateSeoUrlDTO, SeoUrl>().ReverseMap();

            //Update
            CreateMap<UpdateSEOUrlDTO, SeoUrl>().ReverseMap();

            //Entity to DTO
            CreateMap<SeoUrl, SEOUrlDTO>().ReverseMap();
        }
    }
}
