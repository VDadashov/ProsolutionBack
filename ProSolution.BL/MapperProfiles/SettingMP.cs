using AutoMapper;
using ProSolution.BL.DTOs;
using ProSolution.Core.Entities;

namespace ProSolution.BL.MapperProfiles
{
    internal class SettingMP : Profile
    {
        public SettingMP()
        {
            CreateMap<Setting, SettingGetDto>().ReverseMap();
            CreateMap<SettingUpdateDto, Setting>().ReverseMap();
            CreateMap<SettingCreateDto, Setting>().ReverseMap();
        }
    }
}
